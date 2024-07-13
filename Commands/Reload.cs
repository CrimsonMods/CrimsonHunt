using CrimsonHunt.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class ReloadCmd
{
    [Command("reload", "rl", description: "Reloads the config of Crimson Hunt.", adminOnly: true)]
    public static void ReloadNotify(ChatCommandContext ctx)
    {
        Plugin.Settings.InitConfig();
        Database.Data().SaveDatabase();
        Database.InitDatabase("player_hunts");
        ctx.Reply($"Crimson Hunt reloaded! {Database.Data().PlayerExp.Count} entries.");
    }
}
