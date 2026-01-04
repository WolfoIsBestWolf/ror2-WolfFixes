using RoR2;
using RoR2.EntitlementManagement;
using RoR2.Items;
using RoR2.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoLibrary.Testing
{
    class Commands
    {
        public static bool cheatsEnabled = false;

        public static void Awake()
        {

            On.RoR2.EntitlementManagement.EntitlementAbstractions.VerifyLocalSteamUser += EntitlementAbstractions_VerifyLocalSteamUser;
        }
        public static List<EntitlementDef> disabledDefs;

        private static bool EntitlementAbstractions_VerifyLocalSteamUser(On.RoR2.EntitlementManagement.EntitlementAbstractions.orig_VerifyLocalSteamUser orig, EntitlementDef entitlementDef)
        {
            if (disabledDefs != null && disabledDefs.Contains(entitlementDef))
            {
                return false;
            }
            return orig(entitlementDef);
        }



        [ConCommand(commandName = "toggle_voidfiend", flags = ConVarFlags.ExecuteOnServer, helpText = "Toggle junk_unlimited. Makes skillchecks ingore junk cost.")]
        public static void CCToggleVoidFiend(ConCommandArgs args)
        {
            if (!args.senderBody)
            {
                Debug.Log("No Body");
                return;
            }
            VoidSurvivorController voidFiend = args.senderBody.GetComponent<VoidSurvivorController>();
            if (!voidFiend)
            {
                Debug.Log("No Void Fiend");
                return;
            }
            voidFiend.corruptionPerCrit *= -1;
            voidFiend.corruptionForFullDamage *= -1;
            voidFiend.corruptionPerSecondOutOfCombat *= -1;
            voidFiend.corruptionPerSecondInCombat *= -1;
            voidFiend._corruption = 0;
            Debug.Log(voidFiend.corruptionPerCrit > 0 ? "Enabled" : "Disabled" + "Void Fiend Corruption");
        }

        public static bool voidItems = true;
        [ConCommand(commandName = "toggle_voiditems", flags = ConVarFlags.ExecuteOnServer, helpText = "Toggle junk_unlimited. Makes skillchecks ingore junk cost.")]
        public static void CCToggleVoidItems(ConCommandArgs args)
        {
            if (voidItems)
            {
                Inventory.onInventoryChangedGlobal -= ContagiousItemManager.OnInventoryChangedGlobal;
            }
            else
            {
                Inventory.onInventoryChangedGlobal += ContagiousItemManager.OnInventoryChangedGlobal;
            }
            Debug.Log(voidItems ? "Enabled" : "Disabled" + "Void Item Corruption");
        }


        [ConCommand(commandName = "lock_dlc", flags = (ConVarFlags.None), helpText = "Disables given DLC by number")]
        private static void CCToggleDLC(ConCommandArgs args)
        {
            int expansion = args.TryGetArgInt(0).GetValueOrDefault(0);
            expansion--;
            if (expansion == -1)
            {
                Debug.Log("Need expansion number");
                return;
            }
            if (disabledDefs == null)
            {
                disabledDefs = new List<EntitlementDef>();
            }
            var expand = EntitlementCatalog.GetEntitlementDef((EntitlementIndex)expansion);

            if (disabledDefs.Contains(expand))
            {
                disabledDefs.Remove(expand);
                Debug.Log("Reallowing" + Language.GetString(expand.nameToken));
            }
            else
            {
                disabledDefs.Add(expand);
                Debug.Log("Disabling" + Language.GetString(expand.nameToken));
            }
            EntitlementManager.CCEntitlementForceRefresh(args);
        }

        [ConCommand(commandName = "purchase_all", flags = (ConVarFlags.None), helpText = "Purchase everything on the map")]
        private static void CCPurchaseAll(ConCommandArgs args)
        {
            //This dont even work bro
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            Interactor me = args.senderBody.GetComponent<Interactor>();
            foreach (PurchaseInteraction purchaseInteraction in InstanceTracker.GetInstancesList<PurchaseInteraction>())
            {
                purchaseInteraction.OnInteractionBegin(me);
            }
            foreach (BossGroup purchaseInteraction in InstanceTracker.GetInstancesList<BossGroup>())
            {
                purchaseInteraction.DropRewards();
            }
            foreach (HoldoutZoneController purchaseInteraction in InstanceTracker.GetInstancesList<HoldoutZoneController>())
            {
                purchaseInteraction.FullyChargeHoldoutZone();
            }
        }

        [ConCommand(commandName = "load", flags = (ConVarFlags.None), helpText = "Load given asset")]
        private static void CCLoad(ConCommandArgs args)
        {
            Debug.Log("Loading: " + args.TryGetArgString(0));
            Debug.Log(Addressables.LoadAssetAsync<UnityEngine.Object>(key: args.TryGetArgString(0)).WaitForCompletion());
        }

        [ConCommand(commandName = "random_unlocks", flags = (ConVarFlags.None), helpText = "RandomUnlocks (amount)")]
        private static void CCRandomUnlock(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            int a = args.TryGetArgInt(0).GetValueOrDefault(1);
            int done = 0;
            while (done < a)
            {
                int random = Run.instance.runRNG.RangeInt(0, UnlockableCatalog.unlockableCount);
                var def = UnlockableCatalog.GetUnlockableDef((UnlockableIndex)random);
                if (!def.hidden)
                {
                    done++;
                    foreach (NetworkUser networkUser in NetworkUser.readOnlyInstancesList)
                    {
                        if (networkUser.isParticipating)
                        {
                            networkUser.ServerHandleUnlock(def);
                            Debug.Log("Granting " + Language.GetString(def.nameToken));
                        }
                    }
                }

            }

        }


        [ConCommand(commandName = "clear_notifs", flags = ConVarFlags.None, helpText = "Clear Notifications")]
        public static void CC_clear_notifs(ConCommandArgs args)
        {

            var a = LocalUserManager.GetFirstLocalUser();
            var b = a.userProfile;
            var c = b.statSheet;
            if (b != null)
            {
                b.ClearAllAchievementNotifications();
            }
        }

        [ConCommand(commandName = "grant_all_unlocks", flags = ConVarFlags.None, helpText = "Grants all unlockables and achievements on current profile")]
        public static void CC_AllUnlocks(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            var a = LocalUserManager.GetFirstLocalUser();
            var b = a.userProfile;
            var c = b.statSheet;
            if (b != null)
            {
                for (int i = 0; AchievementManager.achievementDefs.Length > i; i++)
                {
                    b.AddAchievement(AchievementManager.achievementDefs[i].identifier, false);
                }
                for (int i = 0; UnlockableCatalog.unlockableCount > i; i++)
                {
                    c.AddUnlockable((UnlockableIndex)i);
                }
                for (int i = 0; BodyCatalog.bodyCount > i; i++)
                {
                    c.PushStatValue(PerBodyStatDef.timesSummoned, (BodyIndex)i, 1);
                }
                for (int i = 0; SurvivorCatalog.survivorCount > i; i++)
                {
                    c.PushStatValue(PerBodyStatDef.totalWins, SurvivorCatalog.survivorIndexToBodyIndex[i], 1);
                }
                for (int i = 0; PickupCatalog.pickupCount > i; i++)
                {
                    b.SetPickupDiscovered(new PickupIndex(i), true);
                }
                b.ClearAllAchievementNotifications();
                b.RequestEventualSave();
            }
        }

        [ConCommand(commandName = "remove_all_unlocks", flags = ConVarFlags.None, helpText = "Removes all unlockables and achievements on current profile")]
        public static void CC_RemoveUnlocks(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            var a = LocalUserManager.GetFirstLocalUser();
            var b = a.userProfile;
            var c = b.statSheet;

            if (b != null)
            {
                for (int i = 0; UnlockableCatalog.unlockableCount > i; i++)
                {
                    c.RemoveUnlockable((UnlockableIndex)i);
                }
                for (int i = b.achievementsList.Count - 1; 0 <= i; i--)
                {
                    b.RevokeAchievement(b.achievementsList[i]);
                }
                b.RequestEventualSave();
            }

        }





        [ConCommand(commandName = "simu_softlock", flags = ConVarFlags.SenderMustBeServer, helpText = "Attempts to end current wave or otherwise spawn a portal")]
        public static void CCSimuSoftlock(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }

            if (!Run.instance)
            {
                Debug.LogWarning("No Run");
            }
            ;
            if (Run.instance is not InfiniteTowerRun)
            {
                Debug.LogWarning("No Simu Run");
            }
            ;

            InfiniteTowerRun run = (InfiniteTowerRun)Run.instance;
            try
            {
                if (run.waveController)
                {
                    run.waveController.OnAllEnemiesDefeatedServer();
                    run.waveController.ForceFinish();
                }
                else
                {
                    run.OnWaveAllEnemiesDefeatedServer(null);
                    run.CleanUpCurrentWave();
                    if (run.waveInstance)
                    {
                        GameObject.Destroy(run.waveInstance);
                    }
                    if (run.IsStageTransitionWave())
                    {
                    }
                    else
                    {
                        run.BeginNextWave();

                    }
                }
                if (run.safeWardController)
                {
                    if (run.safeWardController.wardStateMachine.state is not EntityStates.InfiniteTowerSafeWard.Active)
                    {
                        run.safeWardController.wardStateMachine.SetState(new EntityStates.InfiniteTowerSafeWard.Active());
                    }

                }
            }
            catch (System.Exception e)
            {
                run.PickNextStageSceneFromCurrentSceneDestinations();
                DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(run.stageTransitionPortalCard, new DirectorPlacementRule
                {
                    minDistance = 0f,
                    maxDistance = run.stageTransitionPortalMaxDistance,
                    placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                    position = run.safeWardController.transform.position,
                    spawnOnTarget = run.safeWardController.transform
                }, run.safeWardRng));
                Chat.AddMessage(new Chat.SimpleChatMessage
                {
                    baseToken = run.stageTransitionChatToken
                });
                if (run.safeWardController)
                {
                    run.safeWardController.WaitForPortal();
                }
                Debug.LogWarning(e.ToString());
            }
            Debug.LogWarning("Report any errors");
        }


        [ConCommand(commandName = "all_items_test", flags = ConVarFlags.ExecuteOnServer, helpText = "Give every single item. 0/1 to include untiered. Deletes Void transformations. ")]
        public static void CCAllItems(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            int cas = args.TryGetArgInt(0).GetValueOrDefault(0);

            if (!args.senderMaster)
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            if (cas == 1)
            {
                for (int i = 0; i < ItemCatalog.itemCount; i++)
                {
                    try
                    {
                        args.senderMaster.inventory.GiveItemPermanent((ItemIndex)i);
                    }
                    catch (Exception _) { }
                }
                args.senderMaster.inventory.GiveItemPermanent(RoR2Content.Items.HealthDecay, 999999);
                ContagiousItemManager.originalToTransformed = Array.Empty<ItemIndex>();
                ContagiousItemManager.transformationInfos = Array.Empty<ContagiousItemManager.TransformationInfo>();

            }
            else
            {
                for (int i = 0; i < ItemCatalog.itemCount; i++)
                {
                    ItemDef def = ItemCatalog.GetItemDef((ItemIndex)i);
                    if (def.tier != ItemTier.NoTier)
                    {
                        if (cas != 2 && def.tier != ItemTier.NoTier && def.tier != ItemTier.VoidBoss && def.tier != ItemTier.VoidTier1 && def.tier != ItemTier.VoidTier2 && def.tier != ItemTier.VoidTier3)
                        {
                            try
                            {
                                args.senderMaster.inventory.GiveItemPermanent((ItemIndex)i);
                            }
                            catch (Exception _) { }
                        }
                        try
                        {
                            args.senderMaster.inventory.GiveItemPermanent((ItemIndex)i);
                        }
                        catch (Exception _) { }
                    }
                }
            }
            GameObject.Destroy(args.senderMaster.GetComponent<MasterSuicideOnTimer>());

        }

        [ConCommand(commandName = "all_buffs_test", flags = ConVarFlags.ExecuteOnServer, helpText = "Give every buff.")]
        public static void CCALLBuffs(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }

            if (!args.senderBody)
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            CharacterBody body = args.senderBody;
            for (int i = 0; i < BuffCatalog.buffCount; i++)
            {
                args.senderBody.AddBuff((BuffIndex)i);
            }


        }

    }

}
