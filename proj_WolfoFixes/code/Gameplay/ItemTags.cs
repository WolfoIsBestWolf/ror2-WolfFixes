using HG;
using RoR2;
using WolfoLibrary;
using static WolfoLibrary.Tags;

namespace WolfoFixes
{

    internal class ItemTags
    {


        public static void CallLate()
        {
            //DLC3 weird tag stuff
            //Randomly got rebirth blacklisted;
            RemoveTag(RoR2Content.Items.Bear, ItemTag.RebirthBlacklist);
            RemoveTag(DLC1Content.Items.BearVoid, ItemTag.RebirthBlacklist);
            RemoveTag(DLC1Content.Items.RandomlyLunar, ItemTag.RebirthBlacklist);
            RemoveTag(RoR2Content.Items.SiphonOnLowHealth, ItemTag.RebirthBlacklist);
            RemoveTag(DLC3Content.Items.TransferDebuffOnHit, ItemTag.RebirthBlacklist);

            //Better DroneBuffArrow stuff
            AddTag(RoR2Content.Items.BoostHp, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.BoostDamage, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.BoostAttackSpeed, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.HealthDecay, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.Ghost, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.TeamSizeDamageBonus, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.DrizzlePlayerHelper, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.MonsoonPlayerHelper, ItemTag.HiddenForDroneBuffIcon);
            AddTag(DLC1Content.Items.GummyCloneIdentifier, ItemTag.HiddenForDroneBuffIcon);


            ArrayUtils.ArrayAppend(ref DLC1Content.Items.CloverVoid.tags, ItemTag.DevotionBlacklist);



            #region Tag Fixes

            RemoveTag(RoR2Content.Items.FlatHealth, ItemTag.OnKillEffect); //Remove OnkillTag

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

            //AIBlacklisted because EquipRelated for Arena
            //But those drop tables filter those out nowadays
            //And Scavs can use these just fine
            RemoveTag(RoR2Content.Items.EquipmentMagazine, ItemTag.AIBlacklist);
            RemoveTag(RoR2Content.Items.EnergizedOnEquipmentUse, ItemTag.AIBlacklist);
            RemoveTag(RoR2Content.Items.Talisman, ItemTag.AIBlacklist);


            #endregion

            if (!WConfig.cfgItemTags.Value)
            {
                return;
            }

            #region AI Blacklist
            #region White

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.FlatHealth.tags, Tags.EvolutionBlacklist); //Useless,

            #endregion
            #region Green
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.Infusion.tags, ItemTag.AIBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.PrimarySkillShuriken.tags, Tags.EvolutionBlacklist); //Borderline for runs, Keeping in Vfields
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.RegeneratingScrap.tags, ItemTag.AIBlacklist); //Borderline for runs, Keeping in Vfields

            ArrayUtils.ArrayAppend(ref DLC3Content.Items.ShieldBooster.tags, EvolutionBlacklist); //Borderline for runs, Keeping in Vfields
            #endregion
            #region Red
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.NovaOnHeal.tags, ItemTag.AIBlacklist); //Overpowered

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BarrierOnOverHeal.tags, EvolutionBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoreMissile.tags, EvolutionBlacklist); //Useless

            //ArrayUtils.ArrayAppend(ref RoR2Content.Items.CaptainDefenseMatrix.tags, ItemTag.CannotSteal); //Passive shouldnt be stealable ig?
            #endregion
            #region Boss
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.TitanGoldDuringTP.tags, ItemTag.AIBlacklist); //Cant use
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.SprintWisp.tags, ItemTag.AIBlacklist); //Cant use
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MinorConstructOnKill.tags, ItemTag.AIBlacklist);  //Cant use
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.SiphonOnLowHealth.tags, ItemTag.AIBlacklist); //Already on BrotherBlacklist

            //Mithrix would also be affected by these tags so keep that in mind
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.RoboBallBuddy.tags, Tags.ScavengerBlacklist); //Op Confusing

            #endregion
            #region Lunar
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.LunarSun.tags, ItemTag.AIBlacklist); //Too hard against Melee
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.RandomlyLunar.tags, ItemTag.AIBlacklist);

            #endregion
            #region Void
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, EvolutionBlacklist); //Unfun
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ExplodeOnDeathVoid.tags, EvolutionBlacklist); //Op
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ExplodeOnDeathVoid.tags, ScavengerBlacklist); //Op

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
