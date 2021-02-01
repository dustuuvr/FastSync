
using Dustuu.VRChat.Uutils.MenuuSystem;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem.UI
{
    // TODO: Replace all label buttons with plain images/text
    public class VideoBrowser : UdonSharpBehaviour
    {
        // TODO: Use const?
        // Constants
        private readonly string CLICK_TYPE_NONE = "None";
        private readonly string CLICK_TYPE_BACK = "Back";
        private readonly string CLICK_TYPE_TOP = "Top";
        private readonly string CLICK_TYPE_VIDEO_COLLECTION = "VideoCollection";
        private readonly string CLICK_TYPE_VIDEO_DETAIL = "VideoDetail";

        private readonly string BACK_TEXT = "←";
        private readonly string TOP_TEXT = "↑";
        private readonly int MAX_SCROLL_ITEMS = 10;

        [SerializeField] MenuCreator menuuCreator;
        [SerializeField] private VideoRequestManager videoRequestManager;
        [SerializeField] private VideoCollection videoCollectionRoot;
        private VideoCollection videoCollectionView;

        private VideoCollection GetVideoCollectionView() { return videoCollectionView != null ? videoCollectionView : videoCollectionView = videoCollectionRoot; }
        private void SetVideoCollectionView(VideoCollection videoCollectionView)
        {
            this.videoCollectionView = videoCollectionView;
            RefreshUI();
        }

        private Menu menuu;
        private MenuButton backButton;
        private MenuButton categoryTitleButton;
        private MenuButton topButton;

        private MenuPanel videoCollectionPanel;
        private MenuButton[] videoCollectionButtons;
        private MenuPanel videoDetailPanel;
        private MenuButton[] videoDetailButtons;

        protected void Start()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            menuu = menuuCreator.CreateMenu(this);

            // Main panel
            MenuPanel mainPanel = menuu.CreateMenuPanel(rectTransform);
            // Title
            MenuButton mainPanelLabelButton = menuu.CreateMenuButton(mainPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            mainPanelLabelButton.SetText("Song Browser");
            mainPanelLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            mainPanelLabelButton.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H1;
            // Tools Panel
            MenuPanelHorizontal toolsPanel = menuu.CreateMenuPanelHorizontal(mainPanel.GetRectTransform());
            toolsPanel.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H2;
            // Back Button
            backButton = menuu.CreateMenuButton(toolsPanel.GetRectTransform(), new string[] { CLICK_TYPE_BACK });
            backButton.SetText(BACK_TEXT);
            backButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            backButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;
            // Category Title Button
            categoryTitleButton = menuu.CreateMenuButton(toolsPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            categoryTitleButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            categoryTitleButton.GetLayoutElement().preferredWidth = menuuCreator.MAX_EXPAND;
            // Top Button
            topButton = menuu.CreateMenuButton(toolsPanel.GetRectTransform(), new string[] { CLICK_TYPE_TOP });
            topButton.SetText(TOP_TEXT);
            topButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            topButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;

            // Video Collections
            videoCollectionPanel = menuu.CreateMenuPanel(mainPanel.GetRectTransform());
            videoCollectionPanel.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            MenuButton videoCollectionPanelLabelButton = menuu.CreateMenuButton(videoCollectionPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            videoCollectionPanelLabelButton.SetText("Folders");
            videoCollectionPanelLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            videoCollectionPanelLabelButton.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H1;
            MenuScrollView videoCollectionScrollView = menuu.CreateMenuScrollView(videoCollectionPanel.GetRectTransform());
            videoCollectionScrollView.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            videoCollectionButtons = new MenuButton[MAX_SCROLL_ITEMS];
            for (int i = 0; i < videoCollectionButtons.Length; i++)
            { videoCollectionButtons[i] = menuu.CreateMenuButton(videoCollectionScrollView.GetContentRoot(), new string[] { CLICK_TYPE_VIDEO_COLLECTION, i.ToString() }); }

            // Video Details
            videoDetailPanel = menuu.CreateMenuPanel(mainPanel.GetRectTransform());
            videoDetailPanel.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            MenuButton videoDetailPanelLabelButton = menuu.CreateMenuButton(videoDetailPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            videoDetailPanelLabelButton.SetText("Files");
            videoDetailPanelLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            videoDetailPanelLabelButton.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H1;
            MenuScrollView videoDetailScrollView = menuu.CreateMenuScrollView(videoDetailPanel.GetRectTransform());
            videoDetailScrollView.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            videoDetailButtons = new MenuButton[MAX_SCROLL_ITEMS];
            for (int i = 0; i < videoDetailButtons.Length; i++)
            { videoDetailButtons[i] = menuu.CreateMenuButton(videoDetailScrollView.GetContentRoot(), new string[] { CLICK_TYPE_VIDEO_DETAIL, i.ToString() }); }

            RefreshUI();
        }

        // This is called from UIManager via SendCustomEvent
        public void HandleClick()
        {
            // TODO: any reason to keep a reference to the button? just pop the click data directly?
            MenuButton pendingClick = menuu.PopMenuButtonPending();
            string[] clickData = pendingClick.GetClickData();

            if (clickData.Length > 0)
            {
                string clickType = clickData[0];

                if (clickType == CLICK_TYPE_VIDEO_COLLECTION)
                {
                    int videoCollectionIndex = int.Parse(clickData[1]);

                    VideoCollection videoCollectionClicked = GetVideoCollectionView().GetVideoCollectionByIndex(videoCollectionIndex);
                    if (videoCollectionClicked != null) { SetVideoCollectionView(videoCollectionClicked); }
                }
                else if (clickType == CLICK_TYPE_VIDEO_DETAIL)
                {
                    int videoDetailIndex = int.Parse(clickData[1]);

                    VideoDetail videoDetailClicked = GetVideoCollectionView().GetVideoDetailByIndex(videoDetailIndex);
                    if (videoDetailClicked != null) { videoRequestManager.MakeRequest(videoDetailClicked.GetUrl()); }
                }
            }
        }

        public void RefreshUI()
        {
            VideoCollection[] videoCollections = GetVideoCollectionView().GetVideoCollections();
            for (int i = 0; i < videoCollectionButtons.Length; i++)
            {
                MenuButton videoCollectionButton = videoCollectionButtons[i];
                if (i < videoCollections.Length)
                {
                    VideoCollection videoCollection = videoCollections[i];
                    videoCollectionButton.gameObject.SetActive(true);
                    videoCollectionButton.SetText(videoCollection.GetTitle());
                }
                else { videoCollectionButton.gameObject.SetActive(false); }
            }

            VideoDetail[] videoDetails = GetVideoCollectionView().GetVideoDetails();
            for (int i = 0; i < videoDetailButtons.Length; i++)
            {
                MenuButton videoDetailButton = videoDetailButtons[i];
                if (i < videoDetails.Length)
                {
                    VideoDetail videoDetail = videoDetails[i];
                    videoDetailButton.gameObject.SetActive(true);
                    videoDetailButton.SetText(videoDetail.GetTitle());
                }
                else { videoDetailButton.gameObject.SetActive(false); }
            }
        }
    }
}