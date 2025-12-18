using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Rendering;
using WolfoLibrary.Testing;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;

namespace WolfoLibrary
{

    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoLibrary", "WolfoLibrary", "1.3.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoLib : BaseUnityPlugin
    {
        public static ManualLogSource log;

        public void Awake()
        {
            log = base.Logger;
            Commands.Awake();
            Test.Awake();
        }


        public void Start()
        {
            Commands.cheatsEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("iHarbHD.DebugToolkit");

            WConfig.Awake();
            Test.Start();
            ExtraActions.Start();
            Tags.Start();

            VoidElite.VoidAffix();
            VoidSuppressor.SuppresedScrap();
            VoidSuppressor.FixInteractable();
           
        }


     

    }

}