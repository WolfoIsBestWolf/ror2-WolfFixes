using RoR2;

namespace WolfoFixes
{
    internal class SkinFixes
    {


        public static void Start()
        {


            On.RoR2.CharacterModel.IsAurelioniteAffix += DontRemoveGildedVisualsOnDeath;
            //On.RoR2.CharacterModel.Start += CharacterModel_Start;
        }

        private static void CharacterModel_Start(On.RoR2.CharacterModel.orig_Start orig, CharacterModel self)
        {
            orig(self);
            self.fullBodyPing = true;
        }

        private static bool DontRemoveGildedVisualsOnDeath(On.RoR2.CharacterModel.orig_IsAurelioniteAffix orig, CharacterModel self)
        {
            if (self.myEliteIndex == DLC2Content.Elites.Aurelionite.eliteIndex)
            {
                return true;
            }
            return orig(self);
        }


    }

}