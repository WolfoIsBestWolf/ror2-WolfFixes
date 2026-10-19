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

        public static void Start()
        {
            //Forgive me Please name shows as ??? in log
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


            //EntityStates.MegaConstruct.ExitShield has wrong field name, leading to it being skipped
            Addressables.LoadAssetAsync<EntityStateConfiguration>(key: "04deef0aeeb41dc4aa4ad14f7f967526").WaitForCompletion().serializedFieldsCollection.serializedFields[3].fieldName = "baseDuration"; //Is Duration instead of Base
            #region CHEF
            //BoostedSearFireballProjectile has childDmgCoeff of 0 resulting in 0 damage Oil Pools
            //Oil pools normally do 20% damage 
            Addressables.LoadAssetAsync<GameObject>(key: "c00152ae354576245849336ff7e67ba6").WaitForCompletion().GetComponent<ProjectileExplosion>().childrenDamageCoefficient = 2f / 70f;
            #endregion

            #region ScavLunar
            //Twisted Scavengers are final bosses which are meant to be immune to Newly Hatched Zoea
            Addressables.LoadAssetAsync<GameObject>(key: "746b53f076ca9af4d89f67c981d2bbf9").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "a0a8fa4272069874b9e538c59bbda5ed").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "7dfb4548829852a49a4b2840046787ed").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            Addressables.LoadAssetAsync<GameObject>(key: "769510dc6be546b40aa3aca3cf93945c").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath; //ScavLunar
            #endregion

            DLC3Fixes();
 
            //What was this for
            //IL.RoR2.CharacterMaster.IsDeadAndOutOfLivesServer += ReplaceDroneCheckWithRemoteOpDroneCheck;

        }

        private static void ReplaceDroneCheckWithRemoteOpDroneCheck(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchCallvirt("RoR2.CharacterBody", "get_IsDrone")))
            {
                c.Remove();
                c.EmitDelegate<System.Func<CharacterBody, bool>>((body) =>
                {
                    return body.isRemoteOp;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : IsDeadAndOutOfLivesServer");
            }
        }


        public static void DLC3Fixes()
        {
            //Broke close to launch; No longer gets into position and just fucking sucks because of it.
            SkillDef DroneJailerTrapSkillDef = Addressables.LoadAssetAsync<SkillDef>(key: "1df669ea25cc9cc408ff56b4a2571af9").WaitForCompletion();
            DroneJailerTrapSkillDef.activationState.typeName = "EntityStates.Drone.DroneJailer.AssumePosition";


            On.RoR2.CharacterBody.CheckDroneHasItems += CharacterBody_CheckDroneHasItems;

            //On.RoR2.SolusWebMissionController.EncounterHealthThresholdController_onAllMembersReachedThreshold += SolusWebMissionController_EncounterHealthThresholdController_onAllMembersReachedThreshold;

            On.RoR2.Items.PhysicsProjectileBehavior.InheritMovementItems += RequestedWarbannerBehavior;
 
        }
 
        private static void RequestedWarbannerBehavior(On.RoR2.Items.PhysicsProjectileBehavior.orig_InheritMovementItems orig, RoR2.Items.PhysicsProjectileBehavior self, Inventory friendInventory)
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
 
        private static bool CharacterBody_CheckDroneHasItems(On.RoR2.CharacterBody.orig_CheckDroneHasItems orig, CharacterBody self)
        {
            bool item = orig(self);
            if (self.inventory == null)
            {
                return item;
            }
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
                if (self.inventory.GetItemCountPermanent(DLC1Content.Items.DroneWeaponsBoost) > 0 ||
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

        public static void CallLate()
        {
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
