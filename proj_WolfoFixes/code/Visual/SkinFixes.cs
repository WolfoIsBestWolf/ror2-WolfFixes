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
            SkinDefParams skinBandit2Alt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Bandit2/skinBandit2Alt_params.asset").WaitForCompletion();
            //Collusus as unique model so turn on
            //ACalready turns on
            //HC already turns off
            GameObject Bandit2AccessoriesMesh = skinBandit2Default_params.gameObjectActivations[0].gameObject.transform.parent.GetChild(0).gameObject;
            HG.ArrayUtils.ArrayAppend(ref skinBandit2Default_params.gameObjectActivations, new SkinDefParams.GameObjectActivation
            {
                shouldActivate = true,
                gameObject = Bandit2AccessoriesMesh,
            });
            HG.ArrayUtils.ArrayAppend(ref skinBandit2Alt_params.gameObjectActivations, new SkinDefParams.GameObjectActivation
            {
                shouldActivate = false,
                gameObject = Bandit2AccessoriesMesh,
            });
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