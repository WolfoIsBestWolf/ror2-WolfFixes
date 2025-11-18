using BepInEx;
using BepInEx.Configuration;

namespace SpawnPoolFixer
{
    public class WConfig
    {
        public static ConfigFile ConfigFile = new ConfigFile(Paths.ConfigPath + "\\Wolfo.SpawnPoolFixer.cfg", true);
        public static ConfigEntry<bool> cfgSotV_EnemyRemovals;

        public static void InitConfig()
        {
          
            cfgSotV_EnemyRemovals = ConfigFile.Bind(
               "Config",
               "SotV enemy removals",
                true,
               "SotV replaced various enemies with new enemies.\nThis feature was removed in the new spawn pool system."
            );

     

        }

    }
}