
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.MenuuSystem
{
    public class Menu : UdonSharpBehaviour
    {
        private UdonSharpBehaviour eventHandler;
        public UdonSharpBehaviour GetEventHandler() { return eventHandler; }
        public void SetEventHandler(UdonSharpBehaviour eventHandler) { this.eventHandler = eventHandler; }

        private MenuButton menuButtonPending;
        public void HandleClick(MenuButton menuButtonClicked)
        {
            menuButtonPending = menuButtonClicked;
            GetEventHandler().SendCustomEvent("HandleClick");
        }

        public MenuButton PopMenuButtonPending()
        {
            MenuButton menuButtonPendingPopped = menuButtonPending;
            menuButtonPending = null;
            return menuButtonPendingPopped;
        }

        // Component Creation
        public MenuPanel CreateMenuPanel(RectTransform parent)
        {
            MenuPanel menuPanel = VRCInstantiate(GetMenuCreator().GetMenuPanelPrefab()).GetComponent<MenuPanel>();
            menuPanel.SetMenu(this);
            menuPanel.SetParent(parent);
            return menuPanel;
        }
        public MenuPanelHorizontal CreateMenuPanelHorizontal(RectTransform parent)
        {
            MenuPanelHorizontal menuPanelHorizontal = VRCInstantiate(GetMenuCreator().GetMenuPanelHorizontalPrefab()).GetComponent<MenuPanelHorizontal>();
            menuPanelHorizontal.SetMenu(this);
            menuPanelHorizontal.SetParent(parent);
            return menuPanelHorizontal;
        }
        public MenuScrollView CreateMenuScrollView(RectTransform parent)
        {
            MenuScrollView menuScrollView = VRCInstantiate(GetMenuCreator().GetMenuScrollViewPrefab()).GetComponent<MenuScrollView>();
            menuScrollView.SetMenu(this);
            menuScrollView.SetParent(parent);
            return menuScrollView;
        }
        public MenuButton CreateMenuButton(RectTransform parent, string[] clickData)
        {
            MenuButton menuButton = VRCInstantiate(GetMenuCreator().GetMenuButtonPrefab()).GetComponent<MenuButton>();
            menuButton.SetMenu(this);
            menuButton.SetClickData(clickData);
            menuButton.SetParent(parent);
            return menuButton;
        }

        // Lazy loading caches
        private MenuCreator menuCreator;
        public MenuCreator GetMenuCreator() { return menuCreator != null ? menuCreator : menuCreator = GetComponentInParent<MenuCreator>(); }
    }
}