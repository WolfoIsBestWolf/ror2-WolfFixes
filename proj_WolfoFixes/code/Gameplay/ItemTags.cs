using HG;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace WolfoFixes
{

    internal class ItemTags
    {

        public static ItemTag NoEvolSimu = (ItemTag)94; //Enemy No, Scav Yes, Brother Yes?
        public static ItemTag NoMonsterScav = (ItemTag)95; //Enemy Yes, Scav No, Brother Yes?

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
                NoMonsterScav
            };
            //No Equipment
            //No Onkill
            //Evo blacklist
            ItemTag[] TagsMobsEvo = {
                ItemTag.AIBlacklist,
                ItemTag.OnKillEffect,
                ItemTag.EquipmentRelated,
                ItemTag.CannotCopy,
                NoEvolSimu,
                NoMonsterScav,
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
                NoMonsterScav,
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

        public static void CallLate()
        {
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.CloverVoid.tags, ItemTag.DevotionBlacklist);
            //HG.ArrayUtils.ArrayAppend(ref DLC2Content.Items.SpeedBoostPickup.tags, ItemTag.DevotionBlacklist);



            #region Tag Fixes

            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.FlatHealth.tags, 1, 1); //Remove OnkillTag

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BonusGoldPackOnKill.tags, ItemTag.AIBlacklist); //Enemies cannot use Gold
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoveSpeedOnKill.tags, ItemTag.OnKillEffect); //Missed Tag
           
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.MonstersOnShrineUse.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.GoldOnHit.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.LunarTrinket.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.FocusConvergence.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC2Content.Items.OnLevelUpFreeUnlock.tags, ItemTag.AIBlacklist);

            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MushroomVoid.tags, ItemTag.SprintRelated); //Missed Tag
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MushroomVoid.tags, ItemTag.AIBlacklist); //Sprint is blacklisted


            RoR2Content.Items.ParentEgg.tags[0] = ItemTag.Healing;

            #endregion



            if (!WConfig.cfgItemTags.Value)
            {
                return;
            }

            #region AI Blacklist
            #region White
 
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.FlatHealth.tags, NoEvolSimu); //Useless,

            #endregion
            #region Green
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.Infusion.tags, ItemTag.AIBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.PrimarySkillShuriken.tags, NoEvolSimu); //Borderline for runs, Keeping in Vfields

            #endregion
            #region Red
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.NovaOnHeal.tags, ItemTag.AIBlacklist); //Overpowered

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BarrierOnOverHeal.tags, NoEvolSimu); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoreMissile.tags, NoEvolSimu); //Useless
            
            //ArrayUtils.ArrayAppend(ref RoR2Content.Items.CaptainDefenseMatrix.tags, ItemTag.CannotSteal); //Passive shouldnt be stealable ig?
            #endregion
            #region Boss
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.TitanGoldDuringTP.tags, ItemTag.AIBlacklist); //Cant use
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.SprintWisp.tags, ItemTag.AIBlacklist); //Cant use
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MinorConstructOnKill.tags, ItemTag.AIBlacklist);  //Cant use
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.SiphonOnLowHealth.tags, ItemTag.AIBlacklist); //Already on BrotherBlacklist
 
            //Mithrix would also be affected by these tags so keep that in mind
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.RoboBallBuddy.tags, NoMonsterScav); //Op Confusing

            #endregion
            #region Lunar
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.LunarSun.tags, NoEvolSimu); //Op
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.LunarSun.tags, NoMonsterScav); //Op
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.RandomlyLunar.tags, ItemTag.AIBlacklist);

            #endregion
            #region Void
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, NoEvolSimu); //Unfun
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ExplodeOnDeathVoid.tags, NoEvolSimu); //Op
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ExplodeOnDeathVoid.tags, NoMonsterScav); //Op

            #endregion


            #region Modded
            ItemDef tempDef = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("WatchMetronome"));
            if (tempDef != null)
            {
                ArrayUtils.ArrayAppend(ref tempDef.tags, ItemTag.AIBlacklist);
            }
            tempDef = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("VV_ITEM_EHANCE_VIALS_ITEM"));
            if (tempDef != null)
            {
                ArrayUtils.ArrayAppend(ref tempDef.tags, ItemTag.AIBlacklist);
            }
            #endregion
            #endregion

            #region Category stuff
            //General rule changes
            //Barrier -> Healing : Instead of Healing+Utility
            //Max HP -> Healing : Instead of Healing+Utility

            //Some Max HP is already only tagged as Healing
            //How Barrier is Utility? Like it's not DR 
            //Healing is a small pool it shouldnt overlap with much other stuff

            #region White
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.BarrierOnKill.tags, 0); //Remove Utility
            #endregion
            #region Green
            //Green
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.Infusion.tags, 0); //Remove Utility
            ArrayUtils.ArrayAppend(ref DLC2Content.Items.TeleportOnLowHealth.tags, ItemTag.Healing); //Barrier is Healing.
            #endregion
            #region Red
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.HeadHunter.tags, 0); //Remove Utility
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.BarrierOnOverHeal.tags, 0); //Remove Utility, Barrier is Healing.

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.NovaOnHeal.tags, ItemTag.Healing);
            //ArrayUtils.ArrayAppend(ref DLC1Content.Items.ImmuneToDebuff.tags, ItemTag.Healing); //Already is Healing

            #endregion
            #region Boss
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.Knurl.tags, 0); //Remove Utility

            #endregion
            #region Lunar

            RoR2Content.Items.RandomDamageZone.tags[0] = ItemTag.Damage; //Utility???
            DLC1Content.Items.HalfSpeedDoubleHealth.tags[0] = ItemTag.Healing; //Utility???
            DLC1Content.Items.LunarSun.tags[0] = ItemTag.Damage; //Utility???
            RoR2Content.Items.LunarUtilityReplacement.tags[0] = ItemTag.Healing; //This Heals you
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.ShieldOnly.tags, ItemTag.Healing); //Max HP
            #endregion

            #region Void
            //ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, ItemTag.Utility); //Uh?

            #endregion
  
            #endregion

        }



    }


}
