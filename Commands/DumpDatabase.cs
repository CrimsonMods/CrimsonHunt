using CrimsonHunt.Structs;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class DumpDatabase
{
    [Command("dumpdb", "ddb", description: "Dumps the database manually.", adminOnly: true)]
    public static void Dump(ChatCommandContext ctx)
    {
        Database.Data().SaveDatabase(false);
        ctx.Reply($"Database Dumped! {Database.Data().PlayerExp.Count} entries");
    }
}
