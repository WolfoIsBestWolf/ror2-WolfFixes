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
            RemoveTag(DLC1Content.Items.BearVoid, ItemTag.RebirthBlacklist);
            RemoveTag(DLC1Content.Items.RandomlyLunar, ItemTag.RebirthBlacklist);
            RemoveTag(DLC3Content.Items.TransferDebuffOnHit, ItemTag.RebirthBlacklist);

            //Better DroneBuffArrow stuff
            AddTag(RoR2Content.Items.BoostHp, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.BoostDamage, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.BoostAttackSpeed, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.HealthDecay, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.Ghost, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.TeamSizeDamageBonus, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.CutHp, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.DrizzlePlayerHelper, ItemTag.HiddenForDroneBuffIcon);
            AddTag(RoR2Content.Items.MonsoonPlayerHelper, ItemTag.HiddenForDroneBuffIcon);
            AddTag(DLC1Content.Items.GummyCloneIdentifier, ItemTag.HiddenForDroneBuffIcon);

            AddTag(DLC1Content.Items.CloverVoid, ItemTag.DevotionBlacklist);



            #region Tag Fixes

            RemoveTag(RoR2Content.Items.FlatHealth, ItemTag.OnKillEffect); //Remove OnkillTag
            AddTag(DLC1Content.Items.MoveSpeedOnKill, ItemTag.OnKillEffect); //Missed Tag
            AddTag(DLC1Content.Items.MushroomVoid, ItemTag.SprintRelated); //Missed Tag
            RoR2Content.Items.ParentEgg.tags[0] = ItemTag.Healing;

            AddTag(RoR2Content.Items.BonusGoldPackOnKill, ItemTag.AIBlacklist); //Enemies cannot use Gold

            AddTag(RoR2Content.Items.MonstersOnShrineUse, ItemTag.AIBlacklist);
            AddTag(RoR2Content.Items.GoldOnHit, ItemTag.AIBlacklist);
            AddTag(RoR2Content.Items.LunarTrinket, ItemTag.AIBlacklist);
            AddTag(RoR2Content.Items.FocusConvergence, ItemTag.AIBlacklist);
            AddTag(DLC2Content.Items.OnLevelUpFreeUnlock, ItemTag.AIBlacklist);
 
            #endregion

            if (!WConfig.cfgItemTags.Value)
            {
                return;
            }
            //AIBlacklisted because EquipRelated for Arena
            //But those drop tables filter those out nowadays
            //And Scavs can use these just fine
            RemoveTag(RoR2Content.Items.EquipmentMagazine, ItemTag.AIBlacklist);
            RemoveTag(RoR2Content.Items.EnergizedOnEquipmentUse, ItemTag.AIBlacklist);
            RemoveTag(RoR2Content.Items.Talisman, ItemTag.AIBlacklist);

            #region AI Blacklist
            #region White, Green, Red

            AddTag(RoR2Content.Items.FlatHealth, EvolutionBlacklist); //Useless,

            AddTag(RoR2Content.Items.Infusion, ItemTag.AIBlacklist); //Useless
            AddTag(DLC1Content.Items.PrimarySkillShuriken, EvolutionBlacklist); //Borderline for runs, Keeping in Vfields

            AddTag(DLC3Content.Items.ShieldBooster, ItemTag.AIBlacklist); //Damage reflection items aren't for enemies
            #endregion
            #region Red
            AddTag(RoR2Content.Items.NovaOnHeal, ItemTag.AIBlacklist); //Overpowered

            AddTag(RoR2Content.Items.BarrierOnOverHeal, EvolutionBlacklist); //Useless
            AddTag(DLC1Content.Items.MoreMissile, EvolutionBlacklist);       //Useless
            #endregion
            #region Boss
            AddTag(RoR2Content.Items.TitanGoldDuringTP, ItemTag.AIBlacklist); //Cant use
            AddTag(RoR2Content.Items.SprintWisp, ItemTag.AIBlacklist); //Cant use, or OP
            AddTag(DLC1Content.Items.MinorConstructOnKill, ItemTag.AIBlacklist);  //Cant use
            AddTag(RoR2Content.Items.SiphonOnLowHealth, ItemTag.AIBlacklist); //Already on BrotherBlacklist

            //Mithrix would also be affected by these tags so keep that in mind
            AddTag(RoR2Content.Items.RoboBallBuddy, Tags.ScavengerBlacklist); //Op Confusing
            AddTag(RoR2Content.Items.RoboBallBuddy, Tags.EvolutionBlacklist); //Op Confusing

            #endregion
            #region Lunar
            AddTag(DLC1Content.Items.LunarSun, ItemTag.AIBlacklist); //Too hard against Melee
            AddTag(DLC1Content.Items.RandomlyLunar, ItemTag.AIBlacklist);

            #endregion
            #region Void
            AddTag(DLC1Content.Items.ElementalRingVoid, EvolutionBlacklist); //Unfun
            AddTag(DLC1Content.Items.ExplodeOnDeathVoid, EvolutionBlacklist); //Op
            AddTag(DLC1Content.Items.ExplodeOnDeathVoid, ScavengerBlacklist); //Op

            #endregion
            #region Modded
            ItemDef tempDef = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("WatchMetronome"));
            if (tempDef != null)
            {
                AddTag(tempDef, ItemTag.AIBlacklist);
            }
            tempDef = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("VV_ITEM_EHANCE_VIALS_ITEM"));
            if (tempDef != null)
            {
                AddTag(tempDef, ItemTag.AIBlacklist);
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



            //RemoveTag(RoR2Content.Items.BarrierOnKill, 0); //Remove Utility
            RemoveTag(RoR2Content.Items.Infusion, 0); //Remove Utility
            AddTag(DLC2Content.Items.TeleportOnLowHealth, ItemTag.Healing);//Barrier is Healing.
            AddTag(DLC3Content.Items.JumpDamageStrike, ItemTag.Utility);


            //Red
            //RemoveTag(RoR2Content.Items.HeadHunter, 0); //Remove Utility
            //RemoveTag(RoR2Content.Items.BarrierOnOverHeal, 0); //Remove Utility, Barrier is Healing.
            AddTag(RoR2Content.Items.NovaOnHeal, ItemTag.Healing); //Healing related

            // Boss
            RemoveTag(RoR2Content.Items.Knurl, 0); //Remove Utility
            AddTag(DLC1Content.Items.MinorConstructOnKill, ItemTag.Utility); //Healing related
            AddTag(DLC3Content.Items.ExtraEquipment, ItemTag.Utility); //Healing related

            #region Lunar

            RoR2Content.Items.RandomDamageZone.tags[0] = ItemTag.Damage; //Utility???
            DLC1Content.Items.HalfSpeedDoubleHealth.tags[0] = ItemTag.Healing; //Utility???
            DLC1Content.Items.LunarSun.tags[0] = ItemTag.Damage; //Utility???
            RoR2Content.Items.LunarUtilityReplacement.tags[0] = ItemTag.Healing; //This Heals you

            AddTag(RoR2Content.Items.ShieldOnly, ItemTag.Healing); //HealthUp
            #endregion

            #region Void

            #endregion

            #endregion

        }



    }


}
