using HG;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoLibrary
{
    public static class Tags
    {
        public static void AddTag(ItemDef def, ItemTag tag)
        {
            ArrayUtils.ArrayAppend(ref def.tags, tag);
        }

        public static void RemoveTag(ItemDef def, ItemTag tag)
        {
            int index = Array.IndexOf<ItemTag>(def.tags, tag);
            if (index != -1)
            {
                HG.ArrayUtils.ArrayRemoveAtAndResize(ref def.tags, index);
            }
            //def.tags = def.tags.Remove(tag);
        }
        public static void RemoveTag(ItemDef def, int index)
        {
            HG.ArrayUtils.ArrayRemoveAtAndResize(ref def.tags, index);
        }
        public static ItemTag EvolutionBlacklist = (ItemTag)94; //Blacklisted from Evolution
        public static ItemTag ScavengerBlacklist = (ItemTag)95; //Scav specific blacklist.

        public static BasicPickupDropTable dtMonsterTeamLunar;
        public static BasicPickupDropTable dtAISafeBoss;

        public static void Start()
        {
            //Using more tags because most of these tags are meant to be AIBlacklisted anyways.
            //Ie all Sprint -> AiBlacklist but not always tagged as such

            BasicPickupDropTable dtMonsterTeamTier1Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier1Item.asset").WaitForCompletion();
            BasicPickupDropTable dtMonsterTeamTier2Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier2Item.asset").WaitForCompletion();
            BasicPickupDropTable dtMonsterTeamTier3Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier3Item.asset").WaitForCompletion();
            dtMonsterTeamLunar = ScriptableObject.CreateInstance<BasicPickupDropTable>();
            dtMonsterTeamLunar.name = "dtMonsterTeamLunar";
            dtMonsterTeamLunar.tier1Weight = 0;
            dtMonsterTeamLunar.tier2Weight = 0;
            dtMonsterTeamLunar.tier3Weight = 0;
            dtMonsterTeamLunar.lunarItemWeight = 1;

            ArenaMonsterItemDropTable dtArenaMonsterTier1 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier1.asset").WaitForCompletion();
            ArenaMonsterItemDropTable dtArenaMonsterTier2 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier2.asset").WaitForCompletion();
            ArenaMonsterItemDropTable dtArenaMonsterTier3 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier3.asset").WaitForCompletion();

            BasicPickupDropTable dtAISafeTier1Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier1Item.asset").WaitForCompletion();
            BasicPickupDropTable dtAISafeTier2Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier2Item.asset").WaitForCompletion();
            BasicPickupDropTable dtAISafeTier3Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier3Item.asset").WaitForCompletion();
            dtAISafeBoss = ScriptableObject.CreateInstance<BasicPickupDropTable>();
            dtAISafeBoss.name = "dtAISafeBoss";
            dtAISafeBoss.tier1Weight = 0;
            dtAISafeBoss.tier2Weight = 0;
            dtAISafeBoss.tier3Weight = 0;
            dtAISafeBoss.bossWeight = 1;

            ItemTag[] TagsScav = {
                ItemTag.AIBlacklist,
                ItemTag.SprintRelated,
                ItemTag.InteractableRelated,
                ItemTag.OnStageBeginEffect,
                ItemTag.HoldoutZoneRelated,
                ScavengerBlacklist
            };
            //No Equipment
            //No Onkill
            //Evo blacklist
            ItemTag[] TagsMobsEvo = {
                ItemTag.AIBlacklist,
                ItemTag.OnKillEffect,
                ItemTag.EquipmentRelated,
                ItemTag.CannotCopy,
                EvolutionBlacklist,
                ScavengerBlacklist,
                ItemTag.SprintRelated,
                ItemTag.InteractableRelated,
                ItemTag.OnStageBeginEffect,
                ItemTag.HoldoutZoneRelated
            };
            ItemTag[] Arena = {
                ItemTag.AIBlacklist,
                ItemTag.OnKillEffect,
                ItemTag.EquipmentRelated,
                ItemTag.CannotCopy,
                ScavengerBlacklist,
                ItemTag.SprintRelated,
                ItemTag.InteractableRelated,
                ItemTag.OnStageBeginEffect,
                ItemTag.HoldoutZoneRelated
            };
            dtMonsterTeamTier1Item.bannedItemTags = TagsMobsEvo;
            dtMonsterTeamTier2Item.bannedItemTags = TagsMobsEvo;
            dtMonsterTeamTier3Item.bannedItemTags = TagsMobsEvo;
            dtMonsterTeamLunar.bannedItemTags = TagsMobsEvo;

            dtArenaMonsterTier1.bannedItemTags = Arena;
            dtArenaMonsterTier2.bannedItemTags = Arena;
            dtArenaMonsterTier3.bannedItemTags = Arena;

            dtAISafeTier1Item.bannedItemTags = TagsScav;
            dtAISafeTier2Item.bannedItemTags = TagsScav;
            dtAISafeTier3Item.bannedItemTags = TagsScav;
            dtAISafeBoss.bannedItemTags = TagsScav;


        }

    }

}
