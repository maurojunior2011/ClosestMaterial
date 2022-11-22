using CloserMaterial.Info;
using HarmonyLib;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
    public class AssetPatch
    {
        static void Prefix()
        {
            Utils.RegisterElementStrings(InfoData.PLANBUILDABLERAW_ID);
        }

        static void Postfix()
        {
            Utils.RegisterElementsRaw();
        }
    }
}
