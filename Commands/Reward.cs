using CrimsonHunt.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class Reward
{
    [Command("reward", "r", description: "Give yourself a reward of hunt points", adminOnly: true)]
    public static void GiveScore(ChatCommandContext ctx, int amount)
    {
        Player _player = new(ctx.Event.SenderUserEntity);
        Database.Data().UpdateExp(_player, amount);
    }
}
