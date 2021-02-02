
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
        private readonly string CLICK_TYPE_VIDEO_COLLECTION = "VideoCollection";
        private readonly string CLICK_TYPE_VIDEO_COLLECTION_TOP = "VideoCollectionTop";
        private readonly string CLICK_TYPE_VIDEO_DETAIL = "VideoDetail";
        private readonly string CLICK_TYPE_VIDEO_DETAIL_TOP = "VideoDetailTop";

        private readonly string BACK_TEXT = "←";
        private readonly string TOP_TEXT = "↑";
        // TODO: Make this more dynamic
        private readonly int MAX_SCROLL_ITEMS = 40;

        [SerializeField] MenuCreator menuuCreator;
        [SerializeField] private VideoRequestManager videoRequestManager;
        [SerializeField] private VideoCollection videoCollectionRoot;
        private VideoCollection videoCollectionView;

        private VideoCollection GetVideoCollectionView() { return videoCollectionView != null ? videoCollectionView : videoCollectionView = videoCollectionRoot; }
        private void SetVideoCollectionView(VideoCollection videoCollectionView)
        {
            VideoCollection videoCollectionViewLast = GetVideoCollectionView();
            this.videoCollectionView = videoCollectionView;
            if (GetVideoCollectionView() != videoCollectionViewLast) { RefreshUI(); }
        }

        private Menu menuu;

        private MenuPanel videoCollectionPanel;
        private MenuButton videoCollectionBackButton;
        private MenuButton videoCollectionTopButton;
        private MenuScrollView videoCollectionScrollView;
        private MenuButton[] videoCollectionButtons;

        private MenuPanel videoDetailPanel;
        private MenuButton videoDetailBackButton;
        private MenuButton videoDetailTopButton;
        private MenuScrollView videoDetailScrollView;
        private MenuButton[] videoDetailButtons;

        protected void Start()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            menuu = menuuCreator.CreateMenu(this);

            // Main
            MenuPanel mainPanel = menuu.CreateMenuPanel(rectTransform);
            // Controls
            MenuPanelHorizontal mainControlsPanel = menuu.CreateMenuPanelHorizontal(mainPanel.GetRectTransform());
            mainControlsPanel.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H1;
            // Label
            MenuButton mainPanelLabelButton = menuu.CreateMenuButton(mainControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            mainPanelLabelButton.SetText("Song Browser");
            mainPanelLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            mainPanelLabelButton.GetLayoutElement().preferredWidth = menuuCreator.MAX_EXPAND;

            // Video Collections
            videoCollectionPanel = menuu.CreateMenuPanel(mainPanel.GetRectTransform());
            videoCollectionPanel.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            // Controls
            MenuPanelHorizontal videoCollectionControlsPanel = menuu.CreateMenuPanelHorizontal(videoCollectionPanel.GetRectTransform());
            videoCollectionControlsPanel.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H2;
            // Back Button
            videoCollectionBackButton = menuu.CreateMenuButton(videoCollectionControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_BACK });
            videoCollectionBackButton.SetText(BACK_TEXT);
            videoCollectionBackButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            videoCollectionBackButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;
            // Label
            MenuButton videoCollectionLabelButton = menuu.CreateMenuButton(videoCollectionControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            videoCollectionLabelButton.SetText("Folders");
            videoCollectionLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            videoCollectionLabelButton.GetLayoutElement().preferredWidth = menuuCreator.MAX_EXPAND;
            // Top Button
            videoCollectionTopButton = menuu.CreateMenuButton(videoCollectionControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_VIDEO_COLLECTION_TOP });
            videoCollectionTopButton.SetText(TOP_TEXT);
            videoCollectionTopButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            videoCollectionTopButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;
            
            // Scroll View
            videoCollectionScrollView = menuu.CreateMenuScrollView(videoCollectionPanel.GetRectTransform());
            videoCollectionScrollView.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            videoCollectionButtons = new MenuButton[MAX_SCROLL_ITEMS];
            for (int i = 0; i < videoCollectionButtons.Length; i++)
            { videoCollectionButtons[i] = menuu.CreateMenuButton(videoCollectionScrollView.GetContentRoot(), new string[] { CLICK_TYPE_VIDEO_COLLECTION, i.ToString() }); }

            // Video Details
            videoDetailPanel = menuu.CreateMenuPanel(mainPanel.GetRectTransform());
            videoDetailPanel.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            // Controls
            MenuPanelHorizontal videoDetailControlsPanel = menuu.CreateMenuPanelHorizontal(videoDetailPanel.GetRectTransform());
            videoDetailControlsPanel.GetLayoutElement().minHeight = menuuCreator.MIN_HEIGHT_H2;
            // Back Button
            videoDetailBackButton = menuu.CreateMenuButton(videoDetailControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_BACK });
            videoDetailBackButton.SetText(BACK_TEXT);
            videoDetailBackButton.SetFontSize(menuuCreator.FONT_SIZE_H1);
            videoDetailBackButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;
            // Label
            MenuButton videoDetailLabelButton = menuu.CreateMenuButton(videoDetailControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_NONE });
            videoDetailLabelButton.SetText("Files");
            videoDetailLabelButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            videoDetailLabelButton.GetLayoutElement().preferredWidth = menuuCreator.MAX_EXPAND;
            // Top Button
            videoDetailTopButton = menuu.CreateMenuButton(videoDetailControlsPanel.GetRectTransform(), new string[] { CLICK_TYPE_VIDEO_DETAIL_TOP });
            videoDetailTopButton.SetText(TOP_TEXT);
            videoDetailTopButton.SetFontSize(menuuCreator.FONT_SIZE_H2);
            videoDetailTopButton.GetLayoutElement().minWidth = menuuCreator.MIN_WIDTH_UTIL_BUTTON;
            // Scroll View
            videoDetailScrollView = menuu.CreateMenuScrollView(videoDetailPanel.GetRectTransform());
            videoDetailScrollView.GetLayoutElement().preferredHeight = menuuCreator.MAX_EXPAND;
            videoDetailButtons = new MenuButton[MAX_SCROLL_ITEMS];
            for (int i = 0; i < videoDetailButtons.Length; i++)
            { videoDetailButtons[i] = menuu.CreateMenuButton(videoDetailScrollView.GetContentRoot(), new string[] { CLICK_TYPE_VIDEO_DETAIL, i.ToString() }); }

            RefreshUI();
        }

        protected void Update()
        {
            videoCollectionTopButton.SetInteractable(videoCollectionScrollView.GetContentRoot().GetComponent<RectTransform>().localPosition.y > 0.1f);
            videoDetailTopButton.SetInteractable(videoDetailScrollView.GetContentRoot().GetComponent<RectTransform>().localPosition.y > 0.1f);
        }

        // This is called via SendCustomEvent
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
                    if (videoDetailClicked != null)
                    {
                        videoRequestManager.MakeRequest(videoDetailClicked);
                        SetVideoCollectionView(videoCollectionRoot);
                    }
                }
                else if (clickType == CLICK_TYPE_BACK)
                {
                    VideoCollection videoCollectionParent = GetVideoCollectionView().GetVideoCollectionParent();
                    if (videoCollectionParent != null) { SetVideoCollectionView(videoCollectionParent); }
                }
                else if (clickType == CLICK_TYPE_VIDEO_COLLECTION_TOP) { videoCollectionScrollView.ResetScrollRect(); }
                else if (clickType == CLICK_TYPE_VIDEO_DETAIL_TOP) { videoDetailScrollView.ResetScrollRect(); }
            }
        }

        public void RefreshUI()
        {
            VideoCollection[] videoCollections = GetVideoCollectionView().GetVideoCollections();
            if (videoCollections.Length > 0)
            {
                videoCollectionPanel.gameObject.SetActive(true);
                videoCollectionBackButton.SetInteractable(CanGoBack());
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
            }
            else { videoCollectionPanel.gameObject.SetActive(false); }
            videoCollectionScrollView.ResetScrollRect();

            VideoDetail[] videoDetails = GetVideoCollectionView().GetVideoDetails();
            if (videoDetails.Length > 0)
            {
                videoDetailPanel.gameObject.SetActive(true);
                videoDetailBackButton.SetInteractable(CanGoBack());
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
            else { videoDetailPanel.gameObject.SetActive(false); }
            videoDetailScrollView.ResetScrollRect();
        }

        private bool CanGoBack() { return GetVideoCollectionView().GetVideoCollectionParent() != null; }
    }
}