using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using CrimsonHunt.Structs;
using HarmonyLib;
using System.Reflection;
using VampireCommandFramework;

namespace CrimsonHunt;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.VampireCommandFramework")]
public class Plugin : BasePlugin
{
    private Harmony _harmony;
    public static Plugin Instance { get; private set; }
    public static Harmony Harmony => Instance._harmony;
    public static ManualLogSource LogInstance => Instance.Log;
    public static Settings Settings { get; private set; }

    public override void Load()
    {
        Instance = this;

        Settings = new (Config);
        Settings.InitConfig();
        Database.InitDatabase("player_hunts");

        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        CommandRegistry.RegisterAll();
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly();
        Config.Clear();
        _harmony?.UnpatchSelf();
        return true;
    }
}
