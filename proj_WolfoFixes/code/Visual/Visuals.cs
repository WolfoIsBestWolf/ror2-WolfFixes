using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    public class ReStartParticleOnStart : MonoBehaviour
    {
        private void OnEnable()
        {
            if (particleSystem)
            {
                particleSystem.enableEmission = true;
            }
        }

        public ParticleSystem particleSystem;
    }
    internal class Visuals
    {
        public static void Start()
        {
            //Scope Alpha fix
            GameObject RailgunnerScopeHeavyOverlay = Addressables.LoadAssetAsync<GameObject>(key: "db5a0c21c1f689c4292ae5e292fd4f0e").WaitForCompletion();
            UnityEngine.UI.RawImage scope = RailgunnerScopeHeavyOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.m_Color = scope.color.AlphaMultiplied(0.7f);
            GameObject RailgunnerScopeLightOverlay = Addressables.LoadAssetAsync<GameObject>(key: "c305c2dadaa35d840bd91dd48987c55e").WaitForCompletion();
            scope = RailgunnerScopeLightOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.m_Color = scope.color.AlphaMultiplied(0.7f);

            //Rachis Radius is slightly wrong, noticible on high stacks 
            GameObject RachisObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/DamageZoneWard");
            RachisObject.transform.GetChild(1).GetChild(2).GetChild(1).localScale = new Vector3(2f, 2f, 2f);

            //Too small plant normally
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Plant/InterstellarDeskPlant.prefab").WaitForCompletion().transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);

            //Unused like blue explosion so he doesn't use magma explosion ig, probably unused for a reason but it looks fine
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion().GetComponent<WormBodyPositions2>().blastAttackEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Junk/ElectricWorm/ElectricWormImpactExplosion.prefab").WaitForCompletion();



            //2D beam fix
            GameObject DeepVoidPortalBattery = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion();
            Transform Beam = DeepVoidPortalBattery.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0);
            Beam.localScale = new Vector3(1, 3, 1); //LongerBeam

            //2D beam fix
            ParticleSystem ps = Beam.GetComponent<ParticleSystem>();
            var psM = ps.main;
            //psM.startRotationX = 6.66f;
            psM.startRotationXMultiplier = 6.66f;
            psM.startRotation3D = true;

            //Glass when isGlass
            IL.RoR2.CharacterModel.UpdateOverlays += ArtifactOfGlassGlassVisual1;
            IL.RoR2.CharacterModel.UpdateOverlayStates += ArtifactOfGlassGlassVisual1;

            //REX vine dissappears too fast
            GameObject EntangleOrbEffect = Addressables.LoadAssetAsync<GameObject>(key: "6e330e0a639bc3d4a9c1c282d70705b1").WaitForCompletion();
            AnimateShaderAlpha[] alphas = EntangleOrbEffect.transform.GetChild(0).GetComponents<AnimateShaderAlpha>();
            alphas[0].continueExistingAfterTimeMaxIsReached = false;
            alphas[1].continueExistingAfterTimeMaxIsReached = true;
            //EntangleOrbEffect.GetComponent<DetachParticleOnDestroyAndEndEmission>().enabled = false;



            bool otherGrandparent = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyFixes");
            if (true)
            {
                //Grandparent invisible rock
                GameObject miniRock = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandparentMiniBoulderGhost.prefab").WaitForCompletion();
                //MeshFilter mesh = miniRock.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>();
                //mesh.sharedMesh = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/skymeadow/SMRockAngular.fbx").WaitForCompletion().GetComponent<MeshFilter>().mesh;
                miniRock.transform.GetChild(0).localScale *= 30;
            }


            On.RoR2.DetachParticleOnDestroyAndEndEmission.OnDisable += RenableParticlesOnEnable;

            On.RoR2.TeleporterInteraction.Start += FixOrbOverlap;

            DLC3Fixes();

            //Fix random white ugly fuck donut appearing since SotS
            GameObject PortalVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion();
            PortalVoid.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
            PortalVoid.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);


            //Show extra stocks
            IL.RoR2.UI.SkillIcon.Update += SkillIcon_Update;
            Addressables.LoadAssetAsync<SkillDef>(key: "067364fa932fb5b4793f0546a08709ca").WaitForCompletion().hideStockCount = true;

            //Collective bubble not visible from inside
            //GameObject AffixCollectiveBodyAttachment = Addressables.LoadAssetAsync<GameObject>(key: "479df04eb1a9cb845bdedfaf9ea71cd6").WaitForCompletion();
            //AffixCollectiveBodyAttachment.transform.GetChild(0).GetChild(3).GetComponent<RoR2.UI.MainMenu.PlatformToggle>().Steam = true;


        }

        private static void SkillIcon_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("RoR2.GenericSkill", "get_maxStock")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<int, RoR2.UI.SkillIcon, int>>((yes, self) =>
                {
                    if (self.targetSkill.stock > 1)
                    {
                        return 2;
                    }
                    return yes;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed: IL.SkillIcon_Update");
            }
        }

        public static void DLC3Fixes()
        {
            #region Extractor
            //Fix Extractor legs not gaining Elite Colors
            //Extractor Legs Shader does not support Elite Colors which is why they dont get colored
            Material ExtractorLeg = Addressables.LoadAssetAsync<Material>(key: "38551ddc920facb4bb25705b778af6c1").WaitForCompletion();
            ExtractorLeg.shader = Addressables.LoadAssetAsync<Material>(key: "5e992afb2f42161449e0bcc85f314a6d").WaitForCompletion().shader; //Switch to default Shader, which supports Elite
            ExtractorLeg.DisableKeyword("SPLATMAP"); //Turns pure white with normal shader for some reason.

            //Make it not like transparent spaghetti on some elite types
            SkinDefParams Extractor_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "d97f2fe9113ec3744a0c23710d8c2f48").WaitForCompletion();
            for (int i = 0; i < 6; i++)
            {
                Extractor_params.rendererInfos[i].ignoreOverlays = false;
            }
            #endregion
            #region Tanker
            Material matTankerGlass = Addressables.LoadAssetAsync<Material>(key: "561bdde5eaee70747ab98b0245a1d786").WaitForCompletion();

            //Not really feasible unless we removed a render info?
            //But would that matter?

            #endregion
            #region Distributer
            //Does not cast shadows
            SkinDefParams MinePod_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "e149ed8a264a5a64a99c42e590e997f6").WaitForCompletion();
            MinePod_params.rendererInfos[0].defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            #endregion

            //Fix Tier2 Drone Colors
            IL.RoR2.CharacterModel.GetDroneUpgradePair += CharacterModel_GetDroneUpgradePair;

            //Spurs visual sets diamater not radius, attack uses radius just fine.
            IL.RoR2.Items.JumpDamageStrikeBodyBehavior.UpdateAura += SpurHalfVisualRadius;

            //Shouldnt show. Annoying for Drifter
            On.RoR2.VehicleSeat.ShouldShowOnScanner += VehicleSeat_ShouldShowOnScanner;

            //1 Item misaligned
            //GameObject TemporaryItemsShopTerminal = Addressables.LoadAssetAsync<GameObject>(key: "d31df5066858329458b33f21b3b22d2e").WaitForCompletion();
            //TemporaryItemsShopTerminal.transform.GetChild(1).GetChild(3).localPosition = new Vector3(0f, 2.5f, 0.058f);

        }

        private static bool VehicleSeat_ShouldShowOnScanner(On.RoR2.VehicleSeat.orig_ShouldShowOnScanner orig, VehicleSeat self)
        {
            return false;
        }

        private static void SpurHalfVisualRadius(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCall("RoR2.SphereZone", "set_Networkradius")))
            {
                c.EmitDelegate<System.Func<float, float>>((fake) =>
                {
                    return fake * 2f;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed: IL.SpurHalfVisualRadius");
            }
        }

        private static void CharacterModel_GetDroneUpgradePair(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdcI4(0)))
            {
                c.EmitDelegate<System.Func<int, int>>((fake) =>
                {
                    if (fake == 0)
                    {
                        return 1;
                    }
                    return fake;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed: IL.CharacterModel_GetDroneUpgradePair");
            }
        }

        private static void FixOrbOverlap(On.RoR2.TeleporterInteraction.orig_Start orig, TeleporterInteraction self)
        {
            orig(self);
            //MSOrb overlaps VoidOrb
            Transform MSOrb = self.modelChildLocator.FindChild("MSPortalIndicator");
            MSOrb.localEulerAngles = new Vector3(0, 0, 135f);
            //MSOrb Trail mat isn't a good fit for it, but there's not really a ideal mat
            //Short of making a new mat entirely
            //This helps but looks kinda off still
            TrailRenderer MSTrail = MSOrb.GetChild(0).GetComponent<TrailRenderer>();
            MSTrail.endColor = new Color(1, 1, 1, 0.9f);
            MSTrail.widthMultiplier = 0.5f;
        }

        private static void RenableParticlesOnEnable(On.RoR2.DetachParticleOnDestroyAndEndEmission.orig_OnDisable orig, DetachParticleOnDestroyAndEndEmission self)
        {
            orig(self);
            //Pooled effects get reused
            //So permamently disabling a particle effect means they dont show up again
            //And look wrong upon re-using
            //Small list but for the REX fix
            if (self.GetComponent<EffectComponent>())
            {
                if (!self.GetComponent<ReStartParticleOnStart>())
                {
                    self.gameObject.AddComponent<ReStartParticleOnStart>().particleSystem = self.particleSystem;
                }
            }

        }

        private static void ArtifactOfGlassGlassVisual1(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("RoR2.RoR2Content/Items", "LunarDagger"));
            if (a && c.TryGotoNext(MoveType.Before,
            x => x.MatchBr(out _)))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<bool, CharacterModel, bool>>((yes, model) =>
                {
                    if (model.body.isGlass)
                    {
                        return true;
                    }
                    return yes;
                });
            }
            else
            {
                WolfFixes.log.LogWarning("IL Failed: IL.CharacterModel_UpdateOverlays");
            }
        }










    }

}