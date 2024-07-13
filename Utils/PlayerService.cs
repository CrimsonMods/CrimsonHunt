using CrimsonHunt.Structs;
using Bloodstone.API;
using ProjectM;
using ProjectM.Network;
using ProjectM.Shared;
using Stunlock.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Bloody.Core;

namespace CrimsonHunt.Utils;

internal class PlayerService
{
    Dictionary<FixedString64Bytes, PlayerData> NamePlayerCache = new();
    Dictionary<ulong, PlayerData> SteamPlayerCache = new();

    internal bool TryFindSteam(ulong steamId, out PlayerData playerData)
    {
        return SteamPlayerCache.TryGetValue(steamId, out playerData);
    }

    internal bool TryFindName(FixedString64Bytes name, out PlayerData playerData)
    {
        return NamePlayerCache.TryGetValue(name, out playerData);
    }

    internal PlayerService()
    {
        NamePlayerCache.Clear();
        SteamPlayerCache.Clear();
        EntityQuery _query = Core.EntityManager.CreateEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[]
                {
                ComponentType.ReadOnly<User>()
                },
            Options = EntityQueryOptions.IncludeDisabled
        });

        var _userEntities = _query.ToEntityArray(Allocator.Temp);
        foreach (var _entity in _userEntities)
        {
            var _userData = Core.EntityManager.GetComponentData<User>(_entity);
            PlayerData playerData = new(_userData.CharacterName, _userData.PlatformId, _userData.IsConnected, _entity, _userData.LocalCharacter._Entity);

            NamePlayerCache.TryAdd(_userData.CharacterName.ToString().ToLower(), playerData);
            SteamPlayerCache.TryAdd(_userData.PlatformId, playerData);
        }


        var _onlinePlayers = NamePlayerCache.Values.Where(p => p.IsOnline).Select(p => $"\t{p.CharacterName}");
        Core.Log.LogWarning($"Player Cache Created with {NamePlayerCache.Count} entries total, listing {_onlinePlayers.Count()} online:");
        Core.Log.LogWarning(string.Join("\n", _onlinePlayers));
    }

    internal void UpdatePlayerCache(Entity userEntity, string oldName, string newName, bool forceOffline = false)
    {
        var _userData = Core.EntityManager.GetComponentData<User>(userEntity);
        NamePlayerCache.Remove(oldName.ToLower());
        if (forceOffline) _userData.IsConnected = false;
        PlayerData playerData = new(newName, _userData.PlatformId, _userData.IsConnected, userEntity, _userData.LocalCharacter._Entity);

        NamePlayerCache[newName.ToLower()] = playerData;
        SteamPlayerCache[_userData.PlatformId] = playerData;
    }

    internal bool RenamePlayer(Entity userEntity, Entity charEntity, FixedString64Bytes newName)
    {
        var _des = Core.Server.GetExistingSystemManaged<DebugEventsSystem>();
        var _networkId = Core.EntityManager.GetComponentData<NetworkId>(userEntity);
        var _userData = Core.EntityManager.GetComponentData<User>(userEntity);
        var _renameEvent = new RenameUserDebugEvent
        {
            NewName = newName,
            Target = _networkId
        };
        var fromCharacter = new FromCharacter
        {
            User = userEntity,
            Character = charEntity
        };

        _des.RenameUser(fromCharacter, _renameEvent);
        UpdatePlayerCache(userEntity, _userData.CharacterName.ToString(), newName.ToString());
        return true;
    }

    public static User GetUserComponente(Entity userEntity)
    {
        return VWorld.Server.EntityManager.GetComponentData<User>(userEntity);
    }

    public static bool BuffPlayer(
      Entity character,
      Entity user,
      PrefabGUID buff,
      int duration = -1,
      bool persistsThroughDeath = false)
    {
        List<PrefabGUID> _prefabGuidList = new()
      {
        new PrefabGUID(-1703886455),
        new PrefabGUID(-238197495),
        new PrefabGUID(1068709119),
        new PrefabGUID(-1161197991)
      };
        DebugEventsSystem _des = VWorld.Server.GetExistingSystemManaged<DebugEventsSystem>();
        ApplyBuffDebugEvent _adbe = new()
        {
            BuffPrefabGUID = buff
        };
        FromCharacter _from = new()
        {
            User = user,
            Character = character
        };
        if (BuffUtility.TryGetBuff(VWorld.Server.EntityManager, character, buff, out var _entity)) return false;
        _des.ApplyBuff(_from, _adbe);
        if (!BuffUtility.TryGetBuff(VWorld.Server.EntityManager, character, buff, out _entity)) return false;
        if (_prefabGuidList.Contains(buff))
        {
            if (_entity.Has<CreateGameplayEventsOnSpawn>())
                _entity.Remove<CreateGameplayEventsOnSpawn>();
            if (_entity.Has<GameplayEventListeners>())
                _entity.Remove<GameplayEventListeners>();
        }
        if (persistsThroughDeath)
        {
            _entity.Add<Buff_Persists_Through_Death>();
            if (_entity.Has<RemoveBuffOnGameplayEvent>())
                _entity.Remove<RemoveBuffOnGameplayEvent>();
            if (_entity.Has<RemoveBuffOnGameplayEventEntry>())
                _entity.Remove<RemoveBuffOnGameplayEventEntry>();
        }
        if (duration > 0 && duration != -1)
        {
            if (_entity.Has<LifeTime>())
            {
                LifeTime _data = _entity.Read<LifeTime>();
                _data.Duration = duration;
                _entity.Write(_data);
            }
        }
        else if (duration == 0)
        {
            if (_entity.Has<LifeTime>())
            {
                LifeTime _data = _entity.Read<LifeTime>();
                _data.Duration = -1f;
                _data.EndAction = 0;
                _entity.Write(_data);
            }
            if (_entity.Has<RemoveBuffOnGameplayEvent>())
                _entity.Remove<RemoveBuffOnGameplayEvent>();
            if (_entity.Has<RemoveBuffOnGameplayEventEntry>())
                _entity.Remove<RemoveBuffOnGameplayEventEntry>();
        }
        return true;
    }

    public static void UnbuffCharacter(Entity character, PrefabGUID buffGUID)
    {
        if (!BuffUtility.TryGetBuff(VWorld.Server.EntityManager, character, buffGUID, out var _buffEntity)) return;
        DestroyUtility.Destroy(VWorld.Server.EntityManager, _buffEntity, DestroyDebugReason.TryRemoveBuff);
    }
}
