using CrimsonHunt.Structs;
using ProjectM;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class Event
{
    [Command("event", description: "Toggle exp event.", adminOnly: true)]
    public static void ToggleExpEvent(ChatCommandContext ctx, int expBuff)
    {
        Player _player = new(ctx.Event.SenderUserEntity);
        if (expBuff < 0) return;

        Database.ExpBonus = expBuff;
        var _mes = expBuff == 0 ? $"<color=#ffc905>{_player.Name}</color> has deactivated the <color=#ffc905>Hunt Event</color>!" :
            $"<color=#ffc905>{_player.Name}</color> activated an <color=#ffc905>Hunt Event</color>. EXP: <color=#ffc905>+{expBuff}%</color>";
        ctx.Reply($"Hunt Event {(expBuff == 0 ? "deactivated" : "activated")}!");
        ServerChatUtils.SendSystemMessageToAllClients(Core.EntityManager, _mes);
    }
}
