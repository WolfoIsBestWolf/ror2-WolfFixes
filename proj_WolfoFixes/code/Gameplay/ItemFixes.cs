using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Navigation;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    internal class ItemFixes
    {
        public static void Start()
        {

            IL.RoR2.CharacterBody.RecalculateStats += FixBolsteringLanternBeingPlus10;
            IL.RoR2.CharacterBody.RecalculateStats += FixStoneFluxBeingAppliedTwice;

            IL.RoR2.GlobalEventManager.ProcessHitEnemy += FixChargedPerferatorCrit;

            //Checks for NetworkAuth instead of EffectiveAuth
            //Antler does not work on Lemurians due to wrong Auth Check
            //Antler does not work on Stationary Turrets / Flying Drones due to requiring normal character motor.
            IL.RoR2.CharacterBody.RpcRequestShardInfoClient += Antler_FixWrongAuth;
            On.RoR2.CharacterBody.RpcRequestShardInfoClient += Antler_FixNullRefOnNullMotor;

            //Bandolier does not work on Lemurians and new Drones due to wrong Auth Check
            IL.RoR2.SkillLocator.ApplyAmmoPack += Bandolier_WrongAuthCheck;

            if (WConfig.cfgFixWarpedOSP.Value)
            {
                IL.RoR2.HealthComponent.TakeDamageProcess += RemoveOSPEntirelyIfYouAreBelowOSPThreshold;
         
            }
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWEchoDamageNotProccingPlanulaAnymoreAC141;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWEchoDamageDoubleDippingEnemyWatches;
            if (!WolfoFixes.WolfFixes.riskyTweaks)
            {
                IL.RoR2.HealthComponent.TakeDamageProcess += FixWEchoDoubleDippingLunarRuin;
            }

            IL.RoR2.HealthComponent.TakeDamageProcess += FixParryConsumedOn0Damage0ProcAttacks;

            //Shouldn't apply them like Enforcer
            var SpawnAltar = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC4/SpawnAltar/SpawnAltar.prefab").WaitForCompletion().GetComponent<HoldoutZoneController>();
            SpawnAltar.ignoreRadiusModifiers = true;
            SpawnAltar.applyFocusConvergence = false;
        }

        private static void FixBolsteringLanternBeingPlus10(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buffs", "AttackSpeedPerNearbyAllyOrEnemyBuff"),
            x => x.MatchCall("RoR2.CharacterBody", "GetBuffCount"));
            bool b = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buffs", "AttackSpeedPerNearbyAllyOrEnemyBuff"),
            x => x.MatchCall("RoR2.CharacterBody", "GetBuffCount"));
            if (a & b)
            {
                c.EmitDelegate<Func<int, int>>((buffcount) =>
                {
                    return 0;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : FixBolsteringLanternBeingPlus10");
            }
        }

        private static void FixWEchoDoubleDippingLunarRuin(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buffs", "lunarruin"),
            x => x.MatchCallvirt("RoR2.CharacterBody", "HasBuff"));

            if (a)
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<bool, DamageInfo, bool>>((var, self) =>
                {
                    if (self.delayedDamageSecondHalf)
                    {
                        return false;
                    }
                    return var;
                });
            }
            else
            {
                //IL.RoR2.HealthComponent.TakeDamageProcess -= FixWEchoDoubleDippingLunarRuin; //This kills, the computer
                WolfFixes.log.LogError("IL Failed : FixWEchoDoubleDippingLunarRuin");
            }
        }

        private static void FixParryConsumedOn0Damage0ProcAttacks(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC3Content/Buffs", "SureProc"),
            x => x.MatchCallvirt("RoR2.CharacterBody", "HasBuff"));

            if (a)
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<bool, DamageInfo, bool>>((var, self) =>
                {
                    if (self.procCoefficient == 0 || self.damage == 0)
                    {
                        return false;
                    }
                    return var;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : FixParryConsuemdOn0Damage0ProcAttacks");
            }
        }

        private static void RemoveOSPEntirelyIfYouAreBelowOSPThreshold(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("RoR2.CharacterBody", "get_hasOneShotProtection")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, HealthComponent, bool>>((var, self) =>
                {
                    //Debug.Log(self.combinedHealthFraction);
                    if (self.combinedHealthFraction <= (1 - self.body.oneShotProtectionFraction))
                    {
                        return false;
                    }
                    return var;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : RemoveOSPEntirelyIfYouAreBelowOSPThreshold");
                return;
            }

        }

        private static void FixWEchoDamageDoubleDippingEnemyWatches(ILContext il)
        {
            //Skip all damage code for WEcho code that requires an attacker
            //Because 90% of damage increases happen there.

            ILCursor c = new ILCursor(il);

            bool a = c.TryGotoNext(MoveType.Before,
            x => x.MatchCallvirt("RoR2.CharacterBody", "get_canPerformBackstab"));

            bool b = c.TryGotoPrev(MoveType.After,
            x => x.MatchCall("UnityEngine.Object", "op_Implicit"));

            if (!a || !b)
            {
                WolfFixes.log.LogError("IL Failed : FixWEchoDamageDoubleDippingEnemyWatches Part 1");
                return;
            }
            c.Emit(OpCodes.Ldarg_1);
            c.EmitDelegate<Func<bool, DamageInfo, bool>>((var, damageInfo) =>
            {
                if (damageInfo.delayedDamageSecondHalf)
                {
                    return false;
                }
                return var;
            });
        }

        private static void FixWEchoDamageNotProccingPlanulaAnymoreAC141(ILContext il)
        {
            //Part1 : Let echo damage go into the general armor code section
            //Part2 : Make echo damage specifically only ignore armor, not all other effects

            ILCursor c = new ILCursor(il);
            bool a = false;
            a = c.TryGotoNext(MoveType.Before,
            x => x.MatchBrtrue(out _),
            x => x.MatchLdarg(0),
            x => x.MatchLdfld("RoR2.HealthComponent", "body"),
            x => x.MatchCallvirt("RoR2.CharacterBody", "get_armor"));

            if (!a)
            {
                WolfFixes.log.LogError("IL Failed : FixWEchoDamageNotProccingPlanulaAnymoreAC141 Part 1");
                return;
            }
            c.Emit(OpCodes.Ldarg_1);
            c.EmitDelegate<Func<bool, DamageInfo, bool>>((var, damageInfo) =>
            {
                if (damageInfo.delayedDamageSecondHalf)
                {
                    return false;
                }
                return var;
            });

            a = c.TryGotoNext(MoveType.Before,
            x => x.MatchMul(),
            x => x.MatchCall("UnityEngine.Mathf", "Max"),
            x => x.MatchStloc(12));

            if (!a)
            {
                WolfFixes.log.LogError("IL Failed : FixWEchoDamageNotProccingPlanulaAnymoreAC141 Part 2");
                return;
            }
            c.Emit(OpCodes.Ldarg_1);
            c.EmitDelegate<Func<float, DamageInfo, float>>((var, damageInfo) =>
            {
                if (damageInfo.delayedDamageSecondHalf)
                {
                    return 1f;
                }
                return var;
            });
        }

        private static void FixStoneFluxBeingAppliedTwice(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.Before,
            x => x.MatchLdloc(136),
            x => x.MatchLdloc(47),
            x => x.MatchConvR4(),
            x => x.MatchAdd(),
            x => x.MatchStloc(136));

            if (a && c.TryGotoNext(MoveType.After,
            x => x.MatchLdloc(136),
            x => x.MatchLdloc(47)))
            {
                c.EmitDelegate<Func<int, int>>((skill) =>
                {
                    return 0;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : FixStoneFluxBeingAppliedTwice");
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            self.hasOneShotProtection = self.hasOneShotProtection && self.oneShotProtectionFraction > 0;
        }

        private static void Bandolier_WrongAuthCheck(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("UnityEngine.Networking.NetworkIdentity", "get_hasAuthority")//This will break like instantly on update but probably fine mods right idk
           ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, SkillLocator, bool>>((has, skill) =>
                {
                    return skill.hasEffectiveAuthority;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : Bandolier_WrongAuthCheck");
            }
        }

        private static void Antler_FixNullRefOnNullMotor(On.RoR2.CharacterBody.orig_RpcRequestShardInfoClient orig, CharacterBody self)
        {
            //Lacks nullCheck for Body,
            //nullReffing on Engi Stationary Turrets
            if (self.characterMotor)
            {
                orig(self);
                return;
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(-45f, 45f), 0f);
                Vector3 point = new Vector3(self.inputBank.aimDirection.x, 0f, self.inputBank.aimDirection.z);
                Vector3 vector = rotation * point;
                Vector3 vector2 = self.gameObject.transform.position + vector * (UnityEngine.Random.Range(15f, 30f));
                NodeGraph groundNodes = SceneInfo.instance.groundNodes;
                NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(vector2, HullClassification.Human, float.PositiveInfinity);
                if (groundNodes.GetNodePosition(nodeIndex, out vector2))
                {
                    float num2 = HullDef.Find(HullClassification.Human).radius * 0.7f;
                    if (!HGPhysics.DoesOverlapSphere(vector2 + Vector3.up * (num2 + 0.25f), num2, LayerIndex.world.mask | LayerIndex.defaultLayer.mask | LayerIndex.CommonMasks.fakeActorLayers | LayerIndex.entityPrecise.mask | LayerIndex.debris.mask, QueryTriggerInteraction.UseGlobal))
                    {
                        self.CallCmdSpawnShard(vector2);
                    }
                }
            }
        }

        private static void Antler_FixWrongAuth(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCall("UnityEngine.Networking.NetworkBehaviour", "get_hasAuthority")//This will break like instantly on update but probably fine mods right idk
           ))
            {
                c.Remove(); //Surely this is fine
                c.EmitDelegate<Func<CharacterBody, bool>>((body) =>
                {
                    return body.hasEffectiveAuthority;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : Antler_FixWrongAuth");
            }
        }
        
        public static void FixChargedPerferatorCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.RoR2Content/Items", "LightningStrikeOnHit"));
            c.TryGotoNext(MoveType.After,
            x => x.MatchNewobj("RoR2.Orbs.SimpleLightningStrikeOrb"));


            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.Orbs.GenericDamageOrb", "isCrit")
            ))
            {

                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<bool, DamageInfo, bool>>((isCrit, damageInfo) =>
                {
                    return damageInfo.crit;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : FixChargedPerferatorCrit");
            }
        }

        private static void FixWarpedEchoE8(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchCall("RoR2.Run", "get_instance"),
            x => x.MatchCallvirt("RoR2.Run", "get_selectedDifficulty"),
            x => x.MatchLdcI4((int)DifficultyIndex.Eclipse8));

            if (c.TryGotoNext(MoveType.Before,
           x => x.MatchLdfld("RoR2.DamageInfo", "delayedDamageSecondHalf")
           ))
            {

                c.Remove();
                c.EmitDelegate<System.Func<DamageInfo, bool>>((damageInfo) =>
                {
                    return false;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : FixWarpedEchoE8");
            }
        }



    }

}
