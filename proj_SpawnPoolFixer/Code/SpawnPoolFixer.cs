using BepInEx;
using HG;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.Navigation;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace SpawnPoolFixer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Wolfo.DLCSpawnPoolFixer", "DLCSpawnPoolFixer", "1.3.4")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class SotsSpawnPoolFix : BaseUnityPlugin
    {
         
        public void Awake()
        {
            WConfig.InitConfig();
            FixSotsSpawnpools();

            //Make GolemPlains & Roost not twice as common as other stage 1s
            SceneCollection sgStage1 = Addressables.LoadAssetAsync<SceneCollection>(key: "RoR2/Base/SceneGroups/sgStage1.asset").WaitForCompletion();
            sgStage1._sceneEntries[0].weight = WConfig.cfgStage1Weight.Value;
            sgStage1._sceneEntries[1].weight = WConfig.cfgStage1Weight.Value;
            sgStage1._sceneEntries[2].weight = WConfig.cfgStage1Weight.Value;
            sgStage1._sceneEntries[3].weight = WConfig.cfgStage1Weight.Value;

            SceneCollection loopSgStage1 = Addressables.LoadAssetAsync<SceneCollection>(key: "RoR2/Base/SceneGroups/loopSgStage1.asset").WaitForCompletion();
            loopSgStage1._sceneEntries[0].weight = WConfig.cfgStage1Weight.Value;
            loopSgStage1._sceneEntries[1].weight = WConfig.cfgStage1Weight.Value;
            loopSgStage1._sceneEntries[2].weight = WConfig.cfgStage1Weight.Value;
            loopSgStage1._sceneEntries[3].weight = WConfig.cfgStage1Weight.Value;


            SceneDirector.onGenerateInteractableCardSelection += FixWrongRadarTowers;

            WolfoFixes.ExtraActions.onMonsterDCCS += RemoveMonsterBasedOnSotVReplacement;
            //IL.RoR2.ClassicStageInfo.RebuildCards += SotV_EnemyRemovals;

            On.RoR2.BazaarController.IsUnlockedBeforeLooping += NoPreLoopPostLoop;

            //Limit Altar of Gold and Radar Scanner to 1 because, logical
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "32c40c2b1da4a4244871ef499447ac1a").WaitForCompletion().maxSpawnsPerStage = 1;
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "c6c8f501bfa87e54294f9b0bb9db3da4").WaitForCompletion().maxSpawnsPerStage = 1;

            //Lunar Golem remove only being allowed to spawn in Caves
            //Bad for Dissonance, strange for Commencement, makes them unable to spawn with fixed ITMoon nodes.
            Addressables.LoadAssetAsync<CharacterSpawnCard>(key: "f7aba48d470b1594281c0eab455a12dd").WaitForCompletion().forbiddenFlags &= ~NodeFlags.NoCeiling;


            //All NodeGraphs with 0 NoCeiling Ground spots
            NodeGraph itancientloft_GroundNodeGraph = Addressables.LoadAssetAsync<NodeGraph>(key: "RoR2/DLC1/itancientloft/itancientloft_GroundNodeGraph.asset").WaitForCompletion();
            NodeGraph itmoon_GroundNodeGraph = Addressables.LoadAssetAsync<NodeGraph>(key: "RoR2/DLC1/itmoon/itmoon_GroundNodeGraph.asset").WaitForCompletion();
            NodeGraph voidraid_GroundNodeGraph = Addressables.LoadAssetAsync<NodeGraph>(key: "RoR2/DLC1/voidraid/voidraid_GroundNodeGraph.asset").WaitForCompletion();
            for (int i = 0; i < itancientloft_GroundNodeGraph.nodes.Length; i++)
            {
                itancientloft_GroundNodeGraph.nodes[i].flags |= NodeFlags.NoCeiling;
            }
            for (int i = 0; i < itmoon_GroundNodeGraph.nodes.Length; i++)
            {
                itmoon_GroundNodeGraph.nodes[i].flags |= NodeFlags.NoCeiling;
            }
            for (int i = 0; i < voidraid_GroundNodeGraph.nodes.Length; i++)
            {
                voidraid_GroundNodeGraph.nodes[i].flags |= NodeFlags.NoCeiling;
            }
  
        }

   

        private bool NoPreLoopPostLoop(On.RoR2.BazaarController.orig_IsUnlockedBeforeLooping orig, BazaarController self, SceneDef sceneDef)
        {
            if (WConfig.cfgLoopSeers.Value == true)
            {
                if (sceneDef.loopedSceneDef && Run.instance.stageClearCount >= 5)
                {
                    return false;
                }
            }
            return orig(self, sceneDef);
        }

        private void SotV_EnemyRemovals(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.ClassicStageInfo", "modifiableMonsterCategories")))
            {
                c.EmitDelegate<Func<DirectorCardCategorySelection, DirectorCardCategorySelection>>((dccs) =>
                {
                    RemoveMonsterBasedOnSotVReplacement(dccs);
                    return dccs;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: SotV_EnemyRemovals");
            }
        }

        public static void RemoveMonsterBasedOnSotVReplacement(DirectorCardCategorySelection dccs)
        {
            if (!WConfig.cfgSotV_EnemyRemovals.Value)
            {
                return;
            }
            if (Run.instance && Run.instance.IsExpansionEnabled(WolfoFixes.DLCS.DLC1))
            {
                string scene = SceneInfo.instance.sceneDef.baseSceneName;
                switch (scene)
                {
                    case "golemplains":
                        //They remove my guy jellyfish? wtf
                        RemoveCard(dccs, 2, "fish"); //-> Alpha
                        break;
                    case "frozenwall":
                        RemoveCard(dccs, 2, "Lemurian"); //-> Vermin
                        RemoveCard(dccs, 2, "Wisp"); //-> Pest
                        //Reaver
                        break;
                    case "dampcavesimple":
                        RemoveCard(dccs, 1, "GreaterWisp"); //-> Gup
                        break;
                    case "shipgraveyard":
                        RemoveCard(dccs, 2, "Beetle"); //-> Larva
                        //Reaver
                        break;
                    case "rootjungle":
                        RemoveCard(dccs, 1, "LemurianBruiser"); //-> Gup
                        RemoveCard(dccs, 2, "Lemurian"); //-> Larva
                        break;
                    case "skymeadow":
                        if (WConfig.WORMREMOVAL.Value)
                        {
                            RemoveCard(dccs, 0, "MagmaWorm"); //-> XI
                        }
                        //Reaver
                        break;
                }
            }
        }

        public static void RemoveCard(DirectorCardCategorySelection dccs, int cat, string card)
        {
            //Because categories get shifted around in DCCS Blender, better to just find them
            string a = "";
            switch (cat)
            {
                case 2:
                    a = "Basic Monsters";
                    break;
                case 1:
                    a = "Minibosses";
                    break;
                case 0:
                    a = "Champions";
                    break;

            }
            cat = dccs.FindCategoryIndexByName(a);
            if (cat != -1)
            {
                int c = FindSpawnCard(dccs.categories[cat].cards, card);
                if (c != -1)
                {
                    Debug.Log("SotV Removals : Removed " + card + " from " + SceneInfo.instance.sceneDef);
                    ArrayUtils.ArrayRemoveAtAndResize(ref dccs.categories[cat].cards, c);
                    return;
                }
            }
            Debug.LogWarning("Failed to remove " + card + " from " + SceneInfo.instance.sceneDef);

        }

        public static int FindSpawnCard(DirectorCard[] insert, string LookingFor)
        {
            for (int i = 0; i < insert.Length; i++)
            {
                if (insert[i].spawnCard.name.EndsWith(LookingFor))
                {
                    //Debug.Log("Found " + LookingFor);
                    return i;
                }
            }
            Debug.LogWarning("Couldn't find " + LookingFor);
            return -1;
        }

        private void FixWrongRadarTowers(SceneDirector scene, DirectorCardCategorySelection dccs)
        {
            int rare = dccs.FindCategoryIndexByName("Rare");
            if (rare != -1)
            {
                var a = dccs.categories[rare];
                for (int i = 0; i < a.cards.Length; i++)
                {
                    if (a.cards[i].forbiddenUnlockableDef)
                    {
                        string name = SceneInfo.instance.sceneDef.cachedName;
                        if (name.StartsWith("habitat"))
                        {
                            //This could just be done on every stage, so issues like this NEVER happen again.
                            UnlockableDef unlockableDef = UnlockableCatalog.GetUnlockableDef("Logs.Stages." + name);
                            Debug.Log("Automatic Radar Scanner fix for " + unlockableDef);
                            a.cards[i].forbiddenUnlockableDef = unlockableDef;
                        }
                        //Make Scanners more common because grinding for them is stupid
                        a.cards[i].selectionWeight = 10000;
                    }
                }
            }
 
        }

 
        public static void FixSotsSpawnpools()
        {
            DccsPool dpArtifactWorld02Monsters = Addressables.LoadAssetAsync<DccsPool>(key: "RoR2/DLC2/artifactworld02/dpArtifactWorld02Monsters.asset").WaitForCompletion();
            dpArtifactWorld02Monsters.poolCategories[0].includedIfConditionsMet[0].dccs = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "RoR2/DLC2/artifactworld02/dccsArtifactWorld02Monsters_DLC1.asset").WaitForCompletion(); ;

            DirectorCardCategorySelection dccsVillageInteractables_DLC2 = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "RoR2/DLC2/village/dccsVillageInteractables_DLC2.asset").WaitForCompletion();
            dccsVillageInteractables_DLC2.categories[5].cards[3].minimumStageCompletions = 1;
        }
    }
}