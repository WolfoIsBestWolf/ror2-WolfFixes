using EntityStates.Engi.SpiderMine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{
    internal class Audio
    {

        public static void UnusedSounds()
        {

            //Add unused cool bubbly noise
            On.RoR2.ShopTerminalBehavior.PreStartClient += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron,") || self.name.StartsWith("ShrineCleanse"))
                {
                    Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                }
            };
            On.RoR2.ShopTerminalBehavior.DropPickup += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron") || self.name.StartsWith("ShrineCleanse"))
                {
                    Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                }
                ;
            };

            //Unused Scav Spawn Sound
            On.EntityStates.ScavMonster.Sit.OnEnter += (orig, self) =>
            {
                orig(self);
                if (!self.outer.gameObject) { return; }
                Util.PlaySound(EntityStates.ScavMonster.Sit.soundString, self.outer.gameObject);
            };


            //Pretty sure they added these sounds at some point but keeping it for good measure I guess
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += (orig, self) =>
            {
                orig(self);
                Util.PlaySound("Play_item_lunar_specialReplace_apply", self.outer.gameObject);
            };


            //Awkward and delayed
            LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect").GetComponent<EffectComponent>().soundName = "Play_item_proc_extraLife";
            LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/VoidRezEffect").GetComponent<EffectComponent>().soundName = "Play_item_void_extraLife";

            //On.RoR2.CharacterMaster.ExtraLifeServerBehavior.Awake += ExtraLifeServerBehavior_Awake;
            // On.RoR2.CharacterMaster.ExtraLifeServerBehavior.FixedUpdate += ExtraLifeServerBehavior_FixedUpdate;

        }

        private static void ExtraLifeServerBehavior_FixedUpdate(On.RoR2.CharacterMaster.ExtraLifeServerBehavior.orig_FixedUpdate orig, CharacterMaster.ExtraLifeServerBehavior self)
        {
            if (self.soundTime.hasPassed)
            {
                self.soundTime = Run.FixedTimeStamp.positiveInfinity;
                System.Action action = self.soundCallback;
                if (action != null)
                {
                    action();
                }
            }
            orig(self);
        }

        private static void ExtraLifeServerBehavior_Awake(On.RoR2.CharacterMaster.ExtraLifeServerBehavior.orig_Awake orig, CharacterMaster.ExtraLifeServerBehavior self)
        {
            orig(self);
            self.soundTime += 1;
        }

        private static void CharacterMaster_PlayExtraLifeSFX(On.RoR2.CharacterMaster.orig_PlayExtraLifeSFX orig, CharacterMaster self)
        {
            Debug.Log(self.bodyInstanceObject);
            orig(self);
        }

        public static void Start()
        {
            UnusedSounds();

            //Would be nice to sync sound only to spawn if it has missiles
            IL.RoR2.BarrageOnBossBehaviour.StartMissileCountdown_bool_float += WarBondsNoise_OnlyIfActuallyMissile;
 
            On.EntityStates.Engi.SpiderMine.Detonate.OnEnter += FixClientNoiseSpam;


            //Spawn sound not actually set?
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBoss/RoboBallMiniBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallRedBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallGreenBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";

        }

        private static void FixClientNoiseSpam(On.EntityStates.Engi.SpiderMine.Detonate.orig_OnEnter orig, EntityStates.Engi.SpiderMine.Detonate self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                float duration = 0.25f;
                self.PlayAnimation("Base", BaseSpiderMineState.IdleToArmedStateHash, BaseSpiderMineState.IdleToArmedParamHash, duration);
                foreach (string childName in self.childLocatorStringToDisable)
                {
                    Transform transform = self.FindModelChild(childName);
                    if (transform)
                    {
                        transform.gameObject.SetActive(false);
                    }
                }
            }
        }





        private static void WarBondsNoise_OnlyIfActuallyMissile(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("RoR2.BarrageOnBossBehaviour", "missileStartPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<GameObject, BarrageOnBossBehaviour, GameObject>>((effect, self) =>
                {
                    if (self.barrageQuantity == 0)
                    {
                        return null;
                    }
                    return effect;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed: WarBondsNoise_OnlyIfActuallyMissile");
            }
        }





    }

}