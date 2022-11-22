using CloserMaterial.Info;
using System.Collections.Generic;
using UnityEngine;

namespace CloserMaterial
{
    public static class Utils
    {
        public static Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

        public static void RegisterSimHash(string name)
        {
            SimHashes simHash = (SimHashes)Hash.SDBMLower(name);
            SimHashNameLookup.Add(simHash, name);
            ReverseSimHashNameLookup.Add(name, simHash);
        }

        public static void RegisterElementStrings(string elementId)
        {
            string upperElemId = elementId.ToUpper();
            Strings.PrintTable();
            Strings.Add($"STRINGS.ELEMENTS.{upperElemId}.NAME", STRINGS.UI.FormatAsLink(Strings.Get("STRINGS.InfoDataStrings.STRING_PLAN_MATERIAL_NAME"), upperElemId));
            Strings.Add($"STRINGS.ELEMENTS.{upperElemId}.DESC", Strings.Get("STRINGS.InfoDataStrings.STRING_PLAN_MATERIAL_DESCRIPTION"));
        }

        public static KAnimFile FindAnim(string name)
        {
            KAnimFile result = Assets.Anims.Find((anim) => anim.name == name);
            if (result == null)
                Debug.LogError($"Failed to find KAnim: {name}");
            return result;
        }

        public static void AddSubstance(Substance substance)
        {
            Assets.instance.substanceTable.GetList().Add(substance);
        }

        public static Substance CreateSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour)
        {
            return ModUtil.CreateSubstance(name, state, kanim, material, colour, colour, colour);
        }

        public static Substance CreateRegisteredSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour)
        {
            Substance result = CreateSubstance(name, state, kanim, material, colour);
            RegisterSimHash(name);
            AddSubstance(result);
            ElementLoader.FindElementByHash(result.elementID).substance = result;
            return result;
        }

        static Texture2D TintTexture(Texture sourceTexture, string name)
        {
            Texture2D newTexture = DuplicateTexture(sourceTexture as Texture2D);
            var pixels = newTexture.GetPixels32();
            for (int i = 0; i < pixels.Length; ++i)
            {
                var gray = ((Color)pixels[i]).grayscale * 1.5f;
                pixels[i] = (Color)InfoData.PLAN_COLOR * gray;
            }
            newTexture.SetPixels32(pixels);
            newTexture.Apply();
            newTexture.name = name;
            return newTexture;
        }

        static Material CreateMaterial(Material source)
        {
            var planMaterial = new Material(source);

            Texture2D newTexture = TintTexture(planMaterial.mainTexture, "planbuildable");

            planMaterial.mainTexture = newTexture;
            planMaterial.name = "matPlanBuildable";

            return planMaterial;
        }

        public static void RegisterElementsRaw()
        {
            Substance sand = Assets.instance.substanceTable.GetSubstance(SimHashes.SandStone);

            CreateRegisteredSubstance(
              name: InfoData.PLANBUILDABLERAW_ID,
              state: Element.State.Solid,
              kanim: FindAnim("planbuildable_kanim"),
              material: CreateMaterial(sand.material),
              colour: InfoData.PLAN_COLOR
            );
        }

        public static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}
