using HG;
using RoR2;

namespace WolfoLibrary
{

    public class DccsUtil
    {
        public static void RemoveCard(DirectorCardCategorySelection dccs, int cat, string card)
        {
            //Because categories get shifted around in DCCS Blender, better to just find them
            string a = "";
            switch (cat)
            {
                case 2:
                    a = "Basic Monsters";
                    break;
                case 1:
                    a = "Minibosses";
                    break;
                case 0:
                    a = "Champions";
                    break;

            }
            cat = dccs.FindCategoryIndexByName(a);
            if (cat != -1)
            {
                int c = FindSpawnCard(ref dccs.categories[cat].cards, card);
                if (c != -1)
                {
                    WolfoLib.log.LogMessage("Removed " + card + " from " + SceneInfo.instance.sceneDef);
                    ArrayUtils.ArrayRemoveAtAndResize(ref dccs.categories[cat].cards, c);
                    return;
                }
            }
            WolfoLib.log.LogWarning("Failed to remove " + card + " from " + SceneInfo.instance.sceneDef);

        }

        public static int FindSpawnCard(ref DirectorCard[] insert, string LookingFor)
        {
            //WolfoMain.Logger.LogWarning(insert);
            for (int i = 0; i < insert.Length; i++)
            {
                //WolfoMain.Logger.LogWarning(insert[i].spawnCard);
                if (insert[i].GetSpawnCard() && insert[i].GetSpawnCard().name.EndsWith(LookingFor))
                {
                    //Debug.Log("Found " + LookingFor);
                    return i;
                }
            }
            //WolfoMain.Logger.LogWarning("Couldn't find " + LookingFor);
            return -1;
        }

        public static DirectorCard GetDirectorCard(ref DirectorCard[] insert, string LookingFor)
        {
            //WolfoLib.log.LogWarning(insert);
            for (int i = 0; i < insert.Length; i++)
            {
                //WolfoLib.log.LogWarning(insert[i].GetSpawnCard());
                if (insert[i].GetSpawnCard() && insert[i].GetSpawnCard().name.EndsWith(LookingFor))
                {
                    return insert[i];
                }
            }
            return null;
        }

    }

    public class HLang
    {



    }

}
