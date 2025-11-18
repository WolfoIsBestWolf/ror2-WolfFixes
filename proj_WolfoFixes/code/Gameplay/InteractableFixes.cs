using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{
    internal class InteractableFixes
    {
        public static void Start()
        {
            ShrineShapingFixes.Start();
            ShrineHalcyonFixes.Start();


            //Avoid duplicate Golemplains & BlackBeach
            //Avoid Special Stages removing a choice for 3rd Seers.
            On.RoR2.BazaarController.SetUpSeerStations += FixDuplicateAndEmptyThirdSeers;
            //For any mods adding extra seer stations.
            //Doubt it'd come up in vanilla.
            On.RoR2.SeerStationController.SetRunNextStageToTarget += BazaarDisableAllSeers;

            //Disable Broken Missile & FlameDrone rotation, so it no longer fucking clips in the ground all the god damn time.
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "749e2ff7e1839074885efb0f82197ba7").WaitForCompletion().slightlyRandomizeOrientation = false;
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "592ddd0e913440844b42eff65663abda").WaitForCompletion().slightlyRandomizeOrientation = false;

            On.EntityStates.DroneCombiner.DroneCombinerCombining.OnDeserialize += FixBrokenAH_DroneCombinerCombining_OnDeserialize;
        }

        private static void FixBrokenAH_DroneCombinerCombining_OnDeserialize(On.EntityStates.DroneCombiner.DroneCombinerCombining.orig_OnDeserialize orig, EntityStates.DroneCombiner.DroneCombinerCombining self, NetworkReader reader)
        {
            //NetworkServer.FindLocalObject, on Client
            //This always results in Null
            //Idk how to fix it better
            NetworkInstanceId netId = reader.ReadNetworkId();
            NetworkInstanceId netId2 = reader.ReadNetworkId();
            GameObject gameObject = ClientScene.FindLocalObject(netId);
            GameObject gameObject2 = ClientScene.FindLocalObject(netId2);
            if (gameObject)
            {
                self.toUpgrade = gameObject.GetComponent<CharacterBody>();
            }
            if (gameObject2)
            {
                self.toDestroy = gameObject2.GetComponent<CharacterBody>();
            }

            self._controller = self.gameObject.GetComponent<DroneCombinerController>();
        }

        private static void BazaarDisableAllSeers(On.RoR2.SeerStationController.orig_SetRunNextStageToTarget orig, SeerStationController self)
        {
            orig(self);
            if (BazaarController.instance)
            {
                for (int i = 0; i < BazaarController.instance.seerStations.Length; i++)
                {
                    BazaarController.instance.seerStations[i].GetComponent<PurchaseInteraction>().SetAvailable(false);
                }
            }
        }
        private static void FixDuplicateAndEmptyThirdSeers(On.RoR2.BazaarController.orig_SetUpSeerStations orig, BazaarController self)
        {
            orig(self);

            List<string> takenBaseScenes = new List<string>();
            List<SeerStationController> seersToFix = new List<SeerStationController>();
            foreach (SeerStationController seer in self.seerStations)
            {
                if (seer.targetSceneDefIndex != -1)
                {
                    string a = SceneCatalog.indexToSceneDef[seer.targetSceneDefIndex].baseSceneName;
                    //WolfoMain.Logger.LogMessage(a);
                    if (takenBaseScenes.Contains(a))
                    {
                        seersToFix.Add(seer);
                    }
                    else
                    {
                        takenBaseScenes.Add(a);
                    }
                }
                else
                {
                    seersToFix.Add(seer);
                }
            }
            if (seersToFix.Count > 0)
            {
                SceneDef nextStageScene = Run.instance.nextStageScene;
                List<SceneDef> list = new List<SceneDef>();
                if (nextStageScene != null)
                {
                    int stageOrder = nextStageScene.stageOrder;
                    foreach (SceneDef sceneDef in SceneCatalog.allSceneDefs)
                    {
                        if (sceneDef.stageOrder == stageOrder && (sceneDef.requiredExpansion == null || Run.instance.IsExpansionEnabled(sceneDef.requiredExpansion)) && self.IsUnlockedBeforeLooping(sceneDef))
                        {
                            if (!takenBaseScenes.Contains(sceneDef.baseSceneName))
                            {
                                // WolfoMain.Logger.LogMessage(sceneDef);
                                list.Add(sceneDef);
                            }
                        }
                    }
                }
                if (list.Count > 0)
                {
                    foreach (var seer in seersToFix)
                    {
                        Util.ShuffleList<SceneDef>(list, self.rng);
                        int index = list.Count - 1;
                        seer.SetTargetScene(list[index]);
                        list.RemoveAt(index);
                        seer.GetComponent<PurchaseInteraction>().SetAvailable(true);
                    }
                }
            }

        }

    }


}
