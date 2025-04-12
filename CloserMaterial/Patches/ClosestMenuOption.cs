using CloserMaterial.Info;
using Epic.OnlineServices.Platform;
using HarmonyLib;
using System.Reflection;
using System;
using UnityEngine;
using CloserMaterial.elements;
using System.Runtime.CompilerServices;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(MaterialSelectionPanel))]
    public class ClosestMenuOption
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(OnPrefabInit))]
        public static void OnPrefabInit(MaterialSelectionPanel __instance)
        {
            if (__instance != null)
            {
                ClosestMenuWidget widgetClosest = __instance.gameObject.AddOrGet<ClosestMenuWidget>();
                if (widgetClosest != null)
                {
                    widgetClosest.ChangeButtons(false);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(ConfigureScreen))]
        public static void ConfigureScreen(MaterialSelectionPanel __instance)
        {
            if (__instance != null)
            {
                ClosestMenuWidget widgetClosest = __instance.gameObject.AddOrGet<ClosestMenuWidget>();
                if(widgetClosest != null)
                {
                    widgetClosest.ChangeButtons(false);
                }
            }
        }
    }
}
