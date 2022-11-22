using CloserMaterial.Tool;
using HarmonyLib;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(Game), "DestroyInstances")]
    public static class GamePatch
    {
        public static void Postfix() => PlanTool.DestroyInstance();
    }
}
