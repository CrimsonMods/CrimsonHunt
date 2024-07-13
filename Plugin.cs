using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using VampireCommandFramework;
using Bloodstone.API;
using BepInEx.Logging;
using CrimsonHunt.Structs;
using CrimsonHunt.Hooks;
using CrimsonHunt.Utils;

namespace CrimsonHunt;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.VampireCommandFramework")]
[BepInDependency("gg.deca.Bloodstone")]
[Bloodstone.API.Reloadable]
public class Plugin : BasePlugin, IRunOnInitialized
{
    private Harmony _harmony;
    public static ManualLogSource LogInstance {  get; private set; }
    public static Settings Settings { get; private set; }

    public override void Load()
    {
        LogInstance = Log;
        Settings = new (Config);
        Settings.InitConfig();
        Database.InitDatabase("player_hunts");

        if (!VWorld.IsServer) 
        {
            Log.LogWarning("This plugin is a server-only plugin.");
        }

        CommandRegistry.RegisterAll();
        //Bloodstone.Hooks.GameFrame.OnUpdate += ActionScheduler.HandleHuntFrame;
    }

    public void OnGameInitialized()
    {
        if (VWorld.IsClient) return;

        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();

        Core.InitializeAfterLoaded();
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly();
        Config.Clear();
        _harmony?.UnpatchSelf();
        Bloodstone.Hooks.GameFrame.OnUpdate -= ActionScheduler.HandleHuntFrame;
        return true;
    }
}
