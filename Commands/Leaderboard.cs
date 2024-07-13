using CrimsonHunt.Structs;
using System.Collections.Generic;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class Leaderboard
{
    [Command("leaderboard", "top", "Returns the top 5 Hunt Scores")]
    public static void GetLeaderboard(ChatCommandContext context)
    {
        List<PlayerStats> top = Database.Data().GetTopScores(5);

        string str = $"BloodHunt Leaderboard:\n";

        for(int i = 0; i < top.Count; i++) 
        {
            str += $"{i + 1}: <color=#ffc905>{top[i].Name}</color> : <color=#ffc905>{top[i].Exp}</color>\n";
        }

        context.Reply(str);
    }
}
