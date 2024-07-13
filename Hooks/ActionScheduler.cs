using CrimsonHunt.Structs;
using System;
using System.Collections.Generic;

namespace CrimsonHunt.Hooks;

internal class ActionScheduler
{
    public static List<Structs.KillEvent> KillEvents = new();
    public static Action action;
    public static DateTime lastDateMinute = DateTime.Now;
    private static DateTime lastDateSecond = DateTime.Now;
    private static Dictionary<string, DateTime> lastCheck = new();

    public static void HandleHuntFrame()
    {
        if (!Plugin.Settings.GetActiveSystem(Systems.ENABLE)) return;
        Database.Data().SaveDatabase();

        for(int i = KillEvents.Count - 1; i >= 0; i--) 
        {
            if (DateTime.Now - KillEvents[i].Killed < TimeSpan.FromSeconds(1)) continue;
            foreach (var player in KillEvents[i].Players)
            {
                Database.Data().UpdateExp(player, KillEvents[i].Boss);
            }
            KillEvents.RemoveAt(i);
        }
    }

    public static void StartTimer()
    {
        Plugin.LogInstance.LogInfo("Start Timner for BloodyBoss");
        action = () =>
        {
            var date = DateTime.Now;
            if (lastDateMinute.ToString("HH:mm") != date.ToString("HH:mm"))
            {
                HandleHuntFrame();
            }
            ActionSchedulerPatch.RunActionOnceAfterFrames(action, 30);
        };
        ActionSchedulerPatch.RunActionOnceAfterFrames(action, 30);

    }
}
