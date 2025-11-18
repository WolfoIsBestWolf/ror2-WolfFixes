using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoLibrary
{

    public static class ModUtil
    {

        public static List<EquipmentIndex> GetEliteEquipment(bool droppableOnly = true)
        {
            List<EquipmentIndex> eliteEquips = new List<EquipmentIndex>();
            try
            {
                for (int i = 0; i < EquipmentCatalog.equipmentDefs.Length; i++)
                {
                    EquipmentDef def = EquipmentCatalog.equipmentDefs[i];
                    if (def.passiveBuffDef && def.passiveBuffDef.eliteDef)
                    {

                        if ((def.dropOnDeathChance > 0 && (!Run.instance || !Run.instance.expansionLockedEquipment.Contains(def.equipmentIndex))) || !droppableOnly)
                        {
                            eliteEquips.Add(def.equipmentIndex);
                        }
                    }
                }

                if (!droppableOnly)
                {
                    eliteEquips.Remove(MissedContent.Equipment.EliteSecretSpeedEquipment.equipmentIndex);
                    eliteEquips.Remove(JunkContent.Equipment.EliteYellowEquipment.equipmentIndex);
                    eliteEquips.Remove(JunkContent.Equipment.EliteGoldEquipment.equipmentIndex);
                }

            }
            catch (System.Exception e)
            {
                //This caused too many problems in the past might as well put it in try
                Debug.LogWarning(e);
            }
            return eliteEquips;
        }


        public enum SkinVariant
        {
            Default,
            Snow,
            Sand,
        }
        public static SkinVariant GetInteractableSkinFromStage(SceneDef scene)
        {
            switch (scene.baseSceneName)
            {
                case "snowyforest":
                case "nest":
                case "frozenwall":
                case "itfrozenwall":
                    return SkinVariant.Snow;
                case "goolake":
                case "itgoolake":
                case "lemuriantemple":
                case "ironalluvium":
                case "ironalluvium2":
                case "repurposedcrater":
                    return SkinVariant.Sand;

            }
            return SkinVariant.Default;
        }
    }

}
