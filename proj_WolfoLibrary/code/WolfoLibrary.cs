using BepInEx;
using BepInEx.Logging;
using R2API.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2;
using WolfoLibrary.Testing;
//[assembly: NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
[assembly: NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.DifferentModVersionsAreOk)]
[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace WolfoLibrary
{

    //[BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoLibrary", "WolfoLibrary", "1.3.2")]
    //[NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoLib : BaseUnityPlugin
    {
   
        public void Awake()
        {
            Log.log = base.Logger;
            Commands.Awake();
            Test.Awake();

            SkinStuff.Start();
        }


        public void Start()
        {
            Commands.cheatsEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("iHarbHD.DebugToolkit");

            WConfig.Awake();
            Test.Start();
            ExtraActions.Start();

            Tags.Start();

            ShrineHalcyonFixes.Start(); //Needs to be here for Mod Support.
            VoidElite.VoidAffix();
            VoidSuppressor.SuppresedScrap();
            VoidSuppressor.FixInteractable();

            ItemCatalog.availability.CallWhenAvailable(HideItems);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/OptionPickup/OptionPickerPanel.prefab").WaitForCompletion().GetComponent<RoR2.UI.PickupPickerPanel>().maxColumnCount = 3; //Hud does not support 5 columns

        }


        public void HideItems()
        {
            RoR2Content.Items.BoostDamage.hidden = true;
            RoR2Content.Items.BoostHp.hidden = true;
            RoR2Content.Items.BoostEquipmentRecharge.hidden = true;
            RoR2Content.Items.BoostAttackSpeed.hidden = true;

        }

    }

    public static class Log
    {
        public static ManualLogSource log;

        public static void LogMessage(object message)
        {
            log.LogMessage(message);
        }
        public static void LogWarning(object message)
        {
            log.LogWarning(message);
        }
        public static void LogError(object message)
        {
            log.LogError(message);
        }
    }
}