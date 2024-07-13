using CrimsonHunt.Structs;
using System.Globalization;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class Score
{
    [Command("score", description: "Get your current hunt score", adminOnly: false)]
    public static void GetScore(ChatCommandContext context)
    {
        Player _player = new(context.Event.SenderUserEntity);
        var _exp = Database.Data().GetExp(_player);
        var _prestige = Database.GetPrestige(_exp);

        context.Reply($"Hunt Score progress [Lvl: <color=#ffc905>{_prestige.Level}</color> ~ (<color=#ffc905>{_exp.ToString("0.##", CultureInfo.InvariantCulture)}</color>/<color=#ffc905>{_prestige.ExpNeeded}</color>)]");
    }
}
