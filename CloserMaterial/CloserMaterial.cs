using CloserMaterial.Info;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Actions;
using PeterHan.PLib.AVC;
using PeterHan.PLib.Core;
using PeterHan.PLib.Database;
using PeterHan.PLib.Options;
using PeterHan.PLib.PatchManager;
using PeterHan.PLib.UI;
using System.Reflection;

namespace CloserMaterial
{
    public sealed class CloserMaterial : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new POptions().RegisterOptions((UserMod2)this, typeof(CloserMaterialOptions));
            new PPatchManager(harmony).RegisterPatchClass(typeof(CloserMaterial));
            new PLocalization().Register();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            InfoData.PLAN_ICON_SPRITE = PUIUtils.LoadSprite("CloserMaterial.Tool.Images.image_plan_tool_button.png");
            InfoData.PLAN_ICON_SPRITE.name = InfoData.PLAN_ICON_NAME;
            InfoData.PLAN_VISUALIZER_SPRITE = PUIUtils.LoadSprite("CloserMaterial.Tool.Images.image_plan_tool_visualizer.png");
            InfoData.PLAN_OPENTOOL = new PActionManager().CreateAction("Plan.opentool", (LocString)"Plan", new PKeyBinding());

            if (Localization.GetCurrentLanguageCode().Equals("pt"))
            {
                LocString.CreateLocStringKeys(typeof(Info.pt.InfoDataStrings));
            }
            else
            {
                LocString.CreateLocStringKeys(typeof(Info.InfoDataStrings));
            }

            new PVersionCheck().Register((UserMod2)this, (IModVersionChecker)new SteamVersionChecker());

            InfoData.OPTIONS = POptions.ReadSettings<CloserMaterialOptions>() ?? new CloserMaterialOptions();
        }

        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public static class PlayerControllerOnPrefabInitPatch
        {
            public static void Postfix(PlayerController __instance)
            {
                InfoData.OPTIONS = POptions.ReadSettings<CloserMaterialOptions>() ?? new CloserMaterialOptions();
            }
        }
    }
}
