using RoR2;
using RoR2.EntitlementManagement;
using RoR2.ExpansionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoLibrary
{
    public static class DLCS
    {
        public static ExpansionDef DLC1 = Addressables.LoadAssetAsync<ExpansionDef>(key: "d4f30c23b971a9b428e2796dc04ae099").WaitForCompletion();
        public static ExpansionDef DLC2 = Addressables.LoadAssetAsync<ExpansionDef>(key: "851f234056d389b42822523d1be6a167").WaitForCompletion();
        public static ExpansionDef DLC3 = Addressables.LoadAssetAsync<ExpansionDef>(key: "RoR2/DLC3/DLC3.asset").WaitForCompletion();

        public static EntitlementDef entitlementDLC1 = Addressables.LoadAssetAsync<EntitlementDef>(key: "0166774839e0bd345bb11554aecbfd32").WaitForCompletion();
        public static EntitlementDef entitlementDLC2 = Addressables.LoadAssetAsync<EntitlementDef>(key: "68fcfaddae5157f4581a1fc209e4eac6").WaitForCompletion();
        public static EntitlementDef entitlementDLC3 = Addressables.LoadAssetAsync<EntitlementDef>(key: "6673dacf8c8aa87409fc73e6da34684b").WaitForCompletion();

    }
    public static class SceneList
    {
        public static SceneDef Blackbeach = Addressables.LoadAssetAsync<SceneDef>(key: "c129d2b9e62c31b4ba165091d1ae2c50").WaitForCompletion();
        public static SceneDef Blackbeach2 = Addressables.LoadAssetAsync<SceneDef>(key: "f87198140bf9b5a4f82e206231df9091").WaitForCompletion();
        public static SceneDef Golemplains = Addressables.LoadAssetAsync<SceneDef>(key: "8ba24a95e46b3944280a4b66afd0c4dc").WaitForCompletion();
        public static SceneDef Golemplains2 = Addressables.LoadAssetAsync<SceneDef>(key: "657a62eb3e4c409429c91ba6fdb7d921").WaitForCompletion();

        public static SceneDef SulfurPools = Addressables.LoadAssetAsync<SceneDef>(key: "796f9b67682b3db4c8c5af7294d0490c").WaitForCompletion();
        public static SceneDef RootJungle = Addressables.LoadAssetAsync<SceneDef>(key: "ced489f798226594db0d115af2101a9b").WaitForCompletion();
        public static SceneDef Habitat = Addressables.LoadAssetAsync<SceneDef>(key: "0b33829c6cdfad34a8160c6ae17edfcc").WaitForCompletion();
        public static SceneDef HabitatFall = Addressables.LoadAssetAsync<SceneDef>(key: "962a6a8a56f584b468a4835c82c11951").WaitForCompletion();

        public static SceneDef itGolemPlains = Addressables.LoadAssetAsync<SceneDef>(key: "dda7dbdf013275747949dfc522842635").WaitForCompletion();
        public static SceneDef itGoolake = Addressables.LoadAssetAsync<SceneDef>(key: "0a35bec906d067941a8a378b963e9e9d").WaitForCompletion();
        public static SceneDef itAncientLoft = Addressables.LoadAssetAsync<SceneDef>(key: "568295b69af09c241968fa18ca37b56d").WaitForCompletion();
        public static SceneDef itFrozenwall = Addressables.LoadAssetAsync<SceneDef>(key: "0ab15976715ddd6438e2e5bd6ddbe224").WaitForCompletion();
        public static SceneDef itDampCave = Addressables.LoadAssetAsync<SceneDef>(key: "c33b24f6ebf27db49a5cfb88dbe9b8ff").WaitForCompletion();
        public static SceneDef itSkyMeadow = Addressables.LoadAssetAsync<SceneDef>(key: "6a2712b5c8cf36f44b34c128f2759522").WaitForCompletion();
        public static SceneDef itMoon = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/itmoon/itmoon.asset").WaitForCompletion();

        public static SceneDef Moon2 = Addressables.LoadAssetAsync<SceneDef>(key: "5d69f3396feb7ba428d1e53a44479594").WaitForCompletion();
        public static SceneDef MysterySpace = Addressables.LoadAssetAsync<SceneDef>(key: "1303cd0450cbaf140970e7afd5162314").WaitForCompletion();
        public static SceneDef VoidStage = Addressables.LoadAssetAsync<SceneDef>(key: "66e0cfba315981a40afae481363ea0da").WaitForCompletion();
        public static SceneDef VoidRaid = Addressables.LoadAssetAsync<SceneDef>(key: "223a0f0a86052654a9e473d13f77cb41").WaitForCompletion();


    }


    public static class MissedContent
    {
        public static class Items
        {
            public static ItemDef ScrapWhiteSuppressed = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/ScrapVoid/ScrapWhiteSuppressed.asset").WaitForCompletion();
            public static ItemDef ScrapGreenSuppressed = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/ScrapVoid/ScrapGreenSuppressed.asset").WaitForCompletion();
            public static ItemDef ScrapRedSuppressed = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/ScrapVoid/ScrapRedSuppressed.asset").WaitForCompletion();
        }
        public static class Equipment
        {
            public static EquipmentDef EliteEarthEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "463f8f6917dd8af40860b023a520f051").WaitForCompletion();
            public static EquipmentDef EliteSecretSpeedEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteSecretSpeedEquipment.asset").WaitForCompletion();
        }

        public static class BodyPrefabs
        {
            public static CharacterBody SquidTurretBody = Addressables.LoadAssetAsync<GameObject>(key: "5290eaf612d386740ac26f289e06b62f").WaitForCompletion().GetComponent<CharacterBody>();
            public static CharacterBody BeetleGuardAllyBody = Addressables.LoadAssetAsync<GameObject>(key: "e21e3b9cdc2802148986eda1c923c9a1").WaitForCompletion().GetComponent<CharacterBody>();
            public static CharacterBody RoboBallGreenBuddyBody = Addressables.LoadAssetAsync<GameObject>(key: "d64335d9c1517cb4a8764a0de1d01a03").WaitForCompletion().GetComponent<CharacterBody>();
            public static CharacterBody RoboBallRedBuddyBody = Addressables.LoadAssetAsync<GameObject>(key: "e8a603acea1fa2249a83c0257a9f5f10").WaitForCompletion().GetComponent<CharacterBody>();
        }

        public static class GameEndings
        {
            public static GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();
        }
        public static class Survivors
        {
            public static SurvivorDef VoidSurvivor = Addressables.LoadAssetAsync<SurvivorDef>(key: "dc32bd426643dce478dd1b04fd07cdf6").WaitForCompletion();
        }
        public static class Elites
        {
            public static EliteDef edSecretSpeed = Addressables.LoadAssetAsync<EliteDef>(key: "9752d818bdea9b449845fc4df8aed07a").WaitForCompletion();
            public static EliteDef CollectiveWeak = Addressables.LoadAssetAsync<EliteDef>(key: "5834fbe05a0d4ce4eae57f87aae57920").WaitForCompletion();
        }
    }

}
