using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Navigation;
using System;
using UnityEngine;

namespace WolfoFixes
{

    internal class ItemFixes
    {
        public static void Start()
        {
            IL.RoR2.CharacterBody.RecalculateStats += Stew_GettingDeletedByCurseRandomly;
          
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
                IL.RoR2.HealthComponent.TakeDamageProcess += FixWarpedReducingDamageAfterOSP;
            }
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWEchoDamageNotProccingPlanulaAnymoreAC141;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWEchoDamageDoubleDippingEnemyWatches;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWEchoDoubleDippingLunarRuin;

            IL.RoR2.HealthComponent.TakeDamageProcess += FixParryConsuemdOn0Damage0ProcAttacks;
        }

        private static void Stew_GettingDeletedByCurseRandomly(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchCall("RoR2.CharacterBody", "get_healthComponent"),
            x => x.MatchLdfld("RoR2.HealthComponent", "health"),
            x => x.MatchLdarg(0),
            x => x.MatchCall("RoR2.CharacterBody", "get_maxHealth"),
            x => x.MatchLdarg(0),
            x => x.MatchCall("RoR2.CharacterBody", "get_cursePenalty"),
            x => x.MatchDiv());

            if (a)
            {
                //c.RemoveRange(5);
                c.EmitDelegate<Func<float, float>>((body) =>
                {            
                    return Mathf.Floor(body);
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : STEW getting fucked up by eclipse curse");
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
                WolfFixes.log.LogWarning("IL Failed : FixWEchoDoubleDippingLunarRuin");
            }
        }

        private static void FixParryConsuemdOn0Damage0ProcAttacks(ILContext il)
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
                WolfFixes.log.LogWarning("IL Failed : FixParryConsuemdOn0Damage0ProcAttacks");
            }
        }

        private static void FixWarpedReducingDamageAfterOSP(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Items", "DelayedDamage"));

            if (a & c.TryGotoNext(MoveType.After,
            x => x.MatchLdcR4(0.9f)))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float, HealthComponent, float>>((var, self) =>
                {
                    if (self.ospTimer > 0f)
                    {
                        return 1;
                    }
                    return var;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : FixWarpedReducingDamageAfterOSP");
            }
        }

        private static void RemoveOSPEntirelyIfYouAreBelowOSPThreshold(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if(c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("RoR2.CharacterBody", "get_hasOneShotProtection")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, HealthComponent, bool>>((var, self) =>
                {
                    //Debug.Log(self.combinedHealthFraction);
                    if (self.combinedHealthFraction < (1 - self.body.oneShotProtectionFraction))
                    {
                        return false;
                    }
                    return var;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : RemoveOSPEntirelyIfYouAreBelowOSPThreshold");
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
                WolfFixes.log.LogWarning("IL Failed : FixWEchoDamageDoubleDippingEnemyWatches Part 1");
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
            ILCursor c = new ILCursor(il);
            bool a = false;
            a = c.TryGotoNext(MoveType.Before,
            x => x.MatchBrtrue(out _),
            x => x.MatchLdarg(0),
            x => x.MatchLdfld("RoR2.HealthComponent", "body"),
            x => x.MatchCallvirt("RoR2.CharacterBody", "get_armor"));
 
            if (!a)
            {
                WolfFixes.log.LogWarning("IL Failed : FixWEchoDamageNotProccingPlanulaAnymoreAC141 Part 1");
                return;
            }
            c.Emit(OpCodes.Ldarg_1);
            c.EmitDelegate<Func<bool,DamageInfo, bool>>((var, damageInfo) =>
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
            x => x.MatchStloc(10));
 
            if (!a)
            {
                WolfFixes.log.LogWarning("IL Failed : FixWEchoDamageNotProccingPlanulaAnymoreAC141 Part 2");
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
            x => x.MatchLdloc(112),
            x => x.MatchLdloc(46),
            x => x.MatchConvR4(),
            x => x.MatchAdd(),
            x => x.MatchStloc(112));

            if (a && c.TryGotoNext(MoveType.After,
            x => x.MatchLdloc(112),
            x => x.MatchLdloc(46)))
            {
                //c.RemoveRange(5);
                c.EmitDelegate<Func<int, int>>((skill) =>
                {
                    return 0;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : FixStoneFluxBeingAppliedTwice");
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            self.hasOneShotProtection = self.hasOneShotProtection && self.oneShotProtectionFraction > 0;
        }

        //Fix 1.4.1
        /*private static void FixVoidsentNoLongerChaining(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC1Content/Items", "ExplodeOnDeathVoid"));

            if (a && c.TryGotoNext(MoveType.Before,
            x => x.MatchLdflda("RoR2.DelayBlast", "procChainMask")))
            {
                c.RemoveRange(3);
                c.TryGotoPrev(MoveType.Before,
                x => x.MatchDup());
                c.Remove();
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : FixVoidsentNoLongerChaining");
            }
        }*/

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
                WolfFixes.log.LogWarning("IL Failed : Bandolier_WrongAuthCheck");
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
                WolfFixes.log.LogWarning("IL Failed : Antler_FixWrongAuth");
            }
        }

        private static void WEchoFirstHitIntoDanger(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            //Pre for better compat?
            if (damageReport.damageInfo.firstHitOfDelayedDamageSecondHalf)
            {
                self.outOfDangerStopwatch = 0;
            }
            orig(self, damageReport);
        }

        private static void FixWarpedEchoNotUsingArmor(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            //WEchos first hit is rejected if OSP Happens
            //Because it happens during the OSPTimer that rejects ALL Damage
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("RoR2.HealthComponent", "ospTimer")))
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<float, DamageInfo, float>>((timer, damageInfo) =>
                {
                    if (damageInfo.delayedDamageSecondHalf)
                    {
                        return 0;
                    }
                    return timer;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : WECHO dont delete first hit if OSPed");
            }

            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Items", "DelayedDamage"));

            if (c.TryGotoNext(MoveType.After,
           //x => x.MatchLdloc(8) //This will break like instantly on update but probably fine mods right idk
           x => x.MatchLdloc(10) //This will break like instantly on update but probably fine mods right idk
           ))
            {
                //c.Next.Operand = 7; //Why does this not work?
                //Testing
                //c.Emit(OpCodes.Ldloc, 7); 
                c.Emit(OpCodes.Ldloc, 9);
                c.EmitDelegate<Func<float, float, float>>((aaa, bbb) =>
                {
                    /*WolfoMain.Logger.LogMessage(aaa);
                    WolfoMain.Logger.LogMessage(bbb);
                    WolfoMain.Logger.LogMessage(bbb*0.8f);*/
                    return bbb;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : FixWarpedEchoNotUsingArmor");
            }

            //After 80%
            //if (c.TryGotoNext(MoveType.Before, x => x.MatchStloc(57)))
            if (c.TryGotoNext(MoveType.After,
              //x => x.MatchStloc(50) //This will break like instantly on update but probably fine with other mods right idk
              x => x.MatchLdcR4(0.8f),
              x => x.MatchMul()
              //x => x.MatchStloc(57) //This will break like instantly on update but probably fine with other mods right idk
              ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<float, HealthComponent, DamageInfo, float>>((damage, self, damageInfo) =>
                {
                    //I do not like copying it like this
                    //But I do not trust myself to do it in a more "official way" with breaks or moving pointers
                    //WolfoMain.Logger.LogMessage("Pre" + damage);
                    if (self.body.hasOneShotProtection && (damageInfo.damageType & DamageType.BypassOneShotProtection) != DamageType.BypassOneShotProtection)
                    {
                        float maxDamageOSP = (self.fullCombinedHealth + self.barrier) * (1f - self.body.oneShotProtectionFraction);
                        float b = Mathf.Max(0f, maxDamageOSP - self.serverDamageTakenThisUpdate); //What is this?
                        float huh = damage;
                        damage = Mathf.Min(damage, b); //This is what OSP does
                        if (damage != huh) //If you already took exactly 90% damage, dont do OSP (??)
                        {
                            WolfFixes.log.LogMessage("Trigger Warped OSP");
                            self.TriggerOneShotProtection();
                        }
                    }
                    //WolfoMain.Logger.LogMessage("Post" + damage);

                    return damage;
                });

            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : WARPED OSP FIX");
            }


        }

        private static void FixEchoOSP(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            int beforeEcho = -1;
            int beforeOSP = -1;
            int afterOSP = -1;

            ILCursor cbeforeEcho;
            ILCursor cbeforeOSP;
            ILCursor cafterOSP;



            //Goto X
            //Y
            //Echo Code
            //Goto Z
            //X
            //OSP
            //Goto Y
            //Z

            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buff", "DelayedDamageBuff"));
            c.TryGotoNext(MoveType.After,
            x => x.MatchNewobj("RoR2.Orbs.SimpleLightningStrikeOrb"));


            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.Orbs.GenericDamageOrb", "isCrit")
            ))
            {

                cbeforeEcho = c.Emit(OpCodes.Break);


            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : FixChargedPerferatorCrit");
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
                WolfFixes.log.LogWarning("IL Failed : FixChargedPerferatorCrit");
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
                WolfFixes.log.LogWarning("IL Failed : FixWarpedEchoE8");
            }
        }



    }

}
