﻿using BepInEx;
using HG;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using UnityEngine;
using WolfoFixes.Testing;

namespace WolfoFixes
{
    
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Early.Wolfo.WolfFixes", "WolfoBugFixes", version)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoMain : BaseUnityPlugin
    {
        const string version = "1.1.15";

        public static bool riskyFixes;
        public void Awake()
        {
            WConfig.Awake();
            
        }

 
        public void Start()
        {
            if (WConfig.cfgLoadOrder.Value)
            {
                TestLoadOrder.Logger = base.Logger;
            }
            Commands.Start();
            Test.Start();
            RoR2Application.onLoad += addRiskConfigLatest;

            riskyFixes = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyFixes");
            VoidSuppressor.SuppresedScrap();
            if (WConfig.cfgDisable.Value)
            {
                return;
            }
            VoidSuppressor.FixInteractable();
            VoidElite.VoidAffix();

            BodyFixes.Start();
            DevotionFixes.Start();
            GameplayMisc.Start();
            InteractableFixes.Start();
            ItemFixes.Start();
            EquipmentFixes.Start();
            ItemTags.Start();
            PrayerBeads.Start();
            Simualcrum.Start();

            Audio.Start();
            RandomFixes.Start();
   
            LogFixes.Start();
 
            OptionPickupStuff.Start();
            SkinFixes.Start();
            Visuals.Start();
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
            EntityStates.FalseSon.LaserFather.laserPrefab = null;
 
            DLC2Content.Items.SpeedBoostPickup.tags = DLC2Content.Items.SpeedBoostPickup.tags.Remove(ItemTag.DevotionBlacklist);
        }

        internal static void ModSupport_CallLate()
        {
            if (WConfig.cfgDisable.Value)
            {
                return;
            }


            //The later the better because first come first serve with LanguageAPI
            //Tho could do it with a language file I guess?
            TextFixes.CallLate();
            ItemTags.CallLate();
            BodyFixes.CallLate();
            Simualcrum.CallLate();

            RoR2Content.Buffs.HiddenInvincibility.canStack = false;
            DLC2Content.Buffs.Boosted.canStack = false;
            
            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";

            //Prevent scrapping regen scrap.
            HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.RegeneratingScrap.tags, ItemTag.Scrap);

        }



    }

}