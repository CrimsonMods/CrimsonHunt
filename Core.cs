using BepInEx.Logging;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using CrimsonHunt.Hooks;
using CrimsonHunt.Utils;
using ProjectM.Physics;
using ProjectM.Scripting;
using System;
using System.Collections;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace CrimsonHunt;

internal static class Core
{
    public static World Server { get; } = GetServerWorld() ?? throw new Exception("There is no Server world (yet)...");
    public static EntityManager EntityManager => Server.EntityManager;
    public static ManualLogSource Log => Plugin.LogInstance;

    public static ServerScriptMapper ServerScriptMapper => Server.GetExistingSystemManaged<ServerScriptMapper>();

    public static PlayerService Players { get; internal set; }
    public static UnitSpawnerService UnitSpawner { get; internal set; }
    public static PrefabService Prefabs { get; internal set; }

    static MonoBehaviour MonoBehaviour;

    public static bool hasInitialized = false;
    public static void Initialize()
    {
        if (hasInitialized) return;

        Players = new PlayerService();
        UnitSpawner = new UnitSpawnerService();
        Prefabs = new PrefabService();

        ActionScheduler.StartTimer();

        hasInitialized = true;
    }

    static World GetServerWorld()
    {
        return World.s_AllWorlds.ToArray().FirstOrDefault(world => world.Name == "Server");
    }

    public static void StartCoroutine(IEnumerator routine)
    {
        if (MonoBehaviour == null)
        {
            MonoBehaviour = new GameObject("CrimsonBanned").AddComponent<IgnorePhysicsDebugSystem>();
            UnityEngine.Object.DontDestroyOnLoad(MonoBehaviour.gameObject);
        }
        MonoBehaviour.StartCoroutine(routine.WrapToIl2Cpp());
    }
}