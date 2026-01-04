using EntityStates;
using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace WolfoFixes
{
    internal class RandomFixes
    {

        public static void Start()
        {


            On.RoR2.PortalStatueBehavior.PreStartClient += NewtAvailableFix12;
            On.RoR2.TeleporterInteraction.OnSyncShouldAttemptToSpawnShopPortal += NewtAvailableFix2;


            //Some interactables have empty holograms
            On.RoR2.PurchaseInteraction.ShouldDisplayHologram += DisableEmptyHologram;
            On.RoR2.MultiShopController.ShouldDisplayHologram += DisableEmptyHologram2;

            On.EntityStates.VagrantNovaItem.ChargeState.OnExit += ChargeState_OnExit;

            IL.RoR2.PingerController.GeneratePingInfo += FixUnpingableHelminthRocks;

            //Always open chat when a chat message is sent inbetween stages
            On.RoR2.UI.ChatBox.Awake += ChatBox_Awake;
            Chat.onChatChanged += Chat_onChatChanged;

            //Always update hud if rebuild cards
            On.RoR2.ClassicStageInfo.RebuildCards += MarkHudAsDirtyForKin;

            //Fixed 1.4.1
            //IL.RoR2.UI.ScrapperInfoPanelHelper.AddQuantityToPickerButton += ScrapperInfoPanelHelper_AddQuantityToPickerButton;
        }

        private static void ScrapperInfoPanelHelper_AddQuantityToPickerButton(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCallvirt("RoR2.Inventory", "GetItemCountEffective")
            ))
            {

                c.Next.Operand = AccessTools.Method(typeof(Inventory), nameof(Inventory.GetItemCountPermanent), new[] { typeof(ItemIndex) });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : ScrapperInfoPanelHelper_AddQuantityToPickerButton");
            }
        }

        private static void MarkHudAsDirtyForKin(On.RoR2.ClassicStageInfo.orig_RebuildCards orig, ClassicStageInfo self, DirectorCardCategorySelection forcedMonsterCategory, DirectorCardCategorySelection forcedInteractableCategory)
        {
            orig(self, forcedMonsterCategory, forcedInteractableCategory);
            RoR2.UI.EnemyInfoPanel.MarkDirty();
        }

        private unsafe static void Chat_onChatChanged()
        {
            if (RoR2.UI.ChatBox.instance == null)
            {
                displayChat = true;
            }
            else
            {
                displayChat = false;
            }
        }

        private static void ChatBox_Awake(On.RoR2.UI.ChatBox.orig_Awake orig, RoR2.UI.ChatBox self)
        {
            orig(self);
            if (displayChat)
            {
                self.OnChatChangedHandler();
            }
        }

        public static bool displayChat = false;


        private static void FixUnpingableHelminthRocks(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdloc(11),
            x => x.MatchCall("UnityEngine.Object", "op_Implicit")
            ))
            {
                //Missing Null Check
                c.Index++;
                c.EmitDelegate<System.Func<EntityLocator, EntityLocator>>((entitylocator) =>
                {
                    if (entitylocator && entitylocator.entity == null)
                    {
                        return null;
                    }
                    return entitylocator;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : PingerController_GeneratePingInfo");
            }
        }

        private static void FixChronocDisplayNullRefOnCorpse(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("RoR2.IncreaseDamageOnMultiKillItemDisplayUpdater", "body")
            ))
            {
                //Missing Null Check
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<CharacterBody, IncreaseDamageOnMultiKillItemDisplayUpdater, CharacterBody>>((body, self) =>
                {
                    if (!body)
                    {
                        GameObject.Destroy(self);
                        return null;
                    }
                    return body;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : IncreaseDamageOnMultiKillItemDisplayUpdater");
            }
        }


        private static void ChargeState_OnExit(On.EntityStates.VagrantNovaItem.ChargeState.orig_OnExit orig, EntityStates.VagrantNovaItem.ChargeState self)
        {
            //Idk does this fix it randomly being around for clientss
            //Probably has smth to do with lag skipping the OnExit?
            if (self.chargeVfxInstance != null)
            {
                EntityState.Destroy(self.chargeVfxInstance);
            }
            if (self.areaIndicatorVfxInstance != null)
            {
                EntityState.Destroy(self.areaIndicatorVfxInstance);
            }
            orig(self);
        }

        private static bool DisableEmptyHologram2(On.RoR2.MultiShopController.orig_ShouldDisplayHologram orig, MultiShopController self, UnityEngine.GameObject viewer)
        {
            if (self.costType == CostTypeIndex.None)
            {
                return false;
            }
            return orig(self, viewer);
        }

        private static bool DisableEmptyHologram(On.RoR2.PurchaseInteraction.orig_ShouldDisplayHologram orig, PurchaseInteraction self, UnityEngine.GameObject viewer)
        {
            if (self.costType == CostTypeIndex.None)
            {
                return false;
            }
            return orig(self, viewer);
        }

        private static void NewtAvailableFix12(On.RoR2.PortalStatueBehavior.orig_PreStartClient orig, PortalStatueBehavior self)
        {
            orig(self);
            self.GetComponent<PurchaseInteraction>().setUnavailableOnTeleporterActivated = true;
        }

        private static void NewtAvailableFix2(On.RoR2.TeleporterInteraction.orig_OnSyncShouldAttemptToSpawnShopPortal orig, TeleporterInteraction self, bool newValue)
        {
            orig(self, newValue);
            if (newValue == true)
            {
                foreach (PortalStatueBehavior portalStatueBehavior in UnityEngine.Object.FindObjectsOfType<PortalStatueBehavior>())
                {
                    if (portalStatueBehavior.portalType == PortalStatueBehavior.PortalType.Shop)
                    {
                        PurchaseInteraction component = portalStatueBehavior.GetComponent<PurchaseInteraction>();
                        if (component)
                        {
                            component.Networkavailable = false;
                            portalStatueBehavior.CallRpcSetPingable(portalStatueBehavior.gameObject, false);
                        }
                    }
                }
            }

        }






    }

}
