
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.MenuuSystem
{
    public class MenuButton : UdonSharpBehaviour
    {
        private Menu menu;
        public Menu GetMenu() { return menu; }
        public void SetMenu(Menu menu) { this.menu = menu; }

        public RectTransform GetRectTransform() { return GetComponent<RectTransform>(); }
        public LayoutElement GetLayoutElement() { return GetComponent<LayoutElement>(); }
        public Button GetButton() { return GetComponent<Button>(); }
        private Text GetChildText() { return GetComponentInChildren<Text>(); } // TODO: Enforce single

        public void SetParent(RectTransform parent) { GetRectTransform().SetParent(parent, false); }

        // TODO: Make this an object[] for more flexability? Have a seperate "clickType" variable for the first string
        private string[] clickData;
        // This method is called from the Button component via SendCustomEvent
        public void OnClick() { GetMenu().HandleClick(this); }

        public bool IsInteractable() { return GetButton().interactable; }
        public void SetInteractable(bool interactable) { GetButton().interactable = interactable; }

        public void SetText(string text) { GetChildText().text = text; }
        public string GetText() { return GetChildText().text; }
        public void SetTextColor(Color textColor) { GetChildText().color = textColor; }
        public void SetFontSize(int fontSize) { GetChildText().fontSize = fontSize; }

        public string[] GetClickData() { return clickData; }
        public void SetClickData(string[] clickData) { this.clickData = clickData; }
    }
}