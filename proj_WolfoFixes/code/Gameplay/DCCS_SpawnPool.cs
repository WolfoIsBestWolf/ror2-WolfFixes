//using System;
using RoR2;
using RoR2.Navigation;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{

    internal class DCCS_SpawnPool
    {
        public static void Main()
        {
            DCCSFixes();
            SpawnCardFixes();


            //Make GolemPlains & Roost not twice as common as other stage 1s
            SceneCollection sgStage1 = Addressables.LoadAssetAsync<SceneCollection>(key: "RoR2/Base/SceneGroups/sgStage1.asset").WaitForCompletion();
            sgStage1._sceneEntries[0].weight = WConfig.cfgStage1Weight.Value;
            sgStage1._sceneEntries[1].weight = WConfig.cfgStage1Weight.Value;
            sgStage1._sceneEntries[2].weight = WConfig.cfgStage1Weight.Value;
            sgStage1._sceneEntries[3].weight = WConfig.cfgStage1Weight.Value;

            //For no fucking reason at all
            //They made VerdantFalls twice as rare as a stage 1.
            sgStage1._sceneEntries[5].weight = 1; //Lakes

            SceneCollection loopSgStage1 = Addressables.LoadAssetAsync<SceneCollection>(key: "RoR2/Base/SceneGroups/loopSgStage1.asset").WaitForCompletion();
            loopSgStage1._sceneEntries[0].weight = WConfig.cfgStage1Weight.Value;
            loopSgStage1._sceneEntries[1].weight = WConfig.cfgStage1Weight.Value;
            loopSgStage1._sceneEntries[2].weight = WConfig.cfgStage1Weight.Value;
            loopSgStage1._sceneEntries[3].weight = WConfig.cfgStage1Weight.Value;


            On.RoR2.BazaarController.IsUnlockedBeforeLooping += NoPreLoopPostLoop;
            On.RoR2.Stage.PreStartClient += FixRareStagesWithoutNodesWhenCalledFor;
            On.RoR2.AccessCodesMissionController.OnStartServer += AccessCodesMissionController_OnStartServer;
        }

        private static void AccessCodesMissionController_OnStartServer(On.RoR2.AccessCodesMissionController.orig_OnStartServer orig, AccessCodesMissionController self)
        {
            //Intended to be this way they just forgor
            if (SceneInfo.instance.sceneDef.baseSceneName == "repurposedcrater")
            {
                self.ignoreSolusWingDeath = true;
            }
            orig(self);
        }

        private static void FixRareStagesWithoutNodesWhenCalledFor(On.RoR2.Stage.orig_PreStartClient orig, Stage self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                string scene = SceneInfo.instance.sceneDef.baseSceneName;
                switch (scene)
                {
                    case "itancientloft":
                        //FIX NO NOCEILING NODES
                        NodeGraph itancientloft_GroundNodeGraph = Addressables.LoadAssetAsync<NodeGraph>(key: "RoR2/DLC1/itancientloft/itancientloft_GroundNodeGraph.asset").WaitForCompletion();
                        for (int i = 0; i < itancientloft_GroundNodeGraph.nodes.Length; i += 10)
                        {
                            itancientloft_GroundNodeGraph.nodes[i].flags |= NodeFlags.NoCeiling;
                        }
                        break;
                    case "itmoon":
                        //FIX NO NOCEILING NODES
                        NodeGraph itmoon_GroundNodeGraph = Addressables.LoadAssetAsync<NodeGraph>(key: "RoR2/DLC1/itmoon/itmoon_GroundNodeGraph.asset").WaitForCompletion();
                        for (int i = 0; i < itmoon_GroundNodeGraph.nodes.Length; i += 10)
                        {
                            itmoon_GroundNodeGraph.nodes[i].flags |= NodeFlags.NoCeiling;
                        }
                        break;
                }
            }
        }




        //You can get preLoop variants in LunarSeers, after Looping
        //Unlike normal stage rotation
        //So it looks like a overseight at the very least.
        //Loop Variants are rare enough as is so it's just kinda annoying.
        public static bool NoPreLoopPostLoop(On.RoR2.BazaarController.orig_IsUnlockedBeforeLooping orig, BazaarController self, SceneDef sceneDef)
        {
            if (WConfig.cfgLoopSeers.Value)
            {
                if (sceneDef.loopedSceneDef && Run.instance.stageClearCount >= 5)
                {
                    return false;
                }
            }
            return orig(self, sceneDef);
        }



        public static void DCCSFixes()
        {
            //Wrong Spawn Pool used in DPPool
            DccsPool dpArtifactWorld02Monsters = Addressables.LoadAssetAsync<DccsPool>(key: "RoR2/DLC2/artifactworld02/dpArtifactWorld02Monsters.asset").WaitForCompletion();
            dpArtifactWorld02Monsters.poolCategories[0].includedIfConditionsMet[0].dccs = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "RoR2/DLC2/artifactworld02/dccsArtifactWorld02Monsters_DLC1.asset").WaitForCompletion(); ;

            //Yellow Printers should NOT spawn on stage 1.
            // DirectorCardCategorySelection dccsVillageInteractables_DLC2 = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "RoR2/DLC2/village/dccsVillageInteractables_DLC2.asset").WaitForCompletion();
            //dccsVillageInteractables_DLC2.categories[5].cards[3].minimumStageCompletions = 1;

            //This still contains MajorConstructs
            //This still contains wrong Alpha constructs
            DirectorCardCategorySelection dccsArtifactWorld03MonstersDLC1 = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "eca601c0708cfe940b371b5cae6a4872").WaitForCompletion();
            dccsArtifactWorld03MonstersDLC1.categories[0].cards[0].spawnCard = null; //Idk why it sometimes has or doesnt have this set
            dccsArtifactWorld03MonstersDLC1.categories[1].cards[0].spawnCard = null; //
            dccsArtifactWorld03MonstersDLC1.categories[0].cards[0].spawnCardReference = new AssetReferenceT<SpawnCard>("cf55a1f0cb720ec4eb136a5976013bd0");
            dccsArtifactWorld03MonstersDLC1.categories[1].cards[0].spawnCardReference = new AssetReferenceT<SpawnCard>("0ca64dc7fa66f4e4a88352d08bf15e66");


            //Parent Family missing Grandparent & Child
            DirectorCardCategorySelection dccsParentFamily = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "359a1dff413474745a1d4076c1b26aa7").WaitForCompletion();
            dccsParentFamily.AddCard(0, new DirectorCard
            {
                spawnCardReference = new AssetReferenceT<SpawnCard>("1d23f78593a91a04f818ca9895f4e7d7"),
                selectionWeight = 1,
            });
            dccsParentFamily.AddCard(0, new DirectorCard
            {
                spawnCardReference = new AssetReferenceT<SpawnCard>("db1a26c64dfaf5043949b2aedf193269"),
                selectionWeight = 1,
            });


            //Gup Family missing Geep & Gip
            //Maybe a bit of overstep
            /*DirectorCardCategorySelection dccsGupFamily = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "f3a69355569999b4ebd363e0481e7825").WaitForCompletion();
            dccsGupFamily.AddCard(0, new DirectorCard
            {
                spawnCardReference = new AssetReferenceT<SpawnCard>("9ddfbf5c539df144c8be21008f19be25"),
                selectionWeight = 1,
            });
            dccsGupFamily.AddCard(0, new DirectorCard
            {
                spawnCardReference = new AssetReferenceT<SpawnCard>("564f15ee552b4314197f0b1e5a96ab82"),
                selectionWeight = 1,
            });*/



         

            //Hermit Crab missing from Dissonance
            /*DirectorCardCategorySelection dccsMixEnemy = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: "0453bb766b3ba0448943dd0b703996cb").WaitForCompletion();
            if (FindSpawnCard(ref dccsMixEnemy.categories[2].cards, "Crab") == -1)
            {
                dccsMixEnemy.AddCard(2, new DirectorCard
                {
                    spawnCardReference = new AssetReferenceT<SpawnCard>("844c7cf8f1b6c8b4aa77e082386174b9"),
                    selectionWeight = 1,
                    spawnDistance = DirectorCore.MonsterSpawnDistance.Far
                });
            }*/
        }

        public static void SpawnCardFixes()
        {
            //Lunar Golem remove only being allowed to spawn in Caves
            //Bad for Dissonance, strange for Commencement, makes them unable to spawn with fixed ITMoon nodes.
            Addressables.LoadAssetAsync<CharacterSpawnCard>(key: "f7aba48d470b1594281c0eab455a12dd").WaitForCompletion().forbiddenFlags &= ~NodeFlags.NoCeiling;


            //Limit Altar of Gold
            //Radar Tower was limited to 1 in AC
            //More newer interactables use this too just because logic.
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "32c40c2b1da4a4244871ef499447ac1a").WaitForCompletion().maxSpawnsPerStage = 1;

            //Drone Combiner isn't tagged as "NoDevotion"
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "2eaec01927ea16245822dcb50080cba3").WaitForCompletion().skipSpawnWhenDevotionArtifactEnabled = true;
            //Drone Shop is tagged as "NoSacrifice" probably on accident
            Addressables.LoadAssetAsync<InteractableSpawnCard>(key: "5a86990b032424e48b4b8456f7d684c9").WaitForCompletion().skipSpawnWhenSacrificeArtifactEnabled = false;

        }


    }


}
