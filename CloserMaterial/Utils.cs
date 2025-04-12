using CloserMaterial.Info;
using STRINGS;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace CloserMaterial
{
    public static class Utils
    {
        public static IList<Tag> CloserMaterial(BuildingDef __instance, Vector3 pos, IList<Tag> elements)
        {
            MyDebug.ShowMaterials(elements, __instance);

            if (!elements.Any()) return elements;

            IList<int> listDistances = new List<int>(elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                listDistances.Add(999999);
            }

            IList<Tag> newElements = new List<Tag>(elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                newElements.Add(elements[i]);
            }

            //MyDebug.ShowListTag(newElements);

            //ActiveWorld
            WorldContainer activeWorld = null;

            //Rocket
            WorldContainer rocketWorld = null;

            //All Rockets
            List<WorldContainer> rockets = null;

            if (ClusterManager.Instance.activeWorld.IsModuleInterior)
            {
                rocketWorld = ClusterManager.Instance.activeWorld;

                if(rocketWorld.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)
                {
                    activeWorld = ClusterManager.Instance.GetWorld(ClusterManager.Instance.GetMyParentWorldId());

                    rockets = ClusterManager.Instance.WorldContainers.Where(x => x.ParentWorldId == activeWorld.id && x.IsModuleInterior && x.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded && x.id != rocketWorld.id)?.ToList();
                }
            }
            else
            {
                activeWorld = ClusterManager.Instance.activeWorld;

                rockets = ClusterManager.Instance.WorldContainers.Where(x => x.ParentWorldId == activeWorld.id && x.IsModuleInterior && x.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)?.ToList();
            }

            List<(ICollection<Pickupable> P, WorldContainer W)> availableItens = new List<(ICollection<Pickupable>, WorldContainer)>();

            //activeWorld inventory
            if (activeWorld != null)
            {
                var temp = activeWorld.worldInventory.GetPickupables(GameTags.Pickupable);
                if(temp != null)
                {
                    availableItens.Add((temp, activeWorld));
                }
            }

            //rocketWorld inventory
            if (rocketWorld != null)
            {
                var temp = rocketWorld.worldInventory.GetPickupables(GameTags.Pickupable);
                if (temp != null)
                {
                    availableItens.Add((temp, rocketWorld));
                }
            }

            //rockets inventorys
            if(rockets!= null)
            {
                foreach (var rocket in rockets)
                {
                    var temp = rocket.worldInventory.GetPickupables(GameTags.Pickupable);
                    if (temp != null)
                    {
                        availableItens.Add((temp, rocket));
                    }
                }
            }

            //Discover closest material
            for (int i = 0; i < __instance.MaterialCategory.Length; i++)
            {
                MyDebug.ShowMessage("Material categoria " + i + " = " + __instance.MaterialCategory[i]);

                List<Tag> validMaterials = MaterialSelector.GetValidMaterials(__instance.MaterialCategory[i]);

                //if (__instance.MaterialCategory[i] == MATERIALS.BUILDINGFIBER)
                //{
                //    newElements[i] = "BasicFabric".ToTag();
                //}
                //else if (__instance.MaterialCategory[i] == MATERIALS.WOOD)
                //{
                //    newElements[i] = "WoodLog".ToTag();
                //}
                //else
                //{
                    foreach (var listItensAvailable in availableItens)
                    {
                        //for each element
                        foreach (Pickupable pickupable in listItensAvailable.P)
                        {
                            MyDebug.ShowMessage(pickupable.PrimaryElement.Element.GetMaterialCategoryTag().Name);

                            //game validation
                            if (pickupable.HasTag(GameTags.StoredPrivate))
                            {
                                continue;
                            }

                            if(validMaterials.Contains(pickupable.PrimaryElement.Element.tag))
                            //bool containsCategory = __instance.MaterialCategory[i].Equals(pickupable.PrimaryElement.Element.GetMaterialCategoryTag().Name);
                            //if (pickupable.PrimaryElement.Element.IsSolid && containsCategory)
                            {
                                int cellP = pickupable.GetCell();
                                int cellC = Grid.PosToCell(pos);

                                int d = Grid.GetCellDistance(cellP, cellC);

                                bool validation =
                                    (double)listItensAvailable.W.worldInventory.GetAmount(pickupable.PrimaryElement.Element.tag, false) >= (double)__instance.Mass[i];

                                //Get Closer Material
                                if (d < listDistances[i] && validation)
                                {
                                    listDistances[i] = d;
                                    newElements[i] = pickupable.PrimaryElement.Element.tag;
                                    //MyDebug.ShowElement(pickupable.PrimaryElement.Element);
                                }
                            }
                        }
                    }
                //}
            }

            MyDebug.ShowMaterials(newElements, __instance);

            return newElements;
        }
    }
}
