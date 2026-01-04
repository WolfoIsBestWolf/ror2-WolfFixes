using HG;
using RoR2;
using UnityEngine;

namespace WolfoLibrary
{
    public class InstantiateModelParams : MonoBehaviour
    {
        public Vector3 CameraPosition = new Vector3(-1.5f, 1f, 3f);
        public Vector3 FocusPosition = new Vector3(0f, 1f, 0f);
    }
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
                    Log.LogMessage("Removed " + card + " from " + SceneInfo.instance.sceneDef);
                    ArrayUtils.ArrayRemoveAtAndResize(ref dccs.categories[cat].cards, c);
                    return;
                }
            }
            Log.LogWarning("Failed to remove " + card + " from " + SceneInfo.instance.sceneDef);

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
