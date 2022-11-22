using CloserMaterial.Info;
using HarmonyLib;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(ToolMenu), "OnPrefabInit")]
    public static class ToolMenuPatch_OnPrefabInit
    {
        public static void Postfix()
        {
            if (Assets.Sprites.ContainsKey((HashedString)InfoData.PLAN_ICON_SPRITE.name))
                Assets.Sprites.Remove((HashedString)InfoData.PLAN_ICON_SPRITE.name);
            Assets.Sprites.Add((HashedString)InfoData.PLAN_ICON_SPRITE.name, InfoData.PLAN_ICON_SPRITE);
        }
    }

    [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
    public static class ToolMenuPatch_CreateBasicTools
    {
        public static void Prefix(ToolMenu __instance) => __instance.basicTools.Add(ToolMenu.CreateToolCollection((string)Strings.Get("STRINGS.InfoDataStrings.STRING_PLAN_NAME"), InfoData.PLAN_ICON_NAME, InfoData.PLAN_OPENTOOL.GetKAction(), InfoData.PLAN_TOOLNAME, (LocString)string.Format(Strings.Get("STRINGS.InfoDataStrings.STRING_PLAN_TOOLTIP"), (object)"{Hotkey}"), false));
    }
}
