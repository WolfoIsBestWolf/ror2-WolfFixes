using RoR2;

namespace WolfoFixes
{
    internal class SkinFixes
    {
        public static void Start()
        {
            On.RoR2.CharacterModel.IsAurelioniteAffix += DontRemoveGildedVisualsOnDeath;
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