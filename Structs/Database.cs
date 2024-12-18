using CrimsonHunt.Utils;
using ProjectM;
using ProjectM.Network;
using Stunlock.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Unity.Entities;

namespace CrimsonHunt.Structs;

public class Database
{
    private static Database INSTANCE = null;
    public string DbPath { get; set; }
    public Dictionary<ulong, PlayerStats> PlayerExp { get; set; }
    public DateTime Dumped { get; set; }
    public static int ExpBonus = 0;

    public Database(string _path, string _fileName)
    {
        DbPath = Path.Combine(_path, $"{_fileName}.json");
        PlayerExp = new Dictionary<ulong, PlayerStats>();
        Dumped = DateTime.Now;
    }

    public static void InitDatabase(string fileName)
    {
        var _path = Path.Combine(BepInEx.Paths.ConfigPath, "CrimsonHunt");
        INSTANCE = new Database(_path, fileName);
        if (File.Exists(INSTANCE.DbPath))
        {
            string _json = File.ReadAllText(INSTANCE.DbPath);
            try
            {
                INSTANCE.PlayerExp = JsonSerializer.Deserialize<Dictionary<ulong, PlayerStats>>(_json);
            }
            catch (Exception ex) 
            {
                Plugin.LogInstance.LogWarning(ex.Message);
            }
        }
        Plugin.LogInstance.LogMessage($"Database loaded: {INSTANCE.PlayerExp.Count} entries.");
    }

    public static Database Data()
    {
        return INSTANCE;
    }

    public float GetExp(Player player)
    {
        bool _exists = PlayerExp.TryGetValue(player.SteamID, out var _data);
        return _exists ? _data.Exp : 0F;
    }

    public void UpdateExp(Player player, Entity boss)
    {
        // boss = 79; player = 86
        int _bossLvl = boss.Read<UnitLevel>().Level;
        Plugin.LogInstance.LogDebug($"Player defeated a boss with level {_bossLvl}");
        int _playerLvl = (int)player.Character.Read<Equipment>().GetFullLevel();

        // 88 - 70 = 18
        //int _dif = _playerLvl - _bossLvl;
        //if (_dif > 18) return;

        float _addExp = _bossLvl / 10.0F;
        Plugin.LogInstance.LogDebug($"Add {_addExp} to player");
        if (ExpBonus != 0) _addExp *= 1.0F + (ExpBonus / 100.0F);

        UpdateExp(player, (int)_addExp);
    }

    public void UpdateExp(Player player, int value)
    {
        bool _exists = PlayerExp.TryGetValue(player.SteamID, out var _);
        int _maxExp = Settings.GetLevels()
            .OrderByDescending(x => x.ExpNeeded)
            .ToList()
            .First()
            .ExpNeeded;
        PlayerStats _stats;

        if (_exists)
        {
            _stats = PlayerExp[player.SteamID];
            _stats.Name = player.Name;
            _stats.IsAdmin = player.IsAdmin;
            _stats.Exp = (_stats.Exp + value > _maxExp) ? _maxExp : (_stats.Exp + value);
        }
        else
        {
            _stats = new()
            {
                Name = player.Name,
                IsAdmin = player.IsAdmin,
                Exp = value,
            };
        }

        var _prestige = GetPrestige(_stats.Exp);
        int _curLevel = _stats.Level;
        _stats.Level = _prestige.Level;

        if (_exists) PlayerExp[player.SteamID] = _stats;
        else PlayerExp.Add(player.SteamID, _stats);

        CheckForItemRewards(player, _curLevel, _prestige);
        AddPrestigeBuff(player, _stats, _prestige, _curLevel);

        ServerChatUtils.SendSystemMessageToClient(Core.EntityManager, player.User.Read<User>(),
            $"Hunt Score [Lvl: <color=#ffc905>{_prestige.Level}</color> ~ (<color=#ffc905>{_stats.Exp.ToString("0.##", CultureInfo.InvariantCulture)}</color>/<color=#ffc905>{_prestige.ExpNeeded}</color>)]");
        }

    private static void CheckForItemRewards(Player player, int curLevel, HuntLevel newLevel)
    {
        if (newLevel.Level <= curLevel) return;

        var gainedLevels = Settings.GetLevels().OrderBy(x => x.Level).ToList();

        foreach (var level in gainedLevels)
        {
            if (level.Level <= curLevel || level.Level > newLevel.Level) continue;

            if (level.HuntReward.ItemHash != 0)
            {
                PrefabGUID reward = new PrefabGUID(level.HuntReward.ItemHash);
                PlayerService.TryAddUserInventoryItem(player.Character, reward, level.HuntReward.ItemQuantity);
                ServerChatUtils.SendSystemMessageToClient(Core.EntityManager, player.User.Read<User>(),
                     $"Hunt Level [Lvl: <color=#ffc905>{level.Level}</color> ~ Reward: (<color=#ffc905>{level.HuntReward.ItemQuantity} {reward.LookupName()}</color>");
            }
        }
    }

    private static void AddPrestigeBuff(Player player, PlayerStats stats, HuntLevel prestige, int curLevel)
    {
        if (prestige.Level > curLevel)
        {
            var _manager = Core.EntityManager;

            if (!BuffUtility.HasBuff(_manager, player.Character, new PrefabGUID(-1133938228)))
                PlayerService.BuffPlayer(player.Character, player.User, new PrefabGUID(-1133938228), 2, false);

            string _mes = prestige.Message
                .Replace("{player}", player.Name)
                .Replace("{level}", prestige.Level.ToString());
            ServerChatUtils.SendSystemMessageToAllClients(_manager, _mes);

            var _sort = Settings.GetLevels().OrderBy(x => x.Level).ToList();

            foreach (var _lvl in _sort)
            {
                if (_lvl.HuntReward.EffectHash == 0) continue;

                if (stats.Level > _lvl.Level && BuffUtility.HasBuff(_manager, player.Character, new PrefabGUID(_lvl.HuntReward.EffectHash)))
                {
                    PlayerService.UnbuffCharacter(player.Character, new PrefabGUID(_lvl.HuntReward.EffectHash));
                }
                if (stats.Level == _lvl.Level)
                {
                    if (!BuffUtility.HasBuff(_manager, player.Character, new PrefabGUID(_lvl.HuntReward.EffectHash)))
                        PlayerService.BuffPlayer(player.Character, player.User, new PrefabGUID(_lvl.HuntReward.EffectHash), 0, true);
                    break;
                }
            }
        }
    }

    public static HuntLevel GetPrestige(float exp)
    {
        var _sort = Settings.GetLevels().OrderBy(x => x.Level).ToList();

        HuntLevel _lvl = new()
        {
            Level = 0,
            ExpNeeded = 0,
            Message = ""
        };

        for (int i = 0; i < _sort.Count; i++)
        {
            _lvl.ExpNeeded = _sort.ElementAt(i).ExpNeeded;

            if (exp >= _sort.ElementAt(i).ExpNeeded)
            {
                _lvl.Level = _sort.ElementAt(i).Level;
                _lvl.Message = _sort.ElementAt(i).Message;
                continue;
            }
            break;
        }
        return _lvl;
    }

    public void SaveDatabase(bool check = true)
    {
        if (DateTime.Now - Dumped < TimeSpan.FromMinutes(1) && check) return;
        DateTime _now = DateTime.Now;
        string _json = JsonSerializer.Serialize(PlayerExp, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DbPath, _json);
        //Plugin.LogInstance.LogMessage($"Blood Hunt database dumped {_now.ToString()}.");
        Dumped = _now;
    }

    //========= UTILITY FUNCTIONS ==========
    public List<PlayerStats> GetTopScores(int amount)
    {
        return PlayerExp.Values
                .OrderByDescending(x => x.Exp)
                .Take(amount)
                .ToList();
    }
}


public struct PlayerStats
{
    public string Name { get; set; }
    public bool IsAdmin { get; set; }
    public float Exp { get; set; }
    public int Level { get; set; }
}

