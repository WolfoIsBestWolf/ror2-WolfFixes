using BepInEx;
using BepInEx.Logging;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Rendering;
using WolfoLibrary.Testing;

namespace WolfoLibrary
{

    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoLibrary", "WolfoLibrary", "1.2.3")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoLib : BaseUnityPlugin
    {
        public static ManualLogSource log;

        public void Awake()
        {
            log = base.Logger;
            Commands.Awake();
            Test.Awake();
        }


        public void Start()
        {
            Commands.cheatsEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("iHarbHD.DebugToolkit");

            WConfig.Awake();
            Test.Start();
            ExtraActions.Start();
            Tags.Start();

            VoidElite.VoidAffix();
            VoidSuppressor.SuppresedScrap();
            VoidSuppressor.FixInteractable();
            //On.RoR2.BodyCatalog.CCBodyGeneratePortraits += BodyCatalog_CCBodyGeneratePortraits;
        }

        private void BodyCatalog_CCBodyGeneratePortraits(On.RoR2.BodyCatalog.orig_CCBodyGeneratePortraits orig, ConCommandArgs args)
        {

            Debug.Log("Starting portrait generation.");
            var iconGenerator = LegacyResourcesAPI.Load<GameObject>("Prefabs/UI/IconGenerator");
            ModelPanel modelPanel = iconGenerator.GetComponentInChildren<ModelPanel>();
            modelPanel.renderSettings = new RenderSettingsState
            {
                haloStrength = 0,
                defaultReflectionResolution = 0,
                defaultReflectionMode = 0,
                reflectionBounces = 0,
                reflectionIntensity = 0,
                customReflection = null,
                sun = null,
                skybox = null,
                subtractiveShadowColor = new Color(0, 0, 0, 0),
                flareStrength = 0,
                ambientLight = new Color(r: 0.01372549f, g: 0.015686275f, b: 0.009558824f, a: 0),
                ambientGroundColor = new Color(r: 10.680627f, g: 10.680627f, b: 10.680627f, a: 0),
                ambientEquatorColor = new Color(r: 1, g: 1, b: 1, a: 0),
                ambientSkyColor = new Color(r: 5.3403134f, g: 5.3403134f, b: 5.3403134f, a: 1),
                ambientMode = AmbientMode.Trilight,
                fogDensity = 0,
                fogColor = new Color(0, 0, 0, 0),
                fogMode = 0,
                fogEndDistance = 0,
                fogStartDistance = 0,
                fog = false,
                ambientIntensity = 1,
                flareFadeSpeed = 0,
            };
            modelPanel.disablePostProcessLayer = true;
            modelPanel.modelPostProcessVolumePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

            orig(args);
        }
    }

}