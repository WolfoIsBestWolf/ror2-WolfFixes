using RoR2;
using RoR2.UI;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace WolfoLibrary
{   /*
    public class PortraitGenerator
    {
        [ConCommand(commandName = "generate_portrait", flags = ConVarFlags.None, helpText = "Generate Portrait, BodyIndex, R,G,B,Zoom, ItemIndex")]
        public static void CC_RemoveUnlocks(ConCommandArgs args)
        {
            BodyIndex bodyIndex = args.TryGetArgBodyIndex(0).GetValueOrDefault(BodyIndex.None);
            if (bodyIndex == BodyIndex.None)
            {
                bodyIndex = (BodyIndex)args.TryGetArgInt(0).GetValueOrDefault();
            }
            float red = args.TryGetArgFloat(1).GetValueOrDefault(1f);
            float grn = args.TryGetArgFloat(2).GetValueOrDefault(1f);
            float blu = args.TryGetArgFloat(3).GetValueOrDefault(1f);
            float fov = args.TryGetArgFloat(4).GetValueOrDefault(40);
            bool delete = args.TryGetArgBool(5).GetValueOrDefault(false);
            ItemIndex item = (ItemIndex)args.TryGetArgInt(6).GetValueOrDefault(-1);


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
                ambientGroundColor = new Color(r: 2 * red, g: 2 * grn, b: 2 * blu, a: 0),
                ambientEquatorColor = new Color(r: 1, g: 1, b: 1, a: 0),
                ambientSkyColor = new Color(r: red, g: grn, b: blu, a: 1),
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
            //modelPanel.desiredZoom = zoom;
            modelPanel.fov = fov;
            modelPanel.disablePostProcessLayer = true;
            modelPanel.modelPostProcessVolumePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            modelPanel.modelPostProcessVolumePrefab.SetActive(false);
            RoR2Application.instance.StartCoroutine(GenerateIndividualPortrait(bodyIndex, delete, item));

        }


        private static IEnumerator GenerateIndividualPortrait(BodyIndex body, bool delete, ItemIndex item = ItemIndex.None)
        {
            Debug.Log("Generating Portrait for: " + body);
            var iconGenerator = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/UI/IconGenerator"));
            var modelPanel = iconGenerator.GetComponentInChildren<ModelPanel>();
            yield return GeneratePortrait(modelPanel, BodyCatalog.GetBodyPrefab(body), delete, item);
            UnityEngine.Object.Destroy(modelPanel.transform.root.gameObject);
            Debug.Log("Portrait generation complete.");
        }

        private static IEnumerator GeneratePortrait(ModelPanel modelPanel, GameObject gameObject, bool delete, ItemIndex itemIndex = ItemIndex.None)
        {
            CharacterBody characterBody = gameObject.GetComponent<CharacterBody>();
            if ((bool)characterBody)
            {
                float num = 1f;
                try
                {
                    Debug.Log($"Generating portrait for {gameObject.name} | {delete}");



                    modelPanel.modelPrefab = gameObject.GetComponent<ModelLocator>()?.modelTransform.gameObject;


                    modelPanel.SetAnglesForCharacterThumbnail(setZoom: true);


                    PrintController printController;
                    if ((object)(printController = modelPanel.modelInstance?.GetComponentInChildren<PrintController>()) != null)
                    {
                        num = Mathf.Max(num, printController.printTime + 1f);
                    }
                    TemporaryOverlay temporaryOverlay;
                    if ((object)(temporaryOverlay = modelPanel.modelInstance?.GetComponentInChildren<TemporaryOverlay>()) != null)
                    {
                        num = Mathf.Max(num, temporaryOverlay.duration + 1f);
                    }
                
                    var lights = modelPanel.modelInstance.GetComponentsInChildren<Light>();
                    foreach (var light in lights)
                    {
                        light.enabled = false;
                    }
                    if (delete)
                    {
                        UnityEngine.Object.Destroy(modelPanel.modelInstance.GetComponent<ModelPanelParameters>());
                    }
                }
                catch (Exception message)
                {
                    Debug.Log(message);
                }

                if (itemIndex != ItemIndex.None)
                {
                    modelPanel.modelInstance?.GetComponent<CharacterModel>()?.EnableItemDisplay(itemIndex);
                }
                modelPanel.SetAnglesForCharacterThumbnail(setZoom: true);
                RoR2Application.onLateUpdate += UpdateCamera;
                yield return new WaitForSeconds(num);
                modelPanel.SetAnglesForCharacterThumbnail(setZoom: true);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                try
                {
                    Texture2D texture2D = new Texture2D(modelPanel.renderTexture.width, modelPanel.renderTexture.height, TextureFormat.ARGB32, mipChain: false, linear: false);
                    RenderTexture active = RenderTexture.active;
                    RenderTexture.active = modelPanel.renderTexture;
                    texture2D.ReadPixels(new Rect(0f, 0f, modelPanel.renderTexture.width, modelPanel.renderTexture.height), 0, 0, recalculateMipMaps: false);
                    RenderTexture.active = active;
                    byte[] array = texture2D.EncodeToPNG();
                    FileStream fileStream = new FileStream("Assets/RoR2/GeneratedPortraits/" + gameObject.name + ".png", FileMode.Create, FileAccess.Write);
                    fileStream.Write(array, 0, array.Length);
                    fileStream.Close();
                }
                catch (Exception message2)
                {
                    Debug.Log(message2);
                }
                RoR2Application.onLateUpdate -= UpdateCamera;
                yield return new WaitForEndOfFrame();
            }
            void UpdateCamera()
            {
                modelPanel.SetAnglesForCharacterThumbnail(setZoom: true);
            }
        }
    }
    */
}
