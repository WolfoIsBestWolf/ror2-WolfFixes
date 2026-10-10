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
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Plant/InterstellarDeskPlant.prefab").WaitForCompletion().transform.GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0.5f);

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


            On.RoR2.DetachParticleOnDestroyAndEndEmission.OnDisable += RenableParticlesOnEnable;

            On.RoR2.TeleporterInteraction.Start += FixOrbOverlap;

            DLC3Fixes();


            //Show extra stocks
            IL.RoR2.UI.SkillIcon.LateUpdate += SkillIcon_Update;
            Addressables.LoadAssetAsync<SkillDef>(key: "067364fa932fb5b4793f0546a08709ca").WaitForCompletion().hideStockCount = true;

            //Collective bubble not visible from inside
            //GameObject AffixCollectiveBodyAttachment = Addressables.LoadAssetAsync<GameObject>(key: "479df04eb1a9cb845bdedfaf9ea71cd6").WaitForCompletion();
            //AffixCollectiveBodyAttachment.transform.GetChild(0).GetChild(3).GetComponent<RoR2.UI.MainMenu.PlatformToggle>().Steam = true;

            On.RoR2.GeodeController.ShouldShowOnScanner += GeodeController_ShouldShowOnScanner;

        }
        //Checks for !available, dont do that
        private static bool GeodeController_ShouldShowOnScanner(On.RoR2.GeodeController.orig_ShouldShowOnScanner orig, GeodeController self)
        {
            return self.available;
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
                WolfFixes.log.LogError("IL Failed: IL.SkillIcon_Update");
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
 
            #region Distributer
            //Does not cast shadows
            SkinDefParams MinePod_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "e149ed8a264a5a64a99c42e590e997f6").WaitForCompletion();
            MinePod_params.rendererInfos[0].defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            #endregion

            //1 Item misaligned
            //GameObject TemporaryItemsShopTerminal = Addressables.LoadAssetAsync<GameObject>(key: "d31df5066858329458b33f21b3b22d2e").WaitForCompletion();
            //TemporaryItemsShopTerminal.transform.GetChild(1).GetChild(3).localPosition = new Vector3(0f, 2.5f, 0.058f);

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
                WolfFixes.log.LogError("IL Failed: IL.CharacterModel_UpdateOverlays");
            }
        }










    }

}