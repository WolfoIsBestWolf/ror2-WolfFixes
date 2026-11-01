using BepInEx.Logging;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoLibrary
{
    internal static class FixesAlways
    {
        public static void Start()
        {

            //Clients need these values for Visuals
            On.RoR2.HalcyoniteShrineInteractable.Start += MoreValuesForClients;

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