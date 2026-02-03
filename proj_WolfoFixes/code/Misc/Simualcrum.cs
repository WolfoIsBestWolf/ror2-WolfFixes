//using System;
using MonoMod.Cil;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoLibrary;

namespace WolfoFixes
{

    internal class Simualcrum
    {
        public static void Start()
        {
            IL.RoR2.InfiniteTowerWaveController.FixedUpdate += FixRequestIndicatorsClient;
            On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.OnEnter += Waiting_SetRadius;
            On.RoR2.InfiniteTowerRun.OverrideRuleChoices += ForceSotVOn;

            //Simulacrums Fog was not update to account for the the newly introduced FogDamageController.healthFractionRampIncreaseCooldown
            //This left it dealing pitiful amounts of damage
            FogDamageController InfiniteTowerFogDamager = Addressables.LoadAssetAsync<GameObject>(key: "9c7ca1b454882464f90010d3a68b6795").WaitForCompletion().GetComponent<FogDamageController>();
            InfiniteTowerFogDamager.healthFractionRampIncreaseCooldown = 0;

            //Technicallity
            SceneList.itGolemPlains.requiredExpansion = DLCS.DLC1;
            SceneList.itGoolake.requiredExpansion = DLCS.DLC1;
            SceneList.itAncientLoft.requiredExpansion = DLCS.DLC1;
            SceneList.itFrozenwall.requiredExpansion = DLCS.DLC1;
            SceneList.itDampCave.requiredExpansion = DLCS.DLC1;
            SceneList.itSkyMeadow.requiredExpansion = DLCS.DLC1;
            SceneList.itMoon.requiredExpansion = DLCS.DLC1;

            On.RoR2.InfiniteTowerWaveController.OnDisable += DisableIndicatorHelper;
        }

        private static void DisableIndicatorHelper(On.RoR2.InfiniteTowerWaveController.orig_OnDisable orig, InfiniteTowerWaveController self)
        {
            orig(self);
            simu_enabledIndicators = false;
            TeamComponent.onJoinTeamGlobal -= FixIndicatorsGettingLostAfterRevive;
        }

        private static GameObject simu_indicator;
        private static bool simu_enabledIndicators = false;
        private static void FixIndicatorsGettingLostAfterRevive(TeamComponent arg1, TeamIndex arg2)
        {
            //If a enemy respawned with Dios they lose the indicator pointing to them.
            //Probably helps Client-Side stuff too
            if (simu_enabledIndicators && arg1.indicator == null)
            {
                arg1.RequestDefaultIndicator(simu_indicator);
            }
        }
 
        public static void CallLate()
        {
            bool simulacrumAdds = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Wolfo.SimulacrumAdditions");

            if (simulacrumAdds == false)
            {
                //Simulacrum normally has no Shrines of Chance
                InfiniteTowerRun infiniteTowerRun = Addressables.LoadAssetAsync<GameObject>(key: "ba84d17b27db8b84d925071b4af1e352").WaitForCompletion().GetComponent<InfiniteTowerRun>();
                HG.ArrayUtils.ArrayAppend(ref infiniteTowerRun.blacklistedItems, DLC2Content.Items.ExtraShrineItem);

            }

        }

        private static void ForceSotVOn(On.RoR2.InfiniteTowerRun.orig_OverrideRuleChoices orig, InfiniteTowerRun self, RuleChoiceMask mustInclude, RuleChoiceMask mustExclude, ulong runSeed)
        {
            //Technicallity but does break Augment of Void reward if SotV isnt on.
            orig(self, mustInclude, mustExclude, runSeed);
            RuleDef ruleDef = RuleCatalog.FindRuleDef("Expansions.DLC1");
            RuleChoiceDef ruleChoiceDef = (ruleDef != null) ? ruleDef.FindChoice("On") : null;
            if (ruleChoiceDef != null)
            {
                self.ForceChoice(mustInclude, mustExclude, ruleChoiceDef);
            }
        }



        public static void Waiting_SetRadius(On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.orig_OnEnter orig, EntityStates.InfiniteTowerSafeWard.AwaitingActivation self)
        {
            orig(self);

            //Client fix
            //Fix SafeWardController being unset for Clients
            //This is needed for Teleport location
            //Without it, clients teleport to the wrong location.
            InfiniteTowerRun run = Run.instance.GetComponent<InfiniteTowerRun>();
            if (!run.safeWardController)
            {
                run.safeWardController = self.gameObject.GetComponent<InfiniteTowerSafeWardController>();
            }
        }


        public static void FixRequestIndicatorsClient(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.InfiniteTowerWaveController", "combatSquad"),
            x => x.MatchCallvirt("RoR2.CombatSquad", "get_readOnlyMembersList") 
            ))
            {
                
                c.EmitDelegate<System.Func<InfiniteTowerWaveController, InfiniteTowerWaveController>>((System.Func<InfiniteTowerWaveController, InfiniteTowerWaveController>)((wave) =>
                {
                    TeamComponent.onJoinTeamGlobal += FixIndicatorsGettingLostAfterRevive;
                    simu_enabledIndicators = true;
                    simu_indicator = wave.defaultEnemyIndicatorPrefab;
                    if (wave.combatSquad.readOnlyMembersList.Count == 0)
                    {
                        WolfFixes.log.LogMessage("Couln't do indicators the normal way");
                        for (int i = 0; wave.combatSquad.membersList.Count > i; i++)
                        {
                            wave.RequestIndicatorForMaster(wave.combatSquad.membersList[i]);
                        }
                    }
                    return wave;
                }));
                //WolfFixes.Logger.LogMessage("IL Found : IL.RoR2.InfiniteTowerWaveController.FixedUpdate");
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : IL.RoR2.InfiniteTowerWaveController.FixedUpdate");
            }
        }

     
    }


}
