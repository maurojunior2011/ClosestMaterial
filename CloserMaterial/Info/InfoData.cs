using PeterHan.PLib.Actions;
using UnityEngine;

namespace CloserMaterial.Info
{
    public static class InfoData
    {
        public static string PLAN_TOOLNAME = "PlanTool";
        public static string PLAN_ICON_NAME = "PLAN.TOOL.ICON";
        public static Sprite PLAN_ICON_SPRITE;
        public static Sprite PLAN_VISUALIZER_SPRITE;
        public static PAction PLAN_OPENTOOL;
        public static Color PLAN_COLOR_DRAG = (Color)new Color32((byte)60, (byte)179, (byte)113, byte.MaxValue);
        public static readonly Color32 PLAN_COLOR = new Color32(0, 0, 0, 255);
        public const string PLANBUILDABLERAW_ID = "PlanBuildableRaw";
        public static readonly SimHashes PlanRaw = (SimHashes)Hash.SDBMLower(PLANBUILDABLERAW_ID);
        public static CloserMaterialOptions OPTIONS { get; set; } = new CloserMaterialOptions();
    }
}
