using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoLibrary
{
    public class AdvancedPickupDropTable : BasicPickupDropTable
    {
        //Includes Aspects
        //Only generates when frist drop
        private bool generated;
        public bool clearAfterGenerating;
        public bool unbiasedByItemCount = false;

        public override UniquePickup GeneratePickupPreReplacement(Xoroshiro128Plus rng)
        {
            RegenerateReal();
            return PickupDropTable.GeneratePickupFromWeightedSelection(rng, this.selector);
        }
        public override void GenerateDistinctPickupsPreReplacement(List<UniquePickup> dest, int desiredCount, Xoroshiro128Plus rng)
        {
            RegenerateReal();
            PickupDropTable.GenerateDistinctFromWeightedSelection<UniquePickup>(dest, desiredCount, rng, this.selector);
        }

        public override void Regenerate(Run run)
        {
            generated = false;
        }
        public void RegenerateReal()
        {
            if (generated)
            {
                return;
            }
            generated = true;
            if (unbiasedByItemCount)
            {
                GenerateUnbiased(Run.instance);
            }
            else
            {
                RegenerateDropTable(Run.instance);
            }
            AddEliteEquip(Run.instance);
        }

        private void GenerateUnbiased(Run run)
        {
            this.selector.Clear();
            this.AddUnbiased(run.availableTier1DropList, this.tier1Weight);
            this.AddUnbiased(run.availableTier2DropList, this.tier2Weight);
            this.AddUnbiased(run.availableTier3DropList, this.tier3Weight);
            this.AddUnbiased(run.availableBossDropList, this.bossWeight);
            this.AddUnbiased(run.availableLunarItemDropList, this.lunarItemWeight);
            this.AddUnbiased(run.availableLunarEquipmentDropList, this.lunarEquipmentWeight);
            this.AddUnbiased(run.availableLunarCombinedDropList, this.lunarCombinedWeight);
            this.AddUnbiased(run.availableEquipmentDropList, this.equipmentWeight);
            this.AddUnbiased(run.availableVoidTier1DropList, this.voidTier1Weight);
            this.AddUnbiased(run.availableVoidTier2DropList, this.voidTier2Weight);
            this.AddUnbiased(run.availableVoidTier3DropList, this.voidTier3Weight);
            this.AddUnbiased(run.availableVoidBossDropList, this.voidBossWeight);
            this.AddUnbiased(run.availableFoodTierDropList, this.foodTierWeight);
        }
        private void AddUnbiased(List<PickupIndex> sourceDropList, float chance)
        {
            if (chance <= 0f || sourceDropList.Count == 0)
            {
                return;
            }
            float chanceDiv = chance / sourceDropList.Count;
            foreach (PickupIndex pickupIndex in sourceDropList)
            {
                if (!this.IsFilterRequired() || this.PassesFilter(pickupIndex))
                {
                    this.selector.AddChoice(new UniquePickup
                    {
                        pickupIndex = pickupIndex
                    }, chance);
                }
            }
        }

        public float eliteEquipmentWeight;
        private void AddEliteEquip(Run run)
        {
            if (eliteEquipmentWeight == 0)
            {
                return;
            }
            var list = ModUtil.GetEliteEquipment();


            float dropChance = (unbiasedByItemCount ? eliteEquipmentWeight / (float)list.Count : eliteEquipmentWeight);
            foreach (EquipmentIndex equipmentIndex in list)
            {
                this.selector.AddChoice(new UniquePickup
                {
                    pickupIndex = PickupCatalog.FindPickupIndex(equipmentIndex)
                }, dropChance);
            }
        }

    }


    public class ForcedTeamCSC : CharacterSpawnCard
    {
        public TeamIndex teamIndexOverride = TeamIndex.Monster;
        public override void Spawn(Vector3 position, Quaternion rotation, DirectorSpawnRequest directorSpawnRequest, ref SpawnCard.SpawnResult result)
        {
            directorSpawnRequest.teamIndexOverride = teamIndexOverride;
            base.Spawn(position, rotation, directorSpawnRequest, ref result);
        }
    }

   

}
