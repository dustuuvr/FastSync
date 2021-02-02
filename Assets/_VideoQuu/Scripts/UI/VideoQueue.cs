
using Dustuu.VRChat.Uutils.MenuuSystem;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem.UI
{
    public class VideoQueue : UdonSharpBehaviour
    {
        // TODO: Use const?
        // Constants
        private readonly string CLICK_TYPE_NONE = "None";
        private readonly string CLICK_TYPE_QUEUE_ITEM_SKIP = "QueueItemSkip";
        private readonly string SKIP_BUTTON_TEXT = "X";

        [SerializeField] MenuCreator menuuCreator;
        [SerializeField] private VideoRequestManager videoRequestManager;

        private Menu menuu;
        private MenuPanelHorizontal[] queueItemPanels;
        private MenuButton[] queueItemLabelButtons;
        private MenuButton[] queueItemSkipButtons;

        protected void Start()
        {
            videoRequestManager.AddEventSubscriberOnVideoRequestsChanged(this);

            RectTransform rectTransform = GetComponent<RectTransform>();
            menuu = menuuCreator.CreateMenu(this);

            // Main panel
            MenuPanel mainPanel = menuu.CreateMenuPanel(rectTransform);
            // Title
            MenuButton mainPanelLabelButton = menuu.CreateMenuButton(mainPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            mainPanelLabelButton.SetText("Song Queue");
            mainPanelLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            mainPanelLabelButton.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H1;

            // Scroll view
            MenuScrollView queueViewScrollRect = menuu.CreateMenuScrollView(mainPanel.GetRectTransform());
            queueViewScrollRect.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;

            int queueItemsCount = videoRequestManager.MAX_ACTIVE_REQUESTS_TOTAL;
            queueItemPanels = new MenuPanelHorizontal[queueItemsCount];
            queueItemLabelButtons = new MenuButton[queueItemsCount];
            queueItemSkipButtons = new MenuButton[queueItemsCount];
            // Using for instead of foreach because List<> is not supported by U#
            for (int i = 0; i < queueItemsCount; i++)
            {
                MenuPanelHorizontal queueItemPanel = menuu.CreateMenuPanelHorizontal(queueViewScrollRect.GetContentRoot());
                queueItemPanels[i] = queueItemPanel;

                MenuButton queueItemLabelButton = menuu.CreateMenuButton(queueItemPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
                queueItemLabelButton.GetLayoutElement().preferredWidth = menuuCreator.MAX_EXPAND;
                queueItemLabelButtons[i] = queueItemLabelButton;

                MenuButton queueItemSkipButton = menuu.CreateMenuButton(queueItemPanel.GetRectTransform(), new string[] { CLICK_TYPE_QUEUE_ITEM_SKIP, i.ToString() });
                queueItemSkipButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;
                queueItemSkipButton.SetText(SKIP_BUTTON_TEXT);
                queueItemSkipButtons[i] = queueItemSkipButton;
            }

            RefreshUI();
        }

        // This is called via SendCustomEvent from VideoRequestManager
        public void OnVideoRequestsChanged() { SetVideoRequests(videoRequestManager.GetVideoRequestsSortedCache()); }

        // This is called via SendCustomEvent
        public void HandleClick()
        {
            // TODO: any reason to keep a reference to the button? just pop the click data directly?
            MenuButton pendingClick = menuu.PopMenuButtonPending();
            string[] clickData = pendingClick.GetClickData();

            if (clickData.Length > 0)
            {
                string clickType = clickData[0];

                if (clickType == CLICK_TYPE_QUEUE_ITEM_SKIP)
                {
                    int skipIndex = int.Parse(clickData[1]);
                    VideoRequest videoRequestToSkip = videoRequests[skipIndex];
                    Networking.SetOwner(Networking.LocalPlayer, videoRequestToSkip.gameObject);
                    videoRequestToSkip.ClearAttributes();
                }
            }
        }

        public void RefreshUI()
        {
            if (queueItemPanels == null)
            {
                Debug.LogError("Tried to call RefreshUI before Start");
                return;
            }

            for (int i = 0; i < videoRequestManager.MAX_ACTIVE_REQUESTS_TOTAL; i++)
            {
                MenuPanelHorizontal queueItemPanel = queueItemPanels[i];
                MenuButton queueItemLabelButton = queueItemLabelButtons[i];
                MenuButton queueItemSkipButton = queueItemSkipButtons[i];

                if (i < GetVideoRequests().Length)
                {
                    VideoRequest videoRequest = GetVideoRequests()[i];
                    queueItemLabelButton.SetText($"{videoRequest.GetUrl().Get()} ({videoRequest.GetRequestorUsername()})");
                    queueItemSkipButton.gameObject.SetActive(CanSkip(videoRequest));
                    queueItemPanel.gameObject.SetActive(true);
                }
                else { queueItemPanel.gameObject.SetActive(false); }
            }
        }

        // TODO: Add admin support
        private bool CanSkip(VideoRequest videoRequest)
        { return Networking.LocalPlayer.displayName == videoRequest.GetRequestorUsername() && Networking.IsOwner(videoRequest.gameObject); }

        private VideoRequest[] videoRequests;
        public VideoRequest[] GetVideoRequests() { return videoRequests != null ? videoRequests : videoRequests = new VideoRequest[0]; }
        public void SetVideoRequests(VideoRequest[] videoRequests)
        {
            this.videoRequests = videoRequests;
            RefreshUI();
        }
    }
}