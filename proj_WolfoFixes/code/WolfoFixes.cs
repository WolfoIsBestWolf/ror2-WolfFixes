using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoLibrary;

namespace WolfoFixes
{

    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfFixes", "WolfoBugFixes", "1.3.0")]
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
            riskyFixes = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyFixes");

           
        }


        public void Start()
        {
            log.LogMessage("Start");
            RoR2Application.onLoad += addRiskConfigLatest;

            //POT MOBILE JUMP SCARE
            Addressables.LoadAssetAsync<GameObject>(key: "525b404e87c469f4ab8034de0913d11a").WaitForCompletion().transform.GetChild(4).gameObject.SetActive(false);


            if (WConfig.cfgDisable.Value)
            {
                WolfFixes.log.LogMessage("WolfoFixes disabled");
                return;
            }
         

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



            Language.onCurrentLanguageChanged += Language_onCurrentLanguageChanged;

            On.RoR2.BuffCatalog.Init += BuffCatalog_Init;

            log.LogMessage("Test");
            log.LogWarning("Test");
            log.LogError("Test");

        }



        private void BuffCatalog_Init(On.RoR2.BuffCatalog.orig_Init orig)
        {   try
            {
                DLC2Content.Buffs.TeleportOnLowHealthActive.isCooldown = false;
                DLC2Content.Buffs.TeleportOnLowHealthVictim.isCooldown = false;
                 
                //Dont allow in Tinker, because they do NOT do anything.
                DLC3Content.Buffs.AccelerantIgnited.flags = BuffDef.Flags.ExcludeFromNoxiousThorns;
                DLC3Content.Buffs.Electrocuted.flags = BuffDef.Flags.ExcludeFromNoxiousThorns;
                DLC3Content.Buffs.Conductive.flags = 0;

                DLC3Content.Buffs.Accelerant.ignoreGrowthNectar = true;
                DLC3Content.Buffs.AccelerantIgnited.ignoreGrowthNectar = true;
                DLC3Content.Buffs.Brittle.isDebuff = false;
                DLC3Content.Buffs.Conductive.ignoreGrowthNectar = true;
                DLC3Content.Buffs.Electrocuted.ignoreGrowthNectar = true;
                DLC3Content.Buffs.GravitySlow.ignoreGrowthNectar = true;
                DLC3Content.Buffs.InventoryDisable.ignoreGrowthNectar = true;
                DLC3Content.Buffs.Jailed.ignoreGrowthNectar = true;
                DLC2Content.Buffs.KnockUpHitEnemiesJuggleCount.ignoreGrowthNectar = true;
                DLC3Content.Buffs.NanoBug.ignoreGrowthNectar = true;
                DLC3Content.Buffs.SolusWingWeakpointDestroyed.ignoreGrowthNectar = true;
                DLC3Content.Buffs.Taunted.ignoreGrowthNectar = true;
                DLC3Content.Buffs.Underclock.ignoreGrowthNectar = true;
                DLC3Content.Buffs.VultureRoot.ignoreGrowthNectar = true;

            }
            catch { }


            orig();
        }

        private void Language_onCurrentLanguageChanged()
        {
     
            Language.onCurrentLanguageChanged -= Language_onCurrentLanguageChanged;
            if (!riskyFixes && Language.currentLanguage.TokenIsRegistered("_ITEM_BEARVOID_DESC"))
            {
                LanguageAPI.Add("ITEM_BEARVOID_DESC", Language.GetString("_ITEM_BEARVOID_DESC"));
            }
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
 

            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";

            //Prevent scrapping regen scrap.
            //Tags.AddTag(DLC1Content.Items.RegeneratingScrap, ItemTag.Scrap);
            var pdtUnscrappableItems = Addressables.LoadAssetAsync<ExplicitPickupDropTable>(key: "6db5e5eb0ec0c394da95229ad89cea29").WaitForCompletion();
            HG.ArrayUtils.ArrayAppend(ref pdtUnscrappableItems.pickupEntries, new ExplicitPickupDropTable.PickupDefEntry()
            {
                pickupDef = DLC1Content.Items.RegeneratingScrap,
            });

            ItemTags.CallLate();

            Simualcrum.CallLate();

            //Fix color having alpha of 0.
            RoR2Content.DroneDefs.MissileDrone.bodyPrefab.GetComponent<CharacterBody>().bodyColor.a = 1;
            RoR2Content.DroneDefs.MissileDrone.remoteOpBody.GetComponent<CharacterBody>().bodyColor.a = 1;

        }



    }

}