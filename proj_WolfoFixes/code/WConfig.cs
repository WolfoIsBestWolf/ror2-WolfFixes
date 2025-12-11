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


        public static ConfigEntry<bool> cfgTempShopWeight;
        public static ConfigEntry<float> cfgStage1Weight;
        public static ConfigEntry<bool> cfgLoopSeers;

        public static ConfigEntry<bool> cfgTextFixes;
 
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
             "Main",
             "Plains Roost weight",
             0.75f,
             "They are counted as 2 stages due to the 2 variants, so they essentially have double the weight."
          );
            cfgTempShopWeight = ConfigFile_Client.Bind(
             "Main",
             "Fix Temp Shop Weights",
             true,
             "Temporary shops are 10x more common than intended on Verdant Falls, Reformed Altar, Helminth Hatchery"
          );
            cfgLoopSeers = ConfigFile_Client.Bind(
               "Main",
               "No Pre-Loop Stage Seers during loops",
               true,
               "You can get PreLoop variants of stages that have loop variants as Lunar Seer Destinations.\nVanilla : True\nAt least a dev thought it was a funny quirk but still just feels like a bug."
            );

 
            cfgItemTags = ConfigFile_Client.Bind(
                "Main",
                "Item Tag Changes",
                true,
                "This may slightly affect balance of Mithrix Phase 4, if you think playing around items that should be blacklisted is fun then go ahead and disable it.\n\nAIBlacklist Nkuhanas Opinion and Infusion.\nMoves around certain categories on items.\nBug fixes like Harpoon not being tagged as OnKill still happen even if turned of"
            );
            cfgDevotionSpareDroneParts = ConfigFile_Client.Bind(
                "Main",
                "Devotion Tag",
                true,
                "Add the Devotion Tag to Devoted Lemurians which will make them work with Spare Drone Parts.\n\nThis is a intended synergy, they just put in the wrong Lemurian."
            );

            cfgDisable = ConfigFile_Client.Bind(
                          "Main",
                          "Disable all fixes",
                          false,
                          "Disable all fixes leaving the mod as just a library mod to be used for my other mods and debugging purposes."
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

 
            ConfigEntryBase[] entries = ConfigFile_Client.GetConfigEntries();
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp,true));
                }
                 if (entry.SettingType == typeof(float))
                {
                    var temp = (ConfigEntry<float>)entry;
                    ModSettingsManager.AddOption(new FloatFieldOption(temp, true));
                }
                else
                {
                    WolfFixes.log.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

        }

    }

}
