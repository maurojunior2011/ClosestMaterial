using CloserMaterial.Info;
using HarmonyLib;

namespace CloserMaterial.Patches
{
    public class WorldInventoryPatch
    {
        [HarmonyPatch(typeof(WorldInventory), "GetAmount")]
        public static class WorldInventory_GetAmount_Patch
        {
            public static void Postfix(Tag tag, ref float __result)
            {
                SimHashes simElemA = InfoData.PlanRaw;

                Element elementA = ElementLoader.FindElementByHash(simElemA);

                if (!DiscoveredResources.Instance.IsDiscovered(elementA.tag))
                {
                    DiscoveredResources.Instance.Discover(elementA.tag, elementA.GetMaterialCategoryTag());
                }

                if (tag.Name == elementA.tag.Name && DiscoveredResources.Instance.IsDiscovered(elementA.tag))
                {
                    __result = InfoData.OPTIONS.MaterialAvailable ? 9999 : 0;
                }
            }
        }
    }
}
