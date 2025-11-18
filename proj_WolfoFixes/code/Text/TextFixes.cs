using MonoMod.Cil;
using RoR2;
using RoR2.CharacterSpeech;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    internal class TextFixes
    {

        public static void Start()
        {
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponent<CharacterBody>().subtitleNameToken = "LUNAREXPLODER_BODY_SUBTITLE";
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AssassinBody").GetComponent<CharacterBody>().baseNameToken = ""; //He is not ENGI_BODY

            //Fix False Son saying PlayerKilled line instead of Death line.
            GameObject FalseSonBossFinalPhaseSpeechController = Addressables.LoadAssetAsync<GameObject>(key: "1d9905c8d70993e4f83872c3d07d89d4").WaitForCompletion();
            SimpleCombatSpeechDriver speech = FalseSonBossFinalPhaseSpeechController.GetComponent<SimpleCombatSpeechDriver>();
            speech.speechCategories[2].speechInfos = new SimpleCombatSpeechDriver.SpeechInfo[]
            {
                new SimpleCombatSpeechDriver.SpeechInfo
                {
                    nameToken = "FALSESONBOSS_DEATH_1", weight = 10
                },
                new SimpleCombatSpeechDriver.SpeechInfo
                {
                    nameToken = "FALSESONBOSS_DEATH_2", weight = 10
                },
                new SimpleCombatSpeechDriver.SpeechInfo
                {
                    nameToken = "FALSESONBOSS_DEATH_3", weight = 10
                },
                new SimpleCombatSpeechDriver.SpeechInfo
                {
                    nameToken = "FALSESONBOSS_DEATH_4", weight = 10
                },
                new SimpleCombatSpeechDriver.SpeechInfo
                {
                    nameToken = "FALSESONBOSS_DEATH_5", weight = 10
                },
                new SimpleCombatSpeechDriver.SpeechInfo
                {
                    nameToken = "FALSESONBOSS_DEATH_6", weight = 10
                }
            };

            On.RoR2.ClassicStageInfo.BroadcastFamilySelection += NoFamilyAnnouncementOnDissonance;
            IL.RoR2.BossGroup.UpdateObservations += BossSubtitleMissingSpace;
        }

        private static void BossSubtitleMissingSpace(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdstr("<sprite name=\"CloudRight\" tint=1>")))
            {
                c.Next.Operand = " <sprite name=\"CloudRight\" tint=1>";
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed : BossSubtitleMissingSpace");
            }
        }

        private static System.Collections.IEnumerator NoFamilyAnnouncementOnDissonance(On.RoR2.ClassicStageInfo.orig_BroadcastFamilySelection orig, ClassicStageInfo self, string familySelectionChatString)
        {
            if (RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.MixEnemy))
            {
                familySelectionChatString = string.Empty;
            }
            return orig(self, familySelectionChatString);
        }

        internal static void ItemText()
        {





            //Unique buff (nitpick)
            //LanguageAPI.Add("ITEM_BOOSTALLSTATS_DESC", "Grants <style=cIsUtility>4%</style> increase to <style=cIsUtility>ALL stats</style> for each unique buff, up to a maximum of <style=cIsUtility>4</style> <style=cStack>(+4 per stack)</style>.", "en");

            //Added damage and proc cap
            //LanguageAPI.Add("ITEM_METEORATTACKONHIGHDAMAGE_DESC", "<style=cIsDamage>3%</style> chance on hit to call a meteor strike, dealing <style=cIsDamage>2000%</style> base damage. For every <style=cIsDamage>100%</style> attack damage, increase activation chance by <style=cIsDamage>3%</style> <style=cStack>(+3% per stack)</style> and damage by <style=cIsDamage>150%</style> <style=cStack>(+50% per stack)</style>. Maximum cap of <style=cIsDamage>7500%</style> damage and <style=cIsDamage>75%</style> chance.", "en");


            //Funny All Stats
            //LanguageAPI.Add("ITEM_SHINYPEARL_DESC", "Increases <style=cIsDamage>damage</style>, <style=cIsDamage>attack speed</style>, <style=cIsDamage>critical strike chance</style>, <style=cIsHealing>maximum health</style>, <style=cIsHealing>base health regeneration</style>, <style=cIsHealing>base armor</style>, <style=cIsUtility>movement speed</style> by <style=cIsDamage>1<style=cIsHealing>0<style=cIsUtility>%<style=cStack> (+10% per stack)</style></style></style></style>", "en");


            #region Equipment


            //Red downside text
            //LanguageAPI.Add("EQUIPMENT_CRIPPLEWARD_DESC", "<color=#FF7F7F>ALL characters</color> within have their <style=cIsUtility>movement speed slowed by 100%</style> and have their <style=cIsDamage>armor reduced by 20</style>. Can place up to <style=cIsUtility>5</style>.", "en");

            //Limit of 3
            // LanguageAPI.Add("EQUIPMENT_DEATHPROJECTILE_DESC", "Throw a cursed doll out that <style=cIsDamage>triggers</style> any <style=cIsDamage>On-Kill</style> effects you have every <style=cIsUtility>1</style> second for <style=cIsUtility>8</style> seconds. Cannot throw out more than <style=cIsUtility>3</style> dolls at a time.", "en");

            //Yellow Item text
            //LanguageAPI.Add("EQUIPMENT_BOSSHUNTER_DESC", "<style=cIsDamage>Execute</style> any enemy capable of spawning a <style=cIsTierBoss>unique reward</style>, and it will drop that <style=cIsDamage>item</style>. Equipment is <style=cIsUtility>consumed</style> on use.", "en");
            #endregion
        }

        internal static void CharacterText()
        {

            //LanguageAPI.Add("MAGE_SPECIAL_FIRE_DESCRIPTION", "Burn all enemies in front of you for <style=cIsDamage>2000% damage</style>.");


            //LanguageAPI.Add("CAPTAIN_UTILITY_ALT1_DESCRIPTION", "<style=cIsDamage>Stunning</style>. Request a <style=cIsDamage>kinetic strike</style> from the <style=cIsDamage>UES Safe Travels</style>. After <style=cIsUtility>20 seconds</style>, it deals <style=cIsDamage>40,000% damage</style> to enemies and <style=cIsDamage>20,000% damage</style> to ALL ALLIES..");

        }


    }

}
