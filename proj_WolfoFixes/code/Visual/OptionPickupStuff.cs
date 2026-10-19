using RoR2;
using UnityEngine;


namespace WolfoFixes
{

    internal class OptionPickupStuff
    {
        public static void Start()
        {
            On.RoR2.PickupPickerController.SetOptionsInternal += OptionPickup_Fixes;


        }

        private static void OptionPickup_Fixes(On.RoR2.PickupPickerController.orig_SetOptionsInternal orig, PickupPickerController self, PickupPickerController.Option[] newOptions)
        {
            orig(self, newOptions);
            PickupIndexNetworker index = self.GetComponent<PickupIndexNetworker>();
            if (index != null)
            {
                //Void Command Essence lacks particles.
                if (self.name.StartsWith("Command"))
                {
                    if (index.pickupDisplay)
                    {
                        var pickupDef = index.pickupState.pickupIndex.pickupDef;
                        if (pickupDef.itemTier >= ItemTier.VoidTier1 && pickupDef.itemTier <= ItemTier.VoidBoss)
                        {
                            //Not default token so double checking
                            if (Language.english.TokenIsRegistered("ARTIFACT_COMMAND_CUBE_PINK_NAME"))
                            {
                                self.gameObject.GetComponent<GenericDisplayNameProvider>().displayToken = "ARTIFACT_COMMAND_CUBE_PINK_NAME";
                            }
                        }
                    }
                }
                else
                {
                    //Fix Aur fragment not spinning on clients
                    if (self.transform.childCount == 0)
                    {
                        return;
                    }
                    PickupDisplay pickupDisplay = self.transform.GetChild(0).GetComponent<PickupDisplay>();
                    if (pickupDisplay)
                    {
                        pickupDisplay.pickupState = index.pickupState;
                        MeshRenderer mesh = self.GetComponentInChildren<MeshRenderer>();
                        if (mesh != null)
                        {
                            pickupDisplay.modelObject = mesh.gameObject;
                        }
                    }
                    Highlight highlight = self.GetComponent<Highlight>();
                    if (highlight != null)
                    {
                        highlight.pickupState = index.pickupState;
                        highlight.isOn = true;
                    }
                }

            }






        }



    }
}