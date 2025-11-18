using BepInEx;
using BepInEx.Logging;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoLibrary;

namespace WolfoFixes
{

    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfFixes", "WolfoBugFixes", "1.2.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfFixes : BaseUnityPlugin
    {
        public static ManualLogSource log;

        public static bool riskyFixes;
        public void Awake()
        {
            WConfig.Awake();
            log = base.Logger;
            Assets.Init(base.Info);
        }


        public void Start()
        {
            log.LogMessage("Start");
            RoR2Application.onLoad += addRiskConfigLatest;
            if (WConfig.cfgDisable.Value)
            {
                WolfFixes.log.LogMessage("WolfoFixes disabled");
                return;
            }

            riskyFixes = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyFixes");

            BodyFixes.Start();
            DevotionFixes.Start();
            GameplayMisc.Start();
            InteractableFixes.Start();
            ItemFixes.Start();
            EquipmentFixes.Start();
            DCCS_SpawnPool.Main();

            PrayerBeads.Start();
            Simualcrum.Start();
            Visuals.Start();
            TextFixes.Start();
            Audio.Start();
            RandomFixes.Start();

            LogFixes.Start();

            OptionPickupStuff.Start();
            SkinFixes.Start();

            EquipmentCatalog.availability.CallWhenAvailable(MissingEliteDisplays.Start);

            GameModeCatalog.availability.CallWhenAvailable(ModSupport_CallLate);
            ExtraActions.Start();




        }

        void addRiskConfigLatest()
        {
            RoR2Application.onLoad -= addRiskConfigLatest;
            bool riskOfOptions = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
            if (riskOfOptions)
            {
                WConfig.RiskConfig();
            }

        }

        internal static void ModSupport_CallLate()
        {
            if (WConfig.cfgDisable.Value)
            {
                return;
            }

            //Only ever possible to get 1 stack so shouldn't show x1.
            DLC2Content.Buffs.Boosted.canStack = false;

            //Dont allow in Tinker, because they do NOT do anything.
            DLC3Content.Buffs.AccelerantIgnited.flags = 0;
            DLC3Content.Buffs.Electrocuted.flags = 0;
            DLC3Content.Buffs.Conductive.flags = 0;

            //Fix missing sprite
            DLC3Content.Buffs.EliteCollective.iconSprite = Addressables.LoadAssetAsync<Sprite>(key: "17a3451326822bf45b656c26443cb530").WaitForCompletion();




            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";

            //Prevent scrapping regen scrap.
            Tags.AddTag(DLC1Content.Items.RegeneratingScrap, ItemTag.Scrap);

            ItemTags.CallLate();

            Simualcrum.CallLate();

            //Fix color having alpha of 0.
            RoR2Content.DroneDefs.MissileDrone.bodyPrefab.GetComponent<CharacterBody>().bodyColor.a = 1;
            RoR2Content.DroneDefs.MissileDrone.remoteOpBody.GetComponent<CharacterBody>().bodyColor.a = 1;

        }



    }

}