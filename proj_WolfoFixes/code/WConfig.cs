using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class WConfig
    {

        //public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);
        public static ConfigFile ConfigFile_Client = WolfoLibrary.WConfig.ConfigFile;


        public static ConfigEntry<bool> cfgLava;
        public static ConfigEntry<float> cfgStage1Weight;
        public static ConfigEntry<bool> cfgLoopSeers;

        public static ConfigEntry<bool> cfgTextFixes;

        public static ConfigEntry<bool> cfgMithrix4Skip;

        public static ConfigEntry<bool> cfgItemTags;
        public static ConfigEntry<bool> cfgDevotionSpareDroneParts;

        public static ConfigEntry<bool> cfgDisable;


        public static void Awake()
        {

            InitConfig();

        }


        public static void InitConfig()
        {

            cfgStage1Weight = ConfigFile_Client.Bind(
             "Config",
             "Plains Roost weight",
             0.75f,
             "They are counted as 2 stages due to the 2 variants, so they essentially have double the weight."
          );
            cfgLoopSeers = ConfigFile_Client.Bind(
               "Config",
               "Remove Pre Loop Destination from Seers during loops",
               true,
               "You can get PreLoop variants of stages that have loop variants as Lunar Seer Destinations.\nVanilla : True\nAt least a dev thought it was a funny quirk but still just feels like a bug."
            );



            cfgMithrix4Skip = ConfigFile_Client.Bind(
               "Gameplay",
               "Fix Mithrix P4 Skip",
               false,
               "This bug happens because SetStateOnHurt consider Mithrixes health 0 for 1 frame and staggers him.\n\nOff by default due to feedback."
           );
            cfgMithrix4Skip.SettingChanged += BodyFixes.SetSkippable;

            //Is he tho?
            /*cfgFalseSonP2 = ConfigFile_Client.Bind(
                "Gameplay",
                "Fix False Son P2 Shotgun",
                true,
                "False Son Phase 2 is intended to use the spike shotgun, but can't due to a bugged skill driver."
            );*/

            cfgItemTags = ConfigFile_Client.Bind(
                "Gameplay",
                "Item Tag Changes",
                true,
                "AIBlacklist Nkuhanas Opinion and Infusion.\nMoves around certain categories on items.\nBug fixes like Harpoon not being tagged as OnKill still happen even if turned of"
            );
            cfgDevotionSpareDroneParts = ConfigFile_Client.Bind(
                "Gameplay",
                "Devotion Tag",
                true,
                "Add the Devotion Tag to Devoted Lemurians which will make them work with Spare Drone Parts.\n\nThis is a intended synergy, they just put in the wrong Lemurian."
            );

            cfgDisable = ConfigFile_Client.Bind(
                          "Main",
                          "Disable everything except needed",
                          false,
                          "Disable all changes except things needed for mod compatibility and debugging tools"
            );
            cfgTextFixes = ConfigFile_Client.Bind(
                "Main",
                "Fixed Descriptions",
                true,
                "Updated and fixed descriptions for items and survivors. Disable if other mods change stats or items."
            );



        }



        public static void RiskConfig()
        {
            ModSettingsManager.SetModIcon(Addressables.LoadAssetAsync<Sprite>(key: "8d5cb4f0268083645999f52a10c6904b").WaitForCompletion());
            ModSettingsManager.SetModDescription("Random assortment of fixes for bugs that bothered me.");

            List<ConfigEntry<bool>> noResetB = new List<ConfigEntry<bool>>()
            {
                cfgMithrix4Skip
            };

            ConfigEntryBase[] entries = ConfigFile_Client.GetConfigEntries();
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp, !noResetB.Contains(temp)));
                }
                else
                {
                    WolfFixes.log.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

        }

    }

}
