using BepInEx.Logging;
using CrimsonHunt.Hooks;
using Bloodstone.API;
using System.Runtime.CompilerServices;
using Unity.Entities;

namespace CrimsonHunt.Utils;

internal static class Core
{
    public static World Server = VWorld.Server;
    public static EntityManager EntityManager = Server.EntityManager;
    public static ManualLogSource Log = Plugin.LogInstance;
    public static PlayerService Players { get; internal set; }
    public static UnitSpawnerService UnitSpawner { get; internal set; }
    public static PrefabService Prefabs { get; internal set; }

    private static bool _hasInitialized = false;

    public static void LogException(System.Exception e, [CallerMemberName] string caller = null)
    {
        Core.Log.LogError($"Failure in {caller}\nMessage: {e.Message} Inner:{e.InnerException?.Message}\n\nStack: {e.StackTrace}\nInner Stack: {e.InnerException?.StackTrace}");
    }

    internal static void InitializeAfterLoaded()
    {
        if (_hasInitialized) return;

        // TODO: probably changing when I refactor further.
        Players = new();
        UnitSpawner = new();
        Prefabs = new();

        ActionScheduler.StartTimer();
        _hasInitialized = true;
        Log.LogInfo($"{nameof(InitializeAfterLoaded)} completed");
    }

    private static World GetWorld(string name)
    {
        foreach (var world in World.s_AllWorlds)
        {
            if (world.Name == name)
            {
                return world;
            }
        }

        return null;
    }
}
