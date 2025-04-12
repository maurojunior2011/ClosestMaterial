using CloserMaterial.Info;
using HarmonyLib;
using STRINGS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(BuildingDef), "Instantiate")]
    public static class BuildingDefPatch_Instantiate
    {
        public static void Prefix(ref IList<Tag> selected_elements, ref Vector3 pos, BuildingDef __instance)
        {
            //MyDebug.ShowMessage(InfoData.ACTIVE.ToString());
            if (InfoData.ACTIVE && (selected_elements != null && selected_elements.Any()))
            {
                selected_elements = Utils.CloserMaterial(__instance, pos, selected_elements);
            }
        }
    }
}
