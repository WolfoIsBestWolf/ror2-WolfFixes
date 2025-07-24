using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoFixes
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

    }

}
