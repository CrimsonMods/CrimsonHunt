using CrimsonHunt.Structs;
using ProjectM;
using System.Text;
using VampireCommandFramework;

namespace CrimsonHunt.Commands;

[CommandGroup("crimsonhunt", "bloodhunt")]
internal class ActivateDeactivate
{
    [Command("toggle", "t", description: "Activates / Deactivates the plugin.", adminOnly: true)]
    public static void ToggleSystem(ChatCommandContext ctx, string reason)
    {
        string _status = Plugin.Settings.ToggleSystem();
        Player _admin = new(ctx.Event.SenderUserEntity);
        string[] _reasons = reason.Split("#");

        StringBuilder _message = new();
        _message.Append($"The Crimson Hunt system was <color=#ffc905>{_status}</color> by <color=#ffc905>{_admin.Name}</color>!");

        if (!Plugin.Settings.GetActiveSystem(Systems.BOUNTY))
        {
            _message.Append($" Reason: <color=#cc2936>");
            for (int i = 0; i < _reasons.Length; i++)
            {
                _message.Append(_reasons[i]);
                if (i < _reasons.Length - 1)
                {
                    _message.Append(' ');
                }
            }
            _message.Append("</color>.");
        }

        ServerChatUtils.SendSystemMessageToAllClients(Core.EntityManager, _message.ToString());
    }
}
