using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoLibrary
{
    internal class ShrineHalcyonFixes
    {
        public static void Start()
        {
            //Running Host only code on Client
            On.RoR2.CombatDirector.SpendAllCreditsOnMapSpawns_Transform += CombatDirector_SpendAllCreditsOnMapSpawns;
            On.RoR2.CombatDirector.HalcyoniteShrineActivation += CombatDirector_HalcyoniteShrineActivation;
            On.RoR2.HalcyoniteShrineInteractable.IsDraining += HalcyoniteShrineInteractable_IsDraining;

            //Clients need these values for Visuals
            On.RoR2.HalcyoniteShrineInteractable.Awake += MoreValuesForClients;
            On.RoR2.HalcyoniteShrineInteractable.Start += MoreValuesClient2;

            IL.EntityStates.ShrineHalcyonite.ShrineHalcyoniteBaseState.ModifyVisuals += FixVisualsBeingInconsistent;
        }

        //Visual checks for isDraining when it already only updates when it is draining
        //But this check adds delay every time players exit
        //So it never actually fully reaches gold status
        private static void FixVisualsBeingInconsistent(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("RoR2.HalcyoniteShrineInteractable", "isDraining")))
            {
                c.EmitDelegate<System.Func<bool, bool>>((target) =>
                {
                    return true;
                });
            }
            else
            {
                Log.LogError("IL Failed: FixVisualsBeingInconsistent");
            }
        }

        private static void HalcyoniteShrineInteractable_IsDraining(On.RoR2.HalcyoniteShrineInteractable.orig_IsDraining orig, HalcyoniteShrineInteractable self, bool drainingActive)
        {
            if (!NetworkServer.active)
            {
                //STFU
                return;
            }
            orig(self, drainingActive);
        }

        private static void CombatDirector_SpendAllCreditsOnMapSpawns(On.RoR2.CombatDirector.orig_SpendAllCreditsOnMapSpawns_Transform orig, CombatDirector self, Transform mapSpawnTarget)
        {
            if (!NetworkServer.active)
            {
                Log.LogError("CombatDirector_SpendAllCreditsOnMapSpawns | This isn't meant to run on Client, Gearbox Software");
                return;
            }
            orig(self, mapSpawnTarget);
        }

        private static void CombatDirector_HalcyoniteShrineActivation(On.RoR2.CombatDirector.orig_HalcyoniteShrineActivation orig, CombatDirector self, float monsterCredit, DirectorCard chosenDirectorCard, int difficultyLevel, Transform shrineTransform)
        {
            if (!NetworkServer.active)
            {
                Log.LogError("CombatDirector_HalcyoniteShrineActivation | This isn't meant to run on Client, Gearbox Software");
                return;
            }
            orig(self, monsterCredit, chosenDirectorCard, difficultyLevel, shrineTransform);
        }

        private static void MoreValuesForClients(On.RoR2.HalcyoniteShrineInteractable.orig_Awake orig, HalcyoniteShrineInteractable self)
        {
            orig(self);
            if (!NetworkServer.active)
            {

            }
        }
        private static void MoreValuesClient2(On.RoR2.HalcyoniteShrineInteractable.orig_Start orig, HalcyoniteShrineInteractable self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                self.lowGoldCost = Run.instance.GetDifficultyScaledCost(self.lowGoldCost);
                self.midGoldCost = Run.instance.GetDifficultyScaledCost(self.midGoldCost);
                self.maxGoldCost = Run.instance.GetDifficultyScaledCost(self.maxGoldCost);
                if (self.purchaseInteraction.Networkcost > 0)
                {
                    self.goldDrainValue = self.purchaseInteraction.Networkcost; //Personal mod use
                }
                self.goldDrainValue = Run.instance.GetDifficultyScaledCost(self.goldDrainValue);
                GoldSiphonNearbyBodyController controller = self.transform.GetChild(1).GetComponent<GoldSiphonNearbyBodyController>();
                controller.goldDrainValue = self.goldDrainValue;
            }
        }


    }

}