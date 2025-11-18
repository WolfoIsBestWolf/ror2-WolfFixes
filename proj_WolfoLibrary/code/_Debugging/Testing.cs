using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoLibrary.Testing
{
    internal class Test
    {
        public static void Awake()
        {
            //UnusedStages();

        }
        public static void Start()
        {
            if (WConfig.cfgTestLogbook.Value)
            {

                Test_Logbook.CheatLogbook();
            }
            if (WConfig.cfgTestMultiplayer.Value)
            {
                On.RoR2.PickupPickerController.GetOptionsFromPickupState += CommandWithEveryItem;
                IL.RoR2.Networking.ServerAuthManager.HandleSetClientAuth += ServerAuthManager_HandleSetClientAuth;
                On.RoR2.Run.GetUserMaster += Run_GetUserMaster;
                On.RoR2.SteamworksLobbyManager.OnLobbyJoined_bool_string += SteamworksLobbyManager_OnLobbyJoined_bool_string;
            }
            if (WConfig.cfgLoadOrder.Value)
            {
                TestLoadOrder.Start();
            }

        }















        private static void SteamworksLobbyManager_OnLobbyJoined_bool_string(On.RoR2.SteamworksLobbyManager.orig_OnLobbyJoined_bool_string orig, SteamworksLobbyManager self, bool success, string tokenReasonFailed)
        {
            //Random error loop where it'd open 500 server full windows.
            if (string.IsNullOrEmpty(tokenReasonFailed))
            {
                success = true;
            }
            orig(self, success, tokenReasonFailed);
        }

        public static CharacterMaster Run_GetUserMaster(On.RoR2.Run.orig_GetUserMaster orig, Run self, NetworkUserId networkUserId)
        {
            if (WConfig.cfgTestMultiplayer.Value)
            {
                return null;
            }
            return orig(self, networkUserId);
        }

        public static void ServerAuthManager_HandleSetClientAuth(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdloc(3)
                ))
            {
                c.EmitDelegate<System.Func<NetworkConnection, NetworkConnection>>((self) =>
                {
                    if (WConfig.cfgTestMultiplayer.Value)
                    {
                        return null;
                    }
                    return self;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: ServerAuthManager_HandleSetClientAuth");
            }
        }

        /*public static void UnusedStages()
        {
            SceneDef newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("31b8f728a914c9a4faa3df76d1bc0c0e");
            newScenedDef.cachedName = "golemplains_trailer";
            newScenedDef.nameToken = "Trailer Plains";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("b30e537b28c514b4a99227ab56a3e1d3");
            newScenedDef.cachedName = "ItemLogBookPositionalOffsets";
            newScenedDef.nameToken = "ItemLogBookPositionalOffsets";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("722873b571c73734c8572658dbb8f0db");
            newScenedDef.cachedName = "renderitem";
            newScenedDef.nameToken = "renderitem";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("6b7bd17baff7d094eb2f7624b519f191");
            newScenedDef.cachedName = "slice1";
            newScenedDef.nameToken = "slice1";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("835344f0a7461cc4b8909469b31a3ccc");
            newScenedDef.cachedName = "slice2";
            newScenedDef.nameToken = "slice2";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("5d8ea33392b43b94daac86dcf06740ab");
            newScenedDef.cachedName = "space";
            newScenedDef.nameToken = "space";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("db3348f5ee64faa48b2c14c3c52d5186");
            newScenedDef.cachedName = "stage1";
            newScenedDef.nameToken = "stage1";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("3b8a85a1a46c2454e88c02f400c0b5f5");
            newScenedDef.cachedName = "scnNetTest";
            newScenedDef.nameToken = "scnNetTest";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("b1bdafceaf6eb7e449a9bdd6e8bd0aa7");
            newScenedDef.cachedName = "scnNetTest2";
            newScenedDef.nameToken = "scnNetTest2";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("2f98cc89f5cba394e96b147624d9af73");
            newScenedDef.cachedName = "dampcave";
            newScenedDef.nameToken = "dampcave";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("78d26bf718780e5408f546539011a328");
            newScenedDef.cachedName = "gauntlet2";
            newScenedDef.nameToken = "gauntlet2";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("a409067e2f2500e44a49be1ea3446642");
            newScenedDef.cachedName = "gauntlet3";
            newScenedDef.nameToken = "gauntlet3";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("8ec8b500580b76a4784699d65e976224");
            newScenedDef.cachedName = "gauntlet4";
            newScenedDef.nameToken = "gauntlet4";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("c21260a8ebcc6444895a666b41508404");
            newScenedDef.cachedName = "gauntlet1";
            newScenedDef.nameToken = "gauntlet1";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("6b9e6940e27043d41ada2f7ad9c7c52c");
            newScenedDef.cachedName = "testscene";
            newScenedDef.nameToken = "testscene";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("b0b62ecea0ecf784c81033141eb87cd0");
            newScenedDef.cachedName = "testingscene";
            newScenedDef.nameToken = "testingscene";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);


            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneBlackbeach_Eclipse.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneBlackbeach.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneDampcave.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneEclipseClose.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneEclipseStandard.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneGoldshores.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneGolemplains.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneMoonFoggy.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneWispGraveyardSoot.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneRootJungleClear.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneRootJungleRain.asset").WaitForCompletion();


        }*/

        private static PickupPickerController.Option[] CommandWithEveryItem(On.RoR2.PickupPickerController.orig_GetOptionsFromPickupState orig, UniquePickup pickupIndex)
        {
            if (pickupIndex.pickupIndex == PickupCatalog.FindPickupIndex(JunkContent.Items.AACannon.itemIndex))
            {
                PickupPickerController.Option[] array = new PickupPickerController.Option[ItemCatalog.itemCount];
                for (int i = 0; i < ItemCatalog.itemCount; i++)
                {
                    //Debug.LogWarning(pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i));
                    array[i] = new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i)
                    };
                }
                return array;
            }
            else if (pickupIndex.pickupIndex == PickupCatalog.FindPickupIndex(JunkContent.Equipment.Enigma.equipmentIndex))
            {
                PickupPickerController.Option[] array = new PickupPickerController.Option[EquipmentCatalog.equipmentCount];
                for (int i = 0; i < EquipmentCatalog.equipmentCount; i++)
                {
                    //Debug.LogWarning(pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i));
                    array[i] = new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex((EquipmentIndex)i)
                    };
                }
                return array;
            }
            return orig(pickupIndex);
        }

    }

}