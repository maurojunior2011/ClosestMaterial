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
using System;
using System.IO;
using System.Reflection;

namespace CloserMaterial
{
    public sealed class CloserMaterial : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new PPatchManager(harmony).RegisterPatchClass(typeof(CloserMaterial));
            PLocalization localization = new PLocalization();
            localization.Register();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            InfoData.PLAN_ICON_SPRITE = PUIUtils.LoadSprite("CloserMaterial.Tool.Images.image_plan_tool_button.png");
            InfoData.PLAN_ICON_SPRITE.name = InfoData.PLAN_ICON_NAME;
            InfoData.PLAN_VISUALIZER_SPRITE = PUIUtils.LoadSprite("CloserMaterial.Tool.Images.image_plan_tool_visualizer.png");
            InfoData.PLAN_OPENTOOL = new PActionManager().CreateAction("Plan.opentool", (LocString)"Plan", new PKeyBinding());
            //MyDebug.ShowMessage(Localization.GetCurrentLanguageCode());
            LocString.CreateLocStringKeys(typeof(INFODATASTRING));

            //string modPath = PUtil.GetModPath(executingAssembly);
            //string text = Localization.GetCurrentLanguageCode();
            //string path = Path.Combine(Path.Combine(modPath, "translations"), text + ".po");
            //MyDebug.ShowMessage(path);
            //try
            //{
            //    Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
            //}
            //catch (Exception ex)
            //{
            //    //MyDebug.ShowMessage(ex.Message);
            //}

            //if (Localization.GetCurrentLanguageCode().Equals("pt"))
            //{
            //    LocString.CreateLocStringKeys(typeof(Info.pt.InfoDataStrings));
            //}
            //else
            //{
            //    LocString.CreateLocStringKeys(typeof(Info.InfoDataStrings));
            //}

            new PVersionCheck().Register((UserMod2)this, (IModVersionChecker)new SteamVersionChecker());
        }
    }
}
