using RoR2;
using UnityEngine;
using RoR2.ContentManagement;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class SkinFixes
    {
        public static void Start()
        {
            On.RoR2.CharacterModel.IsAurelioniteAffix += DontRemoveGildedVisualsOnDeath;

            SkinDefParams skinBandit2Default_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Bandit2/skinBandit2Default_params.asset").WaitForCompletion();
   
            GameObject Bandit2AccessoriesMesh = skinBandit2Default_params.gameObjectActivations[0].gameObject.transform.parent.GetChild(0).gameObject;
            HG.ArrayUtils.ArrayAppend(ref skinBandit2Default_params.gameObjectActivations, new SkinDefParams.GameObjectActivation
            {
                shouldActivate = true,
                gameObject = Bandit2AccessoriesMesh,
            });


            SkinDefParams skinEngiAltHallow_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Engi/skinEngiAltHallow_params.asset").WaitForCompletion();
            skinEngiAltHallow_params.projectileGhostReplacements[0].ghostReplacementAddress = new AssetReferenceT<GameObject>("4265a82c362606947bd759e23ee1deea");
        }

        private static bool DontRemoveGildedVisualsOnDeath(On.RoR2.CharacterModel.orig_IsAurelioniteAffix orig, CharacterModel self)
        {
            if (self.myEliteIndex == DLC2Content.Elites.Aurelionite.eliteIndex)
            {
                return true;
            }
            return orig(self);
        }

    }

}