using CloserMaterial.Info;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Grid.Restriction;
using static Operational;

namespace CloserMaterial.Tool
{
    public sealed class PlanTool : FilteredDragTool
    {
        private List<int> IDs = new List<int>();
        public static PlanTool Instance { get; private set; }

        public PlanTool() => PlanTool.Instance = this;

        protected override void OnActivateTool()
        {
            base.OnActivateTool();
            ToolMenu.Instance.PriorityScreen.Show();
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);
            ToolMenu.Instance.PriorityScreen.Show(false);
        }

        public static void DestroyInstance() => PlanTool.Instance = (PlanTool)null;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.visualizer = new GameObject("PlanVisualizer");
            this.visualizer.SetActive(false);
            GameObject go = new GameObject();
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.color = InfoData.PLAN_COLOR_DRAG;
            spriteRenderer.sprite = InfoData.PLAN_VISUALIZER_SPRITE;
            go.transform.SetParent(this.visualizer.transform);
            go.transform.localPosition = new Vector3(0.0f, Grid.HalfCellSizeInMeters);
            Sprite sprite = spriteRenderer.sprite;
            go.transform.localScale = new Vector3(Grid.CellSizeInMeters / ((float)sprite.texture.width / sprite.pixelsPerUnit), Grid.CellSizeInMeters / ((float)sprite.texture.height / sprite.pixelsPerUnit));
            go.SetLayerRecursively(LayerMask.NameToLayer("Overlay"));
            this.visualizer.transform.SetParent(this.transform);
            FieldInfo fieldInfo1 = AccessTools.Field(typeof(DragTool), "areaVisualizer");
            FieldInfo fieldInfo2 = AccessTools.Field(typeof(DragTool), "areaVisualizerSpriteRenderer");
            GameObject gameObject1 = Util.KInstantiate((GameObject)AccessTools.Field(typeof(DeconstructTool), "areaVisualizer").GetValue((object)DeconstructTool.Instance));
            gameObject1.SetActive(false);
            gameObject1.name = "PlanAreaVisualizer";
            SpriteRenderer component = gameObject1.GetComponent<SpriteRenderer>();
            fieldInfo2.SetValue((object)this, (object)component);
            gameObject1.transform.SetParent(this.transform);
            gameObject1.GetComponent<SpriteRenderer>().color = InfoData.PLAN_COLOR_DRAG;
            gameObject1.GetComponent<SpriteRenderer>().material.color = InfoData.PLAN_COLOR_DRAG;
            GameObject gameObject2 = gameObject1;
            fieldInfo1.SetValue((object)this, (object)gameObject2);
            this.gameObject.AddComponent<PlanToolHoverCard>();
        }

        protected override void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
        {
            base.OnDragComplete(cursorDown, cursorUp);
            if (!this.hasFocus)
                return;

            IDs.Clear();
            
            int x1;
            int y1;
            Grid.PosToXY(cursorDown, out x1, out y1);
            int x2;
            int y2;
            Grid.PosToXY(cursorUp, out x2, out y2);
            if (x1 > x2)
                Util.Swap<int>(ref x1, ref x2);
            if (y1 > y2)
                Util.Swap<int>(ref y1, ref y2);
            for (int x3 = x1; x3 <= x2; ++x3)
            {
                for (int y3 = y1; y3 <= y2; ++y3)
                {
                    int cell1 = Grid.XYToCell(x3, y3);
                    if (Grid.IsVisible(cell1))
                    {
                        for (int layer = 0; layer < Grid.ObjectLayers.Length; ++layer)
                        {
                            GameObject input = Grid.Objects[cell1, layer];

                            if (input != null && !IDs.Contains(input.GetInstanceID()))
                            {
                                if (input != null && this.IsActiveLayer(this.GetFilterLayerFromGameObject(input)))
                                {
                                    //showDebugBasic(input, layer);

                                    var listComponentsInput = input.GetComponents<Building>();

                                    if (listComponentsInput != null && listComponentsInput.Count() > 0)
                                    {
                                        //showDebugBasic(input, layer);
                                        buildingPlan(input, cell1, layer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private GameObject BuildBuilding(int cell, BuildingDef def, Orientation orientation, IList<Tag> elements, Vector3 posInput, KBatchedAnimController animControllerInput, bool replace)
        {

            //showDebugMethod("BuildBuilding", elements[0], def);

            GameObject Visualizer;

            Visualizer = def.Instantiate(posInput, orientation, elements);
            if (Visualizer == null)
                return Visualizer;

            Visualizer.transform.SetPosition(posInput);
            if (Visualizer.GetComponent<Rotatable>() != null)
                Visualizer.GetComponent<Rotatable>().SetOrientation(orientation);
            KBatchedAnimController component = Visualizer.GetComponent<KBatchedAnimController>();
            if (component != null)
            {
                component.visibilityType = KAnimControllerBase.VisibilityType.Always;
                component.isMovable = false;
                component.Offset = animControllerInput.Offset;
                component.TintColour = UnityEngine.Color.green;
                component.SetLayer(LayerMask.NameToLayer("Place"));
            }
            else
                Visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));

            if (ToolMenu.Instance != null)
                Visualizer.FindOrAddComponent<Prioritizable>().SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());

            Visualizer.SetActive(true);

            return Visualizer;
        }

        private GameObject BuildUtility(int cell, BuildingDef def, Orientation orientation, IList<Tag> elements, Vector3 posInput, KBatchedAnimController animControllerInput, bool replace)
        {
            //showDebugMethod("BuildUtility", elements[0], def);

            GameObject Visualizer;
            UtilityConnections flag = UtilityConnections.Down;
            IUtilityNetworkMgr networkManager = null;
            if (def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>() != null)
            {
                if (def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager() != null)
                {
                    networkManager = def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager();
                    flag = networkManager.GetConnections(cell, false);
                }
            }

            Constructable componentC = def.BuildingUnderConstruction.GetComponent<Constructable>();

            if (replace) componentC.IsReplacementTile = true;

            Visualizer = def.Instantiate(posInput, orientation, elements);

            if (replace) componentC.IsReplacementTile = true;

            if (Visualizer == null)
            {
                return Visualizer;
            }
            Visualizer.transform.SetPosition(posInput);
            if (Visualizer.GetComponent<Rotatable>() != null)
            {

                Visualizer.GetComponent<Rotatable>().SetOrientation(orientation);
            }
            KBatchedAnimController component = Visualizer.GetComponent<KBatchedAnimController>();
            if (component != null)
            {
                if (networkManager != null)
                {
                    string anim_name = networkManager.GetVisualizerString(flag) + "_place";
                    if (component.HasAnimation((HashedString)anim_name))
                    {
                        component.Play((HashedString)anim_name);
                    }
                }
                component.visibilityType = KAnimControllerBase.VisibilityType.Always;
                component.isMovable = false;
                component.Offset = animControllerInput.Offset;
                component.TintColour = UnityEngine.Color.green;
                component.SetLayer(LayerMask.NameToLayer("Place"));
            }
            else
            {
                Visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
            }

            if (def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>() != null && Visualizer.GetComponent<KAnimGraphTileVisualizer>() != null)
            {
                if (replace)
                {
                    Visualizer.GetComponent<KAnimGraphTileVisualizer>().Connections = flag;
                    Visualizer.GetComponent<KAnimGraphTileVisualizer>().UpdateConnections(flag);
                }
                else
                {
                    Visualizer.GetComponent<KAnimGraphTileVisualizer>().Connections = flag;
                    Visualizer.GetComponent<KAnimGraphTileVisualizer>().UpdateConnections(flag);
                    int connectionCount = 0;
                    if (componentC.IconConnectionAnimation(0.1f * (float)connectionCount, connectionCount, "Wire", "OutletConnected_release") || componentC.IconConnectionAnimation(0.1f * (float)connectionCount, connectionCount, "Pipe", "OutletConnected_release"))
                    {
                    }
                }
            }

            if (ToolMenu.Instance != null)
            {
                Visualizer.FindOrAddComponent<Prioritizable>().SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
            }
            Visualizer.SetActive(true);

            return Visualizer;
        }

        private GameObject BuildTile(int cell, BuildingDef def, Orientation orientation, IList<Tag> elements, Vector3 posInput, KBatchedAnimController animControllerInput, bool replace)
        {
            //showDebugMethod("BuildTile", elements[0], def);

            GameObject Visualizer;

            Constructable componentC = def.BuildingUnderConstruction.GetComponent<Constructable>();

            if (replace) componentC.IsReplacementTile = true;

            Visualizer = def.Instantiate(posInput, orientation, elements);

            if (replace) componentC.IsReplacementTile = false;

            if (Visualizer == null)
                return Visualizer;

            Visualizer.transform.SetPosition(posInput);

            if (Visualizer.GetComponent<Rotatable>() != null)
                Visualizer.GetComponent<Rotatable>().SetOrientation(orientation);
            KBatchedAnimController component = Visualizer.GetComponent<KBatchedAnimController>();
            if (component != null)
            {
                component.visibilityType = KAnimControllerBase.VisibilityType.Always;
                component.isMovable = false;
                component.Offset = animControllerInput.Offset;
                component.TintColour = UnityEngine.Color.green;
            }

            if (def.isKAnimTile)
            {
                World.Instance.blockTileRenderer.RemoveBlock(def, replace, SimHashes.Void, cell);
                World.Instance.blockTileRenderer.AddBlock(LayerMask.NameToLayer("Overlay"), Visualizer.GetComponents<Building>()[0].Def, replace, SimHashes.Void, cell);
            }

            if (ToolMenu.Instance != null)
            {
                Visualizer.FindOrAddComponent<Prioritizable>().SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
            }

            Visualizer.SetActive(true);
            return Visualizer;
        }

        private void buildingPlan(GameObject input, int cell, int layer)
        {
            Constructable compConstructable = input.GetComponent<Constructable>();
            Cancellable compCancellable = input.GetComponent<Cancellable>();
            PrimaryElement compPrimaryElement = input.GetComponent<PrimaryElement>();

            if (compCancellable != null && compConstructable != null && compPrimaryElement != null)
            {
                foreach (var item in input.GetComponents<Building>())
                {
                    TileVisualizer.RefreshCell(cell, item.Def.TileLayer, item.Def.ReplacementLayer);
                }

                IList<Tag> lista = compConstructable.SelectedElementsTags;

                if (lista != null && lista.Count > 0)
                {
                    var listComponentsInput = input.GetComponents<Building>();

                    if (listComponentsInput.Any())
                    {
                        Vector3 posInput = input.transform.GetPosition();
                        KBatchedAnimController animControllerInput = input.GetComponent<KBatchedAnimController>();

                        //showDebug(input, layer, pos, listComponentsInput[0].Def);

                        IList<Tag> lista2 = Utils.CloserMaterial(listComponentsInput[0].Def, posInput, lista);

                        bool isReplace = layer == (int)listComponentsInput[0].Def.ReplacementLayer;
                        bool other = false;
                        if (isReplace)
                        {
                            //showDebug(input, layer, posInput, listComponentsInput[0].Def);
                            IList<Tag> lista3 = Grid.Objects[cell, (int)listComponentsInput[0].Def.ObjectLayer].GetComponent<Deconstructable>().constructionElements;

                            for (int i = 0; i < lista3.Count; i++)
                            {
                                if (lista2[i].GetHash() != lista3[i].GetHash())
                                {
                                    other = true;
                                }
                            }
                        }
                        else
                        {
                            other = true;
                        }

                        if (!lista2.SequenceEqual(lista))
                        {
                            if (other)
                            {
                                Orientation orientation = listComponentsInput[0].Orientation;

                                GameObject gameObject = null;

                                if (listComponentsInput != null && listComponentsInput.Count() > 0)
                                {
                                    compCancellable.Trigger((int)GameHashes.Cancel);

                                    if (listComponentsInput[0].Def.IsTilePiece)
                                    {
                                        if (listComponentsInput[0].Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>() != null)
                                            gameObject = BuildUtility(cell, listComponentsInput[0].Def, orientation, lista2, posInput, animControllerInput, isReplace);
                                        else
                                            gameObject = BuildTile(cell, listComponentsInput[0].Def, orientation, lista2, posInput, animControllerInput, isReplace);
                                    }
                                    else
                                        gameObject = BuildBuilding(cell, listComponentsInput[0].Def, orientation, lista2, posInput, animControllerInput, false);

                                    //showDebugBasic(gameObject, layer, cell);

                                    if (gameObject != null)
                                    {
                                        input.DeleteObject();

                                        Grid.Objects[cell, layer] = gameObject;

                                        foreach (var item in gameObject.GetComponents<Building>())
                                        {
                                            TileVisualizer.RefreshCell(cell, item.Def.TileLayer, item.Def.ReplacementLayer);
                                            item.RefreshCells();
                                        }

                                        ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);

                                        IDs.Add(input.GetInstanceID());
                                    }
                                }
                            }
                            else
                            {
                                compCancellable.Trigger((int)GameHashes.Cancel);
                                input.DeleteObject();
                            }
                        }
                    }
                }
            }
        }
    }
}
