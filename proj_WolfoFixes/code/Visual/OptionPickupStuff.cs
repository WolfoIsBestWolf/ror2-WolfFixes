using RoR2;


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
                        var pickupDef = index.pickupIndex.pickupDef;
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
                    //Command does not Spin
                    //Fix spinning on client.
                    PickupDisplay pickupDisplay = self.transform.GetChild(0).GetComponent<PickupDisplay>();
                    pickupDisplay.pickupState = index.pickupState;
                    pickupDisplay.modelObject = self.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                    self.GetComponent<Highlight>().pickupState = index.pickupState;
                    self.GetComponent<Highlight>().isOn = true;

                }

            }






        }



    }
}