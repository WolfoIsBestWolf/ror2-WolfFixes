using BepInEx;
using BepInEx.Configuration;

namespace SpawnPoolFixer
{
    public class WConfig
    {
        public static ConfigFile ConfigFile = new ConfigFile(Paths.ConfigPath + "\\Wolfo.SpawnPoolFixer.cfg", true);

        public static ConfigEntry<float> cfgStage1Weight;
        public static ConfigEntry<bool> cfgSotV_EnemyRemovals;
        public static ConfigEntry<bool> WORMREMOVAL;

        public static ConfigEntry<bool> cfgLoopSeers;
    
        public static void InitConfig()
        {
            cfgStage1Weight = ConfigFile.Bind(
               "Config",
               "Plains Roost weight",
               0.75f,
               "They are counted as 2 stages due to the 2 variants, so they essentially have double the weight."
            );
            cfgSotV_EnemyRemovals = ConfigFile.Bind(
               "Config",
               "SotV enemy removals",
                true,
               "SotV replaced various enemies with new enemies.\nThis feature was removed in the new spawn pool system."
            );
            WORMREMOVAL = ConfigFile.Bind(
               "Config",
               "SotV Meadows Worm removal",
                false,
               "SotV replaced Magma Worms on Sky Meadows with XI Construct. "
            );

            cfgLoopSeers = ConfigFile.Bind(
               "Config",
               "Remove Pre Loop Destination from Seers during loops",
               true,
               "You can get PreLoop variants of stages that have loop variants as Lunar Seer Destinations.\nVanilla : True\nThis was confirmed a funny quirk so it probably wont be removed.\nBut it looks like a bug so this removes it"
            );
     

        }

    }
}