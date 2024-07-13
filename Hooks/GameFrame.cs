﻿using HarmonyLib;

namespace CrimsonHunt.Hooks;

[HarmonyPatch]
internal class GameFrame_Hook
{
    [HarmonyPatch("ProjectM.GameBootstrap", "Start")]
    [HarmonyPostfix]
    public static void ServerDetours()
    {
        Plugin.LogInstance.LogInfo("Game has bootstrapped. Worlds and systems now exist.");
        // GameFrame.Initialize();
    }
}
