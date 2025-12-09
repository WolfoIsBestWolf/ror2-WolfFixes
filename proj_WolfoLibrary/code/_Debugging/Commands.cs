using RoR2;
using RoR2.EntitlementManagement;
using RoR2.Items;
using RoR2.Stats;
using RoR2.UI;
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
            On.RoR2.TeamManager.Start += TeamManager_Start;
            SceneDirector.onPrePopulateSceneServer += SceneDirector_onPrePopulateSceneServer;

            /*var targetMethod = typeof(BaseUserEntitlementTracker<IDisposable>).GetMethod(nameof(BaseUserEntitlementTracker<IDisposable>.UserHasEntitlement), BindingFlags.NonPublic | BindingFlags.Instance);
            var destMethod = typeof(Commands).GetMethod(nameof(UserHasEntitlement), BindingFlags.Public | BindingFlags.Static);
            var overrideHook = new Hook(targetMethod, destMethod);*/

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



        private static void TeamManager_Start(On.RoR2.TeamManager.orig_Start orig, TeamManager self)
        {
            orig(self);

            if (godEnemyBool)
            {
                TeamManager.instance.teamLevels[2] = godEnemyBool ? (uint)100001 : (uint)0;
            }
        }

        public static void SceneDirector_onPrePopulateSceneServer(SceneDirector obj)
        {
            if (noInteractables)
            {
                obj.interactableCredit = 0;
                obj.monsterCredit = 0;
                obj.teleporterSpawnCard = null;
            }
        }

        [ConCommand(commandName = "purchase_all", flags = (ConVarFlags.None), helpText = "Purchase everything on the map")]
        private static void CCPurchaseAll(ConCommandArgs args)
        {
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
        [ConCommand(commandName = "kill_player", flags = (ConVarFlags.None), helpText = "Kill all players")]
        private static void CCkill_player(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                player.master.TrueKill();
            }
        }
        [ConCommand(commandName = "randomitems", flags = (ConVarFlags.None), helpText = "RandomItems (amount)")]
        private static void CCRandomItems(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            args.senderBody.inventory.GiveRandomItems(args.TryGetArgInt(0).GetValueOrDefault(1), false, false);
        }
        [ConCommand(commandName = "randomequipment", flags = (ConVarFlags.None), helpText = "RandomEquipment")]
        private static void CCRandomEq(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            args.senderBody.inventory.GiveRandomEquipment();
        }
        [ConCommand(commandName = "randomunlocks", flags = (ConVarFlags.None), helpText = "RandomUnlocks (amount)")]
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
                        }
                    }
                }

            }
        }

        [ConCommand(commandName = "remove_equip", flags = (ConVarFlags.None), helpText = "Remove Equipment")]
        private static void CCRemoveEquip(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            args.senderBody.inventory.RemoveEquipment(0);
        }


        [ConCommand(commandName = "run_complete_wave", flags = (ConVarFlags.SenderMustBeServer | ConVarFlags.Cheat), helpText = "Set the current wave #")]
        private static void CCRunEndWave(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            InfiniteTowerRun infiniteTowerRun = Run.instance as InfiniteTowerRun;
            if (!infiniteTowerRun)
            {
                throw new ConCommandException("No Infinite Tower run is currently in progress.");
            }
            infiniteTowerRun.waveController.KillSquad();
            if ((bool)infiniteTowerRun.waveController.combatDirector)
            {
                infiniteTowerRun.waveController.combatDirector.monsterCredit = 0f;
            }
            infiniteTowerRun.waveController.totalWaveCredits = 0f;
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

        [ConCommand(commandName = "evolve_lemurian", flags = ConVarFlags.ExecuteOnServer, helpText = "Evolves all Devoted Lemurians")]
        [ConCommand(commandName = "evolve_lemurians", flags = ConVarFlags.ExecuteOnServer, helpText = "Evolves all Devoted Lemurians")]
        public static void CC_evolve_lemurian(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            DevotionInventoryController.ActivateAllDevotedEvolution();
        }
        [ConCommand(commandName = "poor", flags = ConVarFlags.ExecuteOnServer, helpText = "Set money to 0")]
        public static void CC_poor(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            args.senderMaster.money = 0;
        }
        [ConCommand(commandName = "rich", flags = ConVarFlags.ExecuteOnServer, helpText = "Set money to max")]
        public static void CC_rich(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            args.senderMaster.money = (uint.MaxValue/2);
        }

        [ConCommand(commandName = "cooldown", flags = ConVarFlags.ExecuteOnServer, helpText = "Removes Skill Cooldown for current stage")]
        [ConCommand(commandName = "nocooldown", flags = ConVarFlags.ExecuteOnServer, helpText = "Removes Skill Cooldown for current stage")]
        public static void CC_Cooldown(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                return;
            }
            GenericSkill[] slots = args.senderMaster.GetBody().GetComponents<GenericSkill>();
            foreach (GenericSkill slot in slots)
            {
                if (slot.cooldownOverride == 0)
                {
                    slot.cooldownOverride = 0.01f;
                }
                else
                {
                    slot.cooldownOverride = 0;
                }
            }
        }


        [ConCommand(commandName = "skill", flags = ConVarFlags.ExecuteOnServer, helpText = "Switch Skills of current survivor Shorthand")]
        public static void CC_Skill(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            BodyIndex argBodyIndex = args.senderMaster.GetBody().bodyIndex;
            int argInt = args.GetArgInt(0);
            int argInt2 = args.GetArgInt(1);
            UserProfile userProfile = args.GetSenderLocalUser().userProfile;
            Loadout loadout = new Loadout();
            userProfile.loadout.Copy(loadout);
            loadout.bodyLoadoutManager.SetSkillVariant(argBodyIndex, argInt, (uint)argInt2);
            userProfile.SetLoadout(loadout);
            if (args.senderMaster)
            {
                args.senderMaster.SetLoadoutServer(loadout);
            }
            if (args.senderBody)
            {
                args.senderBody.SetLoadoutServer(loadout);
            }
        }


        [ConCommand(commandName = "invis", flags = ConVarFlags.None, helpText = "Turn off your model for screenshots")]
        [ConCommand(commandName = "model", flags = ConVarFlags.None, helpText = "Turn off your model for screenshots")]
        public static void CC_TurnOffModel(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                WolfoLib.log.LogMessage("No Master");
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                WolfoLib.log.LogMessage("No Body");
                return;
            }
            GameObject mdl = args.senderMaster.GetBody().GetComponent<ModelLocator>().modelTransform.gameObject;
            mdl.SetActive(!mdl.activeSelf);

        }

        [ConCommand(commandName = "hud", flags = ConVarFlags.None, helpText = "Enable/disable the HUD")]
        [ConCommand(commandName = "ui", flags = ConVarFlags.None, helpText = "Enable/disable the HUD")]
        public static void CC_ToggleHUD(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            HUD.cvHudEnable.SetBool(!HUD.cvHudEnable.value);
        }

        public static bool noInteractables;
        [ConCommand(commandName = "no_interactables", flags = ConVarFlags.ExecuteOnServer, helpText = "Prevent interactables from being spawned")]
        public static void CC_NoInteractables(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            noInteractables = !noInteractables;
            WolfoLib.log.LogMessage(noInteractables ? "Interactables no longer spawn" : "Interactables spawn again");
        }

        public static bool godEnemyBool;
        [ConCommand(commandName = "godenemy", flags = ConVarFlags.ExecuteOnServer, helpText = "Inflate enemy level to make them invulnerable")]
        public static void CCGodEnemy(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            godEnemyBool = !godEnemyBool;
            TeamManager.instance.teamLevels[2] = godEnemyBool ? (uint)10000 : (uint)0;
            WolfoLib.log.LogMessage(godEnemyBool ? "Enemy level set to 10000" : "Enemy level set back to normal");
        }

        [ConCommand(commandName = "scanner", flags = ConVarFlags.ExecuteOnServer, helpText = "Give Radar Scanner and Equipment Cooldown Reduction hidden item")]
        [ConCommand(commandName = "give_scanner", flags = ConVarFlags.ExecuteOnServer, helpText = "Give Radar Scanner and Equipment Cooldown Reduction hidden item")]
        public static void CCScanner(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            args.senderMaster.inventory.SetEquipmentIndex(RoR2Content.Equipment.Scanner.equipmentIndex);
            args.senderMaster.inventory.GiveItem(RoR2Content.Items.BoostEquipmentRecharge, 100);
        }

        [ConCommand(commandName = "set_damage", flags = ConVarFlags.ExecuteOnServer, helpText = "Sets damage for testing.")]
        public static void CC_Damage(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                WolfoLib.log.LogMessage("No Body");
                return;
            }
            float newDamage = (float)Convert.ToInt16(args[0]);
            args.senderMaster.GetBody().baseDamage = newDamage;
            args.senderMaster.GetBody().levelDamage = 0;
            args.senderMaster.GetBody().damage = newDamage;

        }

        [ConCommand(commandName = "set_health", flags = ConVarFlags.ExecuteOnServer, helpText = "Sets health and removes regen, for testing.")]
        public static void CC_SetHP(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                WolfoLib.log.LogMessage("No Body");
                return;
            }
            float newDamage = (float)Convert.ToInt16(args[0]);
            var body = args.senderMaster.GetBody();
            body.baseMaxHealth = newDamage;
            body.levelMaxHealth = 0;
            body.maxHealth = newDamage;
            body.baseRegen = 0;
            body.levelRegen = 0;
            body.regen = 0;

            body.healthComponent.health = body.maxHealth;
        }

        [ConCommand(commandName = "speed", flags = ConVarFlags.None, helpText = "Speed")]
        [ConCommand(commandName = "set_speed", flags = ConVarFlags.None, helpText = "Speed")]
        public static void CC_SetSpeed(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                WolfoLib.log.LogMessage("No Body");
                return;
            }
            float newNumb = (float)Convert.ToInt16(args[0]);
            var body = args.senderMaster.GetBody();
            body.moveSpeed = newNumb;
            body.baseMoveSpeed = newNumb;
            body.levelMoveSpeed = 0;

            body.baseJumpCount = 10000;
            body.maxJumpCount = 10000;
            body.baseAcceleration = body.baseAcceleration / 7 * newNumb;
            body.acceleration = body.baseAcceleration;
        }


        [ConCommand(commandName = "goto_boss", flags = ConVarFlags.None, helpText = "Tp to Teleporter, Mithrix or False son depending on stage.")]
        public static void CC_GotoMithrix(ConCommandArgs args)
        {
            if (!cheatsEnabled)
            {
                Debug.Log("WolfoLibrary commands are only available with DebugToolkit installed");
                return;
            }
            Component senderBody = args.GetSenderBody();
            Vector3 newPosition = Vector3.zero;
            if (Stage.instance.sceneDef.cachedName == "moon2")
            {
                newPosition = new Vector3(-11, 490, 80);
            }
            else if (Stage.instance.sceneDef.cachedName == "meridian")
            {
                newPosition = new Vector3(85.2065f, 146.5167f, -70.5265f);
            }
            else if (Stage.instance.sceneDef.cachedName == "mysteryspace")
            {
                newPosition = new Vector3(362.9097f, -151.5964f, 213.0157f);
            }
            else if (Stage.instance.sceneDef.cachedName == "voidraid")
            {
                bool crab = false;
                for (int i = 0; i < CharacterBody.instancesList.Count; i++)
                {
                    if (CharacterBody.instancesList[i].name.StartsWith("MiniVoidRaid"))
                    {
                        crab = true;
                        newPosition = CharacterBody.instancesList[i].corePosition;
                    }
                }
                if (!crab)
                {
                    newPosition = new Vector3(-105f, 0.2f, 92f);
                }

            }
            else if (TeleporterInteraction.instance)
            {
                newPosition = TeleporterInteraction.instance.transform.position;
            }
            else
            {
                for (int i = 0; i < CharacterBody.instancesList.Count; i++)
                {
                    if (CharacterBody.instancesList[i].isBoss)
                    {
                        newPosition = CharacterBody.instancesList[i].corePosition;
                    }
                }
            }
            if (newPosition == Vector3.zero)
            {
                WolfoLib.log.LogMessage("No Teleporter, Specific Location or Boss Monster found.");
                return;
            }
            TeleportHelper.TeleportGameObject(senderBody.gameObject, newPosition);
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
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage
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
