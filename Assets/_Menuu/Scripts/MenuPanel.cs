
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.MenuuSystem
{
    public class MenuPanel : UdonSharpBehaviour
    {
        private Menu menu;
        public Menu GetMenu() { return menu; }
        public void SetMenu(Menu menu) { this.menu = menu; }

        public RectTransform GetRectTransform() { return GetComponent<RectTransform>(); }
        public LayoutElement GetLayoutElement() { return GetComponent<LayoutElement>(); }

        public void SetParent(RectTransform parent)
        {
            GetRectTransform().SetParent(parent, false);
            GetMenu().GetMenuCreator().Stretch(GetRectTransform());
        }
    }
}