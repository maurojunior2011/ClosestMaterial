using System.Collections.Generic;

namespace CloserMaterial.Tool
{
    internal class PlanToolHoverCard : HoverTextConfiguration
    {
        public PlanToolHoverCard() => this.ToolName = (string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_NAME");

        public override void UpdateHoverElements(List<KSelectable> hoveredObjects)
        {
            HoverTextScreen instance = HoverTextScreen.Instance;
            HoverTextDrawer drawer = instance.BeginDrawing();
            drawer.BeginShadowBar();
            this.DrawTitle(instance, drawer);
            drawer.NewLine();
            drawer.DrawIcon(instance.GetSprite("icon_mouse_left"), 20);
            drawer.DrawText((string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_ACTION_DRAG"), this.Styles_Instruction.Standard);
            drawer.AddIndent(8);
            drawer.DrawIcon(instance.GetSprite("icon_mouse_right"), 20);
            drawer.DrawText((string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_ACTION_BACK"), this.Styles_Instruction.Standard);
            drawer.EndShadowBar();
            drawer.EndDrawing();
        }
    }
}
