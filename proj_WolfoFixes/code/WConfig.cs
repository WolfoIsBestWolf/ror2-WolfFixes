using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class WConfig
    {

        //public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);
        public static ConfigFile ConfigFile_Client = WolfoLibrary.WConfig.ConfigFile;


        public static ConfigEntry<bool> cfgFixWarpedOSP;
        public static ConfigEntry<bool> cfgTempShopWeight;
        public static ConfigEntry<float> cfgStage1Weight;
        public static ConfigEntry<bool> cfgLoopSeers;

        public static ConfigEntry<bool> cfgTextFixes;

        public static ConfigEntry<bool> cfgItemTags;
        public static ConfigEntry<bool> cfgDevotionSpareDroneParts;
        public static ConfigEntry<bool> cfgXIEliteFix;

        public static ConfigEntry<bool> cfgDisable;
        public static ConfigEntry<bool> cfgDisableGameplay;


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
             "They are in the pool twice, because they are 2 variants. Leading to them being twice as common as other stage 1s with if a weight of 1."
            );
            cfgTempShopWeight = ConfigFile_Client.Bind(
             "Main",
             "Fix Temp Shop Weights",
             true,
             "Temporary shops are 10x more common than intended on:\n-Verdant Falls\n-Reformed Altar\n-Helminth Hatchery\n\nDue to not using standardized chest weights."
          );
            cfgXIEliteFix = ConfigFile_Client.Bind(
             "Main",
             "Elite XI Elite Minions",
             true,
             "This is definitely a overseight, as Ghost XI don't spawn Ghost Alpha either.\n\nBut could be considered too gameplay affecting."
          );
            cfgLoopSeers = ConfigFile_Client.Bind(
               "Main",
               "No Pre-Loop Stage Seers during loops",
               true,
               "Prevent seing Pre-Loop variants from appearing in Lunar Seers, after you have looped.\n\n(ie Shattered Abodes as a Stage 6)\n\nVanilla: false\n\nThis definitely seems like a bug."
            );
            cfgFixWarpedOSP = ConfigFile_Client.Bind(
              "Main",
              "Fix Warped Echo allowing OSP with curse",
              true,
              "One Shot Protection never truly goes away, and Warped Echo can abuse that to make it so you can live even if Curse is meant to remove OSP.\n\n(With Curse, OSP limits damage taken to a 100%, this normally kills you, but Warped Echo splits and reduces this damage, saving you)\n\nThis fix makes it so OSP is just skipped if you would die anyways."
           );


            cfgItemTags = ConfigFile_Client.Bind(
                "Main",
                "Item Tag Changes",
                true,
                "AIBlacklisted:\nNkuhanas Opinion, Kinetic Dampner.\n\nMithrixBlacklisted:\nEgocentrism\n\nUnAiblacklisted:\nFuel Cell, WarHorn, Soulbound Catalyst\n\nBug fixes like Harpoon not being tagged as OnKill still happen even if turned of"
            );
            cfgDevotionSpareDroneParts = ConfigFile_Client.Bind(
                "Main",
                "Devotion Tag",
                true,
                "Add the Devotion Tag to Devoted Lemurians which will make them work with Spare Drone Parts.\n\nThis is a intended synergy, they just put in the wrong Lemurian."
            );


            cfgTextFixes = ConfigFile_Client.Bind(
                "Main",
                "Fixed Descriptions",
                true,
                "Updated and fixed descriptions for items and survivors. Disable if other mods change stats or items."
            );
            cfgDisableGameplay = ConfigFile_Client.Bind(
                "Main",
                "Disable all gameplay fixes",
                false,
                "Disable all gameplay fixes, but leaving visual fixes."
            );
            cfgDisable = ConfigFile_Client.Bind(
                "Main",
                "Disable all fixes",
                false,
                "Disable all fixes, leaving the mod as just a library mod to be used for my other mods and debugging purposes."
           );

        }



        public static void RiskConfig()
        {
            ModSettingsManager.SetModIcon(Addressables.LoadAssetAsync<Sprite>(key: "8d5cb4f0268083645999f52a10c6904b").WaitForCompletion());
            ModSettingsManager.SetModDescription("Random assortment of fixes for bugs that bothered me.");
            AddAllConfigAsRiskConfig();
        }
        public static void AddAllConfigAsRiskConfig()
        {
            //Would need mod meta data passed along from other mods to work fine so probably better to keep stuff as is
            ConfigEntryBase[] entries = ConfigFile_Client.GetConfigEntries();
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    ModSettingsManager.AddOption(new CheckBoxOption((ConfigEntry<bool>)entry, true));
                }
                else if (entry.SettingType == typeof(float))
                {
                    ModSettingsManager.AddOption(new FloatFieldOption((ConfigEntry<float>)entry, true));
                }
                else if (entry.SettingType == typeof(int))
                {
                    ModSettingsManager.AddOption(new IntFieldOption((ConfigEntry<int>)entry, true));
                }
                else if (entry.SettingType.IsEnum)
                {
                    ModSettingsManager.AddOption(new ChoiceOption(entry, true));
                }
                else
                {
                    Log.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

        }


    }

}
