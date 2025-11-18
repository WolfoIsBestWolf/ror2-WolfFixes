using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoLibrary
{

    internal static class VoidElite
    {
        public static void VoidAffix()
        {
            //Add Display frop dropped Void Affix
            //Used by couple mods
            //Harmless for Vanilla
            EquipmentDef VoidAffix = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteVoid/EliteVoidEquipment.asset").WaitForCompletion();
            GameObject VoidAffixDisplay = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/EliteVoid/DisplayAffixVoid.prefab").WaitForCompletion(), "PickupAffixVoidW", false);

            //Eye
            //Flesh
            //Metal

            VoidAffixDisplay.transform.GetChild(0).GetChild(2).SetAsFirstSibling();
            VoidAffixDisplay.transform.GetChild(1).localPosition = new Vector3(0f, 0.7f, 0f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, -0.5f, -0.6f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(0).localScale = new Vector3(1.5f, 1.5f, 1.5f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            VoidAffixDisplay.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            VoidAffixDisplay.transform.GetChild(0).eulerAngles = new Vector3(310, 0, 0);
            VoidAffixDisplay.transform.GetChild(0).localScale = new Vector3(0.75f, 0.75f, 0.75f);

            ItemDisplay display = VoidAffixDisplay.GetComponent<ItemDisplay>();
            HG.ArrayUtils.ArrayRemoveAtAndResize(ref display.rendererInfos, 4);
            VoidAffixDisplay.AddComponent<ModelPanelParameters>();

            VoidAffix.pickupModelReference = new AssetReferenceT<GameObject>("");
            VoidAffix.pickupModelPrefab = VoidAffixDisplay;
        }

    }

}
