using EntityStates.Fauna;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{

    internal class BodyFixes
    {

        public static void FixBarnalceMinorsSpammingNotImplementedWithOpsTransport(System.Action<PseudoCharacterMotor, Vector3> orig, PseudoCharacterMotor self, Vector3 newVelocity)
        {
            return;
        }

        private static void Inventory_SetActiveEquipmentSlot(On.RoR2.Inventory.orig_SetActiveEquipmentSlot orig, Inventory self, byte slotIndex)
        {
            self.wasRecentlyExtraEquipmentSwapped = true;
            orig(self, slotIndex);
        }

        private static void WhirlwindGround_OnEnter(On.EntityStates.Merc.WhirlwindGround.orig_OnEnter orig, EntityStates.Merc.WhirlwindGround self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                self.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, 0.8f, 1);
            }
        }

        //On.RoR2.GenericSkill.SetSkillOverride += FixHeresyForEnemies;
        //Is this even still needed check that
        private static void FixHeresyForEnemies(On.RoR2.GenericSkill.orig_SetSkillOverride orig, GenericSkill self, object source, RoR2.Skills.SkillDef skillDef, GenericSkill.SkillOverridePriority priority)
        {
            if (priority == GenericSkill.SkillOverridePriority.Replacement && self.characterBody && !self.characterBody.isPlayerControlled)
            {
                //Why do I do this again?
                EntityStateMachine stateMachine = self.stateMachine;
                orig(self, source, skillDef, priority);
                if (stateMachine)
                {
                    self.stateMachine = stateMachine;
                }
                return;
            }
            else
            {
                orig(self, source, skillDef, priority);
            }
        }

        public static void Start()
        {
            //Added in AC
            //On.EntityStates.Merc.WhirlwindBase.OnEnter += WhirlwindBase_OnEnter;

            //Move this somewhere
            /*On.EntityStates.Croco.Spawn.OnEnter += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active)
                {
                    self.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                }
            };
            On.EntityStates.Croco.Spawn.OnExit += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active)
                {
                    self.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                    self.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 3f);
                }
            };*/



            //Forgive me Please marked as ???
            CharacterBody DeathProjectile = Addressables.LoadAssetAsync<GameObject>(key: "1336d77e77299964884c3bd02757fde7").WaitForCompletion().GetComponent<CharacterBody>();
            DeathProjectile.baseNameToken = "EQUIPMENT_DEATHPROJECTILE_NAME";
            DeathProjectile.portraitIcon = Addressables.LoadAssetAsync<Texture2D>(key: "dda5febead506894fa6e053cea042ddc").WaitForCompletion();


            #region Mushroom tree do not produce fruit or die or whatever 
            On.EntityStates.Fauna.HabitatFruitDeathState.OnEnter += FixDumbFruit;

            HabitatFruitDeathState.deathSoundString = "Play_jellyfish_death";
            HabitatFruitDeathState.healPackMaxVelocity = 60;
            HabitatFruitDeathState.fractionalHealing = 0.15f;
            HabitatFruitDeathState.scale = 1;
            #endregion

            //Fix Captain Beacons not critting
            IL.EntityStates.CaptainSupplyDrop.HitGroundState.OnEnter += FixCaptainBeaconNoCrit;


            //For testing ig but also it spams the console
            IL.EntityStates.Commando.CommandoWeapon.FirePistol2.FixedUpdate += CommandoReloadStateRemove;
            //Huntress issue only starts at 780% attack speed who cares really

            #region XI 
            //Fix Ghost XI not spawning Ghosts
            //Fix Elite XI not spawning Elites
            //Hopoo forgor
            On.RoR2.NetworkedBodySpawnSlot.OnSpawnedServer += XI_GhostEliteMinionFix;
            //Fix XI Tail lagging
            //GameObject MegaConstructBody = Addressables.LoadAssetAsync<GameObject>(key: "64b97b2c7e3e0d949b41abbe57bf3c2d").WaitForCompletion();
            //MegaConstructBody.transform.Find("Model Base/mdlMegaConstruct/MegaConstructArmature/ROOT/base/body.1").GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;

            //EntityStates.MegaConstruct.ExitShield has wrong field name, leading to it being skipped
            Addressables.LoadAssetAsync<EntityStateConfiguration>(key: "04deef0aeeb41dc4aa4ad14f7f967526").WaitForCompletion().serializedFieldsCollection.serializedFields[3].fieldName = "baseDuration"; //Is Duration instead of Base
            #endregion
            #region CHEF
            //BoostedSearFireballProjectile has childDmgCoeff of 0 resulting in 0 damage Oil Pools
            //Oil pools normally do 20% damage 
            Addressables.LoadAssetAsync<GameObject>(key: "c00152ae354576245849336ff7e67ba6").WaitForCompletion().GetComponent<ProjectileExplosion>().childrenDamageCoefficient = 2f / 70f;
            #endregion

            #region ScavLunar

            //Twisted Scavengers are final bosses
            //Final bosses are *meant* to be immune to Newly Hatched Zoea
            //Hopoo forgor

            Addressables.LoadAssetAsync<GameObject>(key: "746b53f076ca9af4d89f67c981d2bbf9").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "a0a8fa4272069874b9e538c59bbda5ed").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "7dfb4548829852a49a4b2840046787ed").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "769510dc6be546b40aa3aca3cf93945c").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            #endregion

            Addressables.LoadAssetAsync<GameObject>(key: "8684165ea0542bf4bae1eea1f5865386").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "9d63f2d3bc6c52c44883128cc4b97bf4").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar

            DLC3Fixes();


            On.EntityStates.Merc.WhirlwindGround.OnEnter += WhirlwindGround_OnEnter;

            On.RoR2.Inventory.SetActiveEquipmentSlot += Inventory_SetActiveEquipmentSlot;

            //Scores problem now.
            /*var targetMethod = typeof(PseudoCharacterMotor).GetProperty(nameof(PseudoCharacterMotor.velocityAuthority), BindingFlags.Public | BindingFlags.Instance).GetSetMethod();
            var destMethod = typeof(BodyFixes).GetMethod(nameof(FixBarnalceMinorsSpammingNotImplementedWithOpsTransport), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var overrideHook2 = new Hook(targetMethod, destMethod);*/


        }

        public static void DLC3Fixes()
        {
            //Broke close to launch; No longer gets into position and just fucking sucks because of it.
            SkillDef DroneJailerTrapSkillDef = Addressables.LoadAssetAsync<SkillDef>(key: "1df669ea25cc9cc408ff56b4a2571af9").WaitForCompletion();
            DroneJailerTrapSkillDef.activationState.typeName = "EntityStates.Drone.DroneJailer.AssumePosition";

            //Fucked up pathing or smth I don't really know much about it
            Hook hook = new Hook(typeof(CharacterBody).GetProperty("footPosition", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), FuckedUpFootPosition);

            On.RoR2.CharacterBody.CheckDroneHasItems += CharacterBody_CheckDroneHasItems;

            //On.RoR2.SolusWebMissionController.EncounterHealthThresholdController_onAllMembersReachedThreshold += SolusWebMissionController_EncounterHealthThresholdController_onAllMembersReachedThreshold;

            On.RoR2.Items.PhysicsProjectileBehavior.InheritMovementItems += PhysicsProjectileBehavior_InheritMovementItems;

            On.EntityStates.SolusHeart.Death.MissionCompleted.FixedUpdate += MissionCompleted_FixedUpdate;
        }

        private static void MissionCompleted_FixedUpdate(On.EntityStates.SolusHeart.Death.MissionCompleted.orig_FixedUpdate orig, EntityStates.SolusHeart.Death.MissionCompleted self)
        {
            for (int i = 0; i < self.combatSquad.readOnlyMembersList.Count; i++)
            {
                CharacterMaster characterMaster = self.combatSquad.readOnlyMembersList[i];
                if (characterMaster)
                {
                    CharacterBody body = characterMaster.GetBody();
                    if (body)
                    {
                        characterMaster.TrueKill();
                        //body.healthComponent.Suicide(null, null, default(DamageTypeCombo));
                    }
                }
            }
            orig(self);
        }

        private static void PhysicsProjectileBehavior_InheritMovementItems(On.RoR2.Items.PhysicsProjectileBehavior.orig_InheritMovementItems orig, RoR2.Items.PhysicsProjectileBehavior self, Inventory friendInventory)
        {
            orig(self, friendInventory);
            if (friendInventory)
            {
                if (Run.instance.ambientLevel > 99)
                {
                    friendInventory.ResetItemPermanent(RoR2Content.Items.WardOnLevel);
                }
            }

        }

        private static void SolusWebMissionController_EncounterHealthThresholdController_onAllMembersReachedThreshold(On.RoR2.SolusWebMissionController.orig_EncounterHealthThresholdController_onAllMembersReachedThreshold orig, SolusWebMissionController self, int threshold)
        {
            if (threshold == 3)
            {
                foreach (var master in self.combatSquad.readOnlyMembersList)
                {
                    if (!master.IsDeadAndOutOfLivesServer())
                    {
                        threshold = 2;
                    }
                }
            }
            orig(self, threshold);
        }

        private static bool CharacterBody_CheckDroneHasItems(On.RoR2.CharacterBody.orig_CheckDroneHasItems orig, CharacterBody self)
        {
            bool item = orig(self);
            /*if (!self.IsDrone && (self.bodyFlags & CharacterBody.BodyFlags.Mechanical) > CharacterBody.BodyFlags.None)
            {
                List<ItemIndex> itemAcquisitionOrder = self.inventory.itemAcquisitionOrder;
                for (int i = 0; i < itemAcquisitionOrder.Count; i++)
                {
                    if (!ItemCatalog.GetItemDef(itemAcquisitionOrder[i]).ContainsTag(ItemTag.HiddenForDroneBuffIcon))
                    {
                        self.bodyFlags |= CharacterBody.BodyFlags.DroneHasItems;
                        item = true;
                    }
                }
            }*/
            if ((self.bodyFlags & CharacterBody.BodyFlags.Mechanical) > CharacterBody.BodyFlags.None)
            {
                if (self.inventory.GetItemCountPermanent(RoR2Content.Items.CaptainDefenseMatrix) > 0 ||
                    self.inventory.GetItemCountPermanent(DLC1Content.Items.DroneWeaponsBoost) > 0 ||
                    self.inventory.GetItemCountPermanent(DLC3Content.Items.DroneDynamiteDisplay) > 0 ||
                    self.inventory.GetItemCountPermanent(DLC3Content.Items.TransferDebuffOnHit) > 0
                    )
                {
                    self.bodyFlags |= CharacterBody.BodyFlags.DroneHasItems;
                    item = true;
                }
            }
            else if ((self.bodyFlags & CharacterBody.BodyFlags.Devotion) > CharacterBody.BodyFlags.None)
            {
                if (
                    self.inventory.GetItemCountPermanent(DLC1Content.Items.DroneWeaponsBoost) > 0 ||
                    self.inventory.GetItemCountPermanent(DLC3Content.Items.DroneDynamiteDisplay) > 0
                    )
                {
                    self.bodyFlags |= CharacterBody.BodyFlags.DroneHasItems;
                    item = true;
                }
            }
            if (!item && self.bodyFlags.HasFlag(CharacterBody.BodyFlags.DroneHasItems))
            {
                self.bodyFlags &= ~CharacterBody.BodyFlags.DroneHasItems;
            }
            return item;
        }

        public delegate Vector3 orig_footPosition(CharacterBody self);
        public static Vector3 FuckedUpFootPosition(BodyFixes.orig_footPosition orig, CharacterBody self)
        {
            Vector3 a = orig(self);
            if (self.characterMotor)
            {
                a.y += self.characterMotor.capsuleYOffset;
            }
            return a;
        }

        public static void CallLate()
        {
            //Makes artifact borderline unusable, but probably overstepping fix territory
            //CU8Content.BodyPrefabs.DevotedLemurianBody.bodyFlags |= CharacterBody.BodyFlags.ImmuneToLava;
            //CU8Content.BodyPrefabs.DevotedLemurianBruiserBody.bodyFlags |= CharacterBody.BodyFlags.ImmuneToLava;
            //Child shouldnt be burnable like Parents
            DLC2Content.BodyPrefabs.ChildBody.bodyFlags |= CharacterBody.BodyFlags.OverheatImmune;

            //SKY MEADOW ROCKS OWN THEM  SELF

            GameObject ROCK = null;
            ROCK = Addressables.LoadAssetAsync<GameObject>(key: "1fb51531942733b469329cbcd0647a68").WaitForCompletion();
            ROCK.GetComponent<ProjectileController>().owner = ROCK;
            ROCK = Addressables.LoadAssetAsync<GameObject>(key: "40316250be8e9a049b87745e197820e2").WaitForCompletion();
            ROCK.GetComponent<ProjectileController>().owner = ROCK;
            ROCK = Addressables.LoadAssetAsync<GameObject>(key: "40316250be8e9a049b87745e197820e2").WaitForCompletion();
            ROCK.GetComponent<ProjectileController>().owner = ROCK;
            ROCK = Addressables.LoadAssetAsync<GameObject>(key: "fa0f995f3a42e244db914ac7d61cab47").WaitForCompletion();
            ROCK.GetComponent<ProjectileController>().owner = ROCK;
            ROCK = Addressables.LoadAssetAsync<GameObject>(key: "efcf575c05e1ea543be85f5cac0c12fd").WaitForCompletion();
            ROCK.GetComponent<ProjectileController>().owner = ROCK;
            ROCK = Addressables.LoadAssetAsync<GameObject>(key: "d59e337394dd72f418dbb1b622d2480d").WaitForCompletion();
            ROCK.GetComponent<ProjectileController>().owner = ROCK;



        }

        public static void XI_GhostEliteMinionFix(On.RoR2.NetworkedBodySpawnSlot.orig_OnSpawnedServer orig, NetworkedBodySpawnSlot self, GameObject ownerBodyObject, SpawnCard.SpawnResult spawnResult, System.Action<MasterSpawnSlotController.ISlot, SpawnCard.SpawnResult> callback)
        {
            orig(self, ownerBodyObject, spawnResult, callback);
            if (spawnResult.success && spawnResult.spawnedInstance && ownerBodyObject)
            {
                CharacterBody ownerBody = ownerBodyObject.GetComponent<CharacterBody>();
                Inventory component = spawnResult.spawnedInstance.GetComponent<Inventory>();
                if (component)
                {
                    if (WConfig.cfgXIEliteFix.Value)
                    {
                        component.CopyEquipmentFrom(ownerBody.inventory, false);
                    }
                    if (ownerBody.inventory.GetItemCountPermanent(RoR2Content.Items.Ghost) > 0)
                    {
                        component.GiveItemPermanent(RoR2Content.Items.Ghost, 1);
                        component.GiveItemPermanent(RoR2Content.Items.HealthDecay, 30);
                        component.GiveItemPermanent(RoR2Content.Items.BoostDamage, 150);
                    }
                }
            }

        }



        private static void FallDamageImmunityOnOilSpillCancel(On.EntityStates.Chef.OilSpillBase.orig_OnExit orig, EntityStates.Chef.OilSpillBase self)
        {
            //Intended for when cancelled with YES Chef,
            //Would be more ideal to have a way where hitting an enemy and dropping still does damage
            //Like how Acrid has it
            self.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, 0.25f);
            orig(self);

        }

        private static void FixCaptainBeaconNoCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStfld("RoR2.BulletAttack", "isCrit")))
            {
                c.Emit(OpCodes.Ldloc_1);
                c.EmitDelegate<System.Func<bool, ProjectileDamage, bool>>((a, projectileDamage) =>
                {
                    return projectileDamage.crit;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : FixCaptainBeaconNoCrit");
            }
        }

        private static void XI_LaserFix(On.EntityStates.MajorConstruct.Weapon.FireLaser.orig_OnExit orig, EntityStates.MajorConstruct.Weapon.FireLaser self)
        {
            orig(self);
            self.outer.SetNextState(self.GetNextState());
        }




        //public static GameObject JellyfishDeath;
        private static void FixDumbFruit(On.EntityStates.Fauna.HabitatFruitDeathState.orig_OnEnter orig, EntityStates.Fauna.HabitatFruitDeathState self)
        {
            //self.outer.SetNextState(new EntityStates.Fauna.VultureEggDeathState());

            orig(self);
            Transform Fruit = self.characterBody.mainHurtBox.transform;
            EffectManager.SimpleImpactEffect(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Jellyfish/JellyfishDeath.prefab").WaitForCompletion(), Fruit.position, Vector3.up, false);
            if (NetworkServer.active)
            {

                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/HealPack"), Fruit.position, UnityEngine.Random.rotation);
                gameObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
                gameObject.GetComponentInChildren<HealthPickup>().fractionalHealing = HabitatFruitDeathState.fractionalHealing;
                gameObject.transform.localScale = new Vector3(HabitatFruitDeathState.scale, HabitatFruitDeathState.scale, HabitatFruitDeathState.scale);
                gameObject.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere * HabitatFruitDeathState.healPackMaxVelocity, ForceMode.VelocityChange);
                NetworkServer.Spawn(gameObject);
            }

        }




        public static void CommandoReloadStateRemove(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCallvirt("RoR2.GenericSkill", "get_stock")))
            {
                c.EmitDelegate<System.Func<int, int>>((stock) =>
                {
                    return 1;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : CommandoReloadStateRemove");
            }
        }


        private static void WhirlwindBase_OnEnter(On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, EntityStates.Merc.WhirlwindBase self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                self.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, self.duration);
            }
        }
    }

    internal class MithrixPhase4Fix : MonoBehaviour
    {
        public bool stoleItems = false;
        public void Start()
        {
            //-> PreAwake Fix Hurtable in body prefab
            //-> Awake does his thing
            //-> Start, we need to turn them on again if he's not doing the animation.
            SetStateOnHurt component = this.GetComponent<SetStateOnHurt>();
            if (component)
            {
                component.canBeHitStunned = true;
            }
            HurtBoxGroup hurtBoxGroup = this.GetComponent<HurtBoxGroup>();
            if (hurtBoxGroup)
            {
                //WolfoMain.Logger.LogMessage(component.hurtBoxesDeactivatorCounter);
                if (hurtBoxGroup.hurtBoxesDeactivatorCounter == 0)
                {
                    hurtBoxGroup.SetHurtboxesActive(true);
                }
            }
        }
    }

}
