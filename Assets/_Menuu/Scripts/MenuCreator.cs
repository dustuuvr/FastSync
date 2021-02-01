
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.MenuuSystem
{
    public class MenuCreator : UdonSharpBehaviour
    {
        // TODO: Use const?
        // Constants
        public readonly int FONT_SIZE_H1 = 175;
        public readonly int FONT_SIZE_H2 = 150;
        public readonly float MIN_HEIGHT_H1 = 350;
        public readonly float MIN_HEIGHT_H2 = 300;
        public readonly float MIN_WIDTH_UTIL_BUTTON = 250;
        public readonly float MAX_EXPAND = 99999;

        [SerializeField] private GameObject menuPrefab;
        [SerializeField] private GameObject menuPanelPrefab;
        [SerializeField] private GameObject menuPanelHorizontalPrefab;
        [SerializeField] private GameObject menuScrollViewPrefab;
        [SerializeField] private GameObject menuButtonPrefab;

        public GameObject GetMenuPrefab() { return menuPrefab; }
        public GameObject GetMenuPanelPrefab() { return menuPanelPrefab; }
        public GameObject GetMenuPanelHorizontalPrefab() { return menuPanelHorizontalPrefab; }
        public GameObject GetMenuScrollViewPrefab() { return menuScrollViewPrefab; }
        public GameObject GetMenuButtonPrefab() { return menuButtonPrefab; }

        // Component Creation
        public Menu CreateMenu(UdonSharpBehaviour eventHandler)
        {
            Menu menu = VRCInstantiate(GetMenuPrefab()).GetComponent<Menu>();
            menu.SetEventHandler(eventHandler);
            menu.transform.SetParent(transform, false);
            return menu;
        }

        // Utility methods
        public void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}