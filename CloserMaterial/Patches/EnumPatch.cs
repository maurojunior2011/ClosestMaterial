using HarmonyLib;
using System;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(Enum), "ToString", new Type[] { })]
    class SimHashes_ToString
    {
        static bool Prefix(ref Enum __instance, ref string __result)
        {
            if (!(__instance is SimHashes)) return true;
            return !Utils.SimHashNameLookup.TryGetValue((SimHashes)__instance, out __result);
        }
    }

    [HarmonyPatch(typeof(Enum), nameof(Enum.Parse), new Type[] { typeof(Type), typeof(string), typeof(bool) })]
    class SimHashes_Parse
    {
        static bool Prefix(Type enumType, string value, ref object __result)
        {
            if (!enumType.Equals(typeof(SimHashes))) return true;
            return !Utils.ReverseSimHashNameLookup.TryGetValue(value, out __result);
        }
    }
}
