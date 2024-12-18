using CrimsonHunt.Structs;
using CrimsonHunt.Utils;
using ProjectM;
using ProjectM.Network;
using Stunlock.Core;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class ToggleEffectCmd
{
    [Command("mode", description: "Toggle crimson hunt effects.", adminOnly: false)]
    public static void ToggleHuntEffects(ChatCommandContext ctx)
    {
        var _manager = Core.EntityManager;
        Player _player = new(ctx.Event.SenderUserEntity);
        var _exp = Database.Data().GetExp(_player);
        var _prestige = Database.GetPrestige(_exp);

        if (_prestige.Level == 0)
        {
            ServerChatUtils.SendSystemMessageToClient(_manager, _player.User.Read<User>(), $"You dont have any hunt score [Lvl: <color=#ffc905>{_prestige.Level}</color>]");
            return;
        }

        foreach (var _level in Settings.GetLevels())
        {
            if (_level.HuntReward.EffectHash == 0) continue;

            if (_level.Level <= _prestige.Level)
            {
                if (BuffUtility.HasBuff(_manager, _player.Character, new PrefabGUID(_level.HuntReward.EffectHash)))
                {
                    PlayerService.UnbuffCharacter(_player.Character, new PrefabGUID(_level.HuntReward.EffectHash));
                    ServerChatUtils.SendSystemMessageToClient(_manager, _player.User.Read<User>(), $"Your hunt effect [Rank: <color=#ffc905>{_level.Level}</color>] was removed!");
                }
                else
                {
                    PlayerService.BuffPlayer(_player.Character, _player.User, new PrefabGUID(_level.HuntReward.EffectHash), 0, true);
                    ServerChatUtils.SendSystemMessageToClient(_manager, _player.User.Read<User>(), $"Your hunt effect [Rank: <color=#ffc905>{_level.Level}</color>] was added!");
                }
            }
        }
    }
}
