using CloserMaterial.Info;
using CloserMaterial.Tool;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CloserMaterial.Patches
{
    [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
    public static class PlayerControllerPatch
    {
        public static void Postfix(PlayerController __instance)
        {
            List<InterfaceTool> interfaceToolList = new List<InterfaceTool>((IEnumerable<InterfaceTool>)__instance.tools);
            GameObject gameObject = new GameObject(InfoData.PLAN_TOOLNAME);
            gameObject.AddComponent<PlanTool>();
            gameObject.transform.SetParent(__instance.gameObject.transform);
            gameObject.gameObject.SetActive(true);
            gameObject.gameObject.SetActive(false);
            interfaceToolList.Add(gameObject.GetComponent<InterfaceTool>());
            __instance.tools = interfaceToolList.ToArray();
        }
    }
}
