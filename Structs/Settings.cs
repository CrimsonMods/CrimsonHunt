using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CrimsonHunt.Structs
{
    public readonly struct Settings
    {
        private readonly ConfigFile CONFIG;
        private readonly ConfigEntry<bool> ENABLE_MOD;
        private readonly ConfigEntry<bool> ENABLE_BOUNTY;

        public static readonly string CONFIG_PATH = Path.Combine(BepInEx.Paths.ConfigPath, "CrimsonHunt");
        private static readonly string PRESTIGE = Path.Combine(CONFIG_PATH, "hunt_levels.json");

        private static List<HuntLevel> HUNT_LEVELS = new();

        public Settings(ConfigFile config)
        {
            CONFIG = config;
            ENABLE_MOD = CONFIG.Bind("Config", "EnableMod", true, "Enable or disable the mod");
            ENABLE_BOUNTY = CONFIG.Bind("Config", "EnableHunt", true, "Enable or disable v blood hunting");
        }

        public readonly void InitConfig()
        {
            WriteConfig();
            HUNT_LEVELS.Clear();

            string _json = File.ReadAllText(PRESTIGE);
            HUNT_LEVELS = JsonSerializer.Deserialize<List<HuntLevel>>(_json);

            Plugin.LogInstance.LogInfo($"Mod enabled: {ENABLE_MOD.Value}");
        }

        public readonly void WriteConfig()
        {
            if (!Directory.Exists(CONFIG_PATH)) Directory.CreateDirectory(CONFIG_PATH);
            if (!File.Exists(PRESTIGE))
            {
                HUNT_LEVELS.Add(
                    new HuntLevel(
                        1,
                        1000,
                        new HuntReward(),
                        "{player} has reached {level}!"
                        )
                    );
                HUNT_LEVELS.Add(
                    new HuntLevel(
                        2,
                        2000,
                        new HuntReward(),
                        "{player} has reached {level}!"
                        )
                    );

                string _json = JsonSerializer.Serialize(HUNT_LEVELS, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(PRESTIGE, _json);
            }
        }

        public readonly bool GetActiveSystem(Systems type)
        {
            return type switch
            {
                Systems.ENABLE => ENABLE_MOD.Value,
                Systems.BOUNTY => ENABLE_BOUNTY.Value,
                _ => false,
            };
        }

        public string ToggleSystem()
        {
            ENABLE_BOUNTY.Value = !ENABLE_BOUNTY.Value;
            return ENABLE_BOUNTY.Value ? "enabled" : "disabled";
        }

        public static List<HuntLevel> GetLevels()
        {
            return HUNT_LEVELS;
        }
    }
}
