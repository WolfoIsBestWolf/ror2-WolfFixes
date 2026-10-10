using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoFixes
{
    internal class ShrineHalcyonFixes
    {
        public static void Start()
        {

            //Clients need these values for Visuals
            On.RoR2.HalcyoniteShrineInteractable.Start += MoreValuesForClients;

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

        private static void MoreValuesForClients(On.RoR2.HalcyoniteShrineInteractable.orig_Start orig, HalcyoniteShrineInteractable self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                if (self.automaticallyScaleCostWithDifficulty)
                {
                    self.lowGoldCost = Run.instance.GetDifficultyScaledCost(self.lowGoldCost);
                    self.midGoldCost = Run.instance.GetDifficultyScaledCost(self.midGoldCost);
                    self.maxGoldCost = Run.instance.GetDifficultyScaledCost(self.maxGoldCost);
                    self.goldDrainValue = Run.instance.GetDifficultyScaledCost(self.goldDrainValue);
                }
            }
        }


    }

}