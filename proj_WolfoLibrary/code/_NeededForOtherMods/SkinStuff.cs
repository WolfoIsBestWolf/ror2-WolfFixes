using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoLibrary
{
    public class SkinStuff
    {
        public static void Start()
        {
            SkinDefParams skinDroneTechDef_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "6517af04dc3e7ce45a55f09e04c96772").WaitForCompletion();
            SkinDefParams skinDroneTechDefAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "4c92b69e995926b46876ed3c644a2e81").WaitForCompletion();
            skinDroneTechDef_params.minionSkinReplacements = new SkinDefParams.MinionSkinReplacement[]
            {
                new SkinDefParams.MinionSkinReplacement
                {
                    minionBodyPrefab = Addressables.LoadAssetAsync<GameObject>(key: "2340146f44941cc45aa8edc354ad77ae").WaitForCompletion(),
                    minionSkin = Addressables.LoadAssetAsync<SkinDef>(key: "510231c58a9944e45be8b90f5b41a4d0").WaitForCompletion(),
                },
                new SkinDefParams.MinionSkinReplacement
                {
                    minionBodyPrefab = Addressables.LoadAssetAsync<GameObject>(key: "521223397dd4da74694c073e63c3183b").WaitForCompletion(),
                    minionSkin = Addressables.LoadAssetAsync<SkinDef>(key: "37af658961eb1d64b91d03d8098da0f2").WaitForCompletion(),
                },
                new SkinDefParams.MinionSkinReplacement
                {
                    minionBodyPrefab = Addressables.LoadAssetAsync<GameObject>(key: "442aea01d301dec41a4a7c28e112d737").WaitForCompletion(),
                    minionSkin = Addressables.LoadAssetAsync<SkinDef>(key: "a725c698056caf04790c1f808f62d948").WaitForCompletion(),
                },
            };
            skinDroneTechDefAlt_params.minionSkinReplacements = HG.ArrayUtils.Clone(skinDroneTechDef_params.minionSkinReplacements);
            Addressables.LoadAssetAsync<GameObject>(key: "ff7711aebac29b148a7312a3cae20572").WaitForCompletion().transform.GetChild(0).GetChild(0).gameObject.AddComponent<OperatorGunColorReplacer>();
            Addressables.LoadAssetAsync<GameObject>(key: "de1037cdfdf9bdc4f833708daffdf33d").WaitForCompletion().AddComponent<OperatorLobbySyncDroneSkinToMainSkin>();
        }
    }

    public class OperatorLobbySyncDroneSkinToMainSkin : MonoBehaviour
    {
        private Transform droneSlotL;
        private Transform droneSlotR;
        private ModelSkinController controller;

        private Material mat1 = Addressables.LoadAssetAsync<Material>(key: "a5e2282a0ad1c474696de2fe1ddd09d4").WaitForCompletion(); //matDrone1_B
        private Material mat2 = Addressables.LoadAssetAsync<Material>(key: "4172ed2e16de6f04b8be2f6377521abb").WaitForCompletion(); //matDrone2_B
        private Material mat3 = Addressables.LoadAssetAsync<Material>(key: "448eba93c01ca6046970593a3cc2d234").WaitForCompletion();  //matHaulerJailerRechargeCleanup 1.mat

        public void OnEnable()
        {
            controller = GetComponentInChildren<ModelSkinController>();
            controller.onSkinApplied += Controller_onSkinApplied;

            Transform drone_center_M = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1);
            droneSlotL = drone_center_M.GetChild(0).GetChild(0);
            droneSlotR = drone_center_M.GetChild(1).GetChild(0);
        }
        public void OnDisable()
        {
            controller.onSkinApplied -= Controller_onSkinApplied;
        }
        private void Controller_onSkinApplied(int index)
        {
            if (controller.skins[index]._runtimeSkin == null)
            {
                Debug.LogWarning("No runtime skin");
                return;
            }


            Material matToSet1 = mat1;
            Material matToSet2 = mat2;
            Material matToSet3 = mat3;

            if (controller.skins[index].runtimeSkin.minionSkinTemplates.Length != 0)
            {
                //How do we *unset* this if we there are no replacements.
                BodyIndex DTGunnerDroneBody = BodyCatalog.FindBodyIndex("DTGunnerDroneBody");
                BodyIndex DTHealingDroneBody = BodyCatalog.FindBodyIndex("DTHealingDroneBody");
                BodyIndex DTHaulerDroneBody = BodyCatalog.FindBodyIndex("DTHaulerDroneBody");

                foreach (var minionReplacements in controller.skins[index].runtimeSkin.minionSkinTemplates)
                {
                    if (minionReplacements.minionSkin._runtimeSkin == null)
                    {
                        minionReplacements.minionSkin.Bake();
                    }
                    Material matToSet = minionReplacements.minionSkin.runtimeSkin.rendererInfoTemplates[0].data.defaultMaterial;
                    if (matToSet != null)
                    {
                        if (minionReplacements.minionBodyIndex == DTGunnerDroneBody)
                        {
                            matToSet1 = matToSet;
                        }
                        else if (minionReplacements.minionBodyIndex == DTHealingDroneBody)
                        {
                            matToSet2 = matToSet;
                        }
                        else if (minionReplacements.minionBodyIndex == DTHaulerDroneBody)
                        {
                            matToSet3 = matToSet;
                        }
                    }
                }
            }

            droneSlotL.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = matToSet1;
            droneSlotR.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = matToSet1;
            droneSlotL.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = matToSet2;
            droneSlotR.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = matToSet2;
            droneSlotL.GetChild(3).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = matToSet3;
            droneSlotR.GetChild(3).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = matToSet3;

        }
    }

    public class OperatorGunColorReplacer : MonoBehaviour
    {
        //0.5, 0.1, 0.2
        public static Dictionary<SkinDef, Color> skinDefDic = new Dictionary<SkinDef, Color>();
        public ModelSkinController skinController;
        public void Awake()
        {
            skinController = GetComponent<ModelSkinController>();
            skinController.onSkinApplied += Apply;
        }
        public void OnDisable()
        {
            skinController.onSkinApplied -= Apply;
        }

        public void Apply(int index)
        {
            if (skinDefDic.TryGetValue(skinController.skins[index], out var color))
            {
                ChildLocator childLocator = GetComponent<ChildLocator>();
                Transform ChargeVFX = childLocator.FindChild("ChargeVFX");
                Transform NanoPistolChargeEffect = ChargeVFX.GetChild(0);

                NanoPistolChargeEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = color.AlphaMultiplied(0.2667f); //0 0.2863 0.2902 0.2667
                NanoPistolChargeEffect.GetChild(1).GetComponent<ParticleSystem>().startColor = color; //0 0.2863 0.2902 1

                ParticleSystemRenderer pixels = NanoPistolChargeEffect.GetChild(2).GetComponent<ParticleSystemRenderer>();
                pixels.material = Instantiate(pixels.material);
                pixels.material.SetColor("_TintColor", color); //0 0.2863 0.2902 1

                TrailRenderer trail = NanoPistolChargeEffect.GetChild(2).GetComponent<TrailRenderer>();
                trail.material = Instantiate(trail.material);
                trail.material.SetColor("_TintColor", color); //0 0.2863 0.2902 1

            }
        }

    }

}
