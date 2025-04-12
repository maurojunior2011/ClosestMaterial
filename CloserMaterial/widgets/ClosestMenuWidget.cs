using CloserMaterial.Info;
using PeterHan.PLib.UI;
using UnityEngine;

namespace CloserMaterial.elements
{
    public class ClosestMenuWidget : KMonoBehaviour
    {
        private ColorStyleSetting unselected = new ColorStyleSetting();
        private ColorStyleSetting selected = new ColorStyleSetting();
        PButton buttonClosestElement;
        PButton buttonOptionUnselect;
        GameObject GOselect;
        GameObject GOunselect;

        protected override void OnPrefabInit()
        {
            unselected.Init(new Color(0.24f, 0.26f, 0.34f, 1f));
            selected.Init(new Color(0.50f, 0.55f, 0.70f, 1f));

            var margin = new RectOffset(4, 4, 4, 4);
            var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
            if (baseLayout != null)
                baseLayout.Params = new BoxLayoutParams()
                {
                    Direction = PanelDirection.Horizontal,
                    Margin = margin,
                };
            PPanel panel = new PPanel("MainPanelClosestElement")
            {
                Direction = PanelDirection.Horizontal,
                Margin = margin,
                Spacing = 4,
                FlexSize = Vector2.right
            };

            InfoData.ACTIVE = false;

            //TRUE
            buttonClosestElement = new PButton("ButtonClosestElement")
            {
                Text = (string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_NAME"),
                SpriteMode = UnityEngine.UI.Image.Type.Filled,
                DynamicSize = true,
                Color = unselected,
                OnClick = clickButtonSelect,
                ToolTip = (string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_TOOLTIP")
            };
            buttonClosestElement.AddOnRealize((obj) => GOselect = obj);


            //FALSE
            buttonOptionUnselect = new PButton("option")
            {
                Text = (string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_NAME"),
                SpriteMode = UnityEngine.UI.Image.Type.Filled,
                DynamicSize = true,
                Color = selected,
                OnClick = clickButtonUnselect,
                ToolTip = (string)Strings.Get("STRINGS.INFODATASTRING.MJ_CLOSESTMATERIAL.PLAN_TOOLTIP")
            };
            buttonOptionUnselect.AddOnRealize((obj) => GOunselect = obj);


            panel.AddChild(buttonClosestElement);
            panel.AddChild(buttonOptionUnselect);
            panel.AddTo(gameObject);

            GOselect.SetActive(value: true);
            GOunselect.SetActive(value: false);            

            base.OnPrefabInit();

        }

        private void clickButtonSelect(GameObject source)
        {
            ChangeButtons(true);
        }

        private void clickButtonUnselect(GameObject source)
        {
            ChangeButtons(false);
        }

        public void ChangeButtons(bool newState)
        {
            if(GOselect != null && GOunselect != null)
            {
                InfoData.ACTIVE = newState;
                GOselect.SetActive(value: !InfoData.ACTIVE);
                GOunselect.SetActive(value: InfoData.ACTIVE);
            }
        }
    }
}
