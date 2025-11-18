using BepInEx;
using BepInEx.Configuration;

namespace WolfoLibrary
{
    public class WConfig
    {

        public static ConfigFile ConfigFile = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);

        public static ConfigEntry<bool> cfgTestMultiplayer;
        public static ConfigEntry<bool> cfgTestLogbook;
        public static ConfigEntry<bool> cfgLoadOrder;

        public static void Awake()
        {
            InitConfig();
        }


        public static void InitConfig()
        {


            cfgTestLogbook = ConfigFile.Bind(
               "Testing",
               "Everything Logbook",
               false,
               "Add all items, equipments and mobs to logbook, including untiered and cut."
            );
            cfgTestMultiplayer = ConfigFile.Bind(
                "Testing",
                "Multiplayer Test",
                false,
                "Allows you to join yourself via connect localhost:7777"
            );
            cfgLoadOrder = ConfigFile.Bind(
                 "Testing",
                 "Debugging",
                 false,
                 "Log prints when certain events happen"
             );

        }





    }

}
