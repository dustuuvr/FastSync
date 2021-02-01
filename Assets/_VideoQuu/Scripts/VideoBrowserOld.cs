
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoBrowserOld : UdonSharpBehaviour
    {
        /*
        [SerializeField] private VideoRequestManager videoRequestManager;
        [SerializeField] private VideoCollection videoCollectionRoot;
        // private string browserPath;
        private VideoCollection videoCollectionView;

        protected void Start()
        {
            SetVideoCollectionView(videoCollectionRoot);
        }

        protected void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                VideoDetail videoToPlay = null;
                if (Input.GetKeyDown(KeyCode.Alpha1)) { videoToPlay = VD(0); }
                else if (Input.GetKeyDown(KeyCode.Alpha2)) { videoToPlay = VD(1); }
                else if (Input.GetKeyDown(KeyCode.Alpha3)) { videoToPlay = VD(2); }
                else if (Input.GetKeyDown(KeyCode.Alpha4)) { videoToPlay = VD(3); }
                else if (Input.GetKeyDown(KeyCode.Alpha5)) { videoToPlay = VD(4); }
                if ( videoToPlay != null ) { videoRequestManager.MakeRequest(videoToPlay.GetUrl()); }
            }
            else
            {
                VideoCollection videoCollectionViewNew = null;
                if (Input.GetKeyDown(KeyCode.Alpha1)) { videoCollectionViewNew = VC(0); }
                else if (Input.GetKeyDown(KeyCode.Alpha2)) { videoCollectionViewNew = VC(1); }
                else if (Input.GetKeyDown(KeyCode.Alpha3)) { videoCollectionViewNew = VC(2); }
                else if (Input.GetKeyDown(KeyCode.Alpha4)) { videoCollectionViewNew = VC(3); }
                else if (Input.GetKeyDown(KeyCode.Alpha5)) { videoCollectionViewNew = VC(4); }
                else if (Input.GetKeyDown(KeyCode.Backspace)) { videoCollectionViewNew = GetVideoCollectionView().GetVideoCollectionParent(); }
                if (videoCollectionViewNew != null) { SetVideoCollectionView(videoCollectionViewNew); }
            }
        }

        private VideoCollection VC(int index)
        {
            if (index < GetVideoCollectionView().GetVideoCollections().Length)
            { return GetVideoCollectionView().GetVideoCollections()[index]; }
            else { return null; }
        }

        private VideoDetail VD(int index)
        {
            if (index < GetVideoCollectionView().GetVideoDetails().Length)
            { return GetVideoCollectionView().GetVideoDetails()[index]; }
            else { return null; }
        }*/

        /*public string GetBrowserPath() { return browserPath; }
        private void SetBrowserPath(string browserPath)
        {
            this.browserPath = browserPath;
            SetVideoCollectionView(string.IsNullOrEmpty(GetBrowserPath()) ? videoCollectionRoot : videoCollectionRoot.GetVideoCollection(GetBrowserPath()));
        }*/

        /*private VideoCollection GetVideoCollectionView() { return videoCollectionView != null ? videoCollectionView : videoCollectionView = videoCollectionRoot; }
        private void SetVideoCollectionView(VideoCollection videoCollectionView)
        {
            this.videoCollectionView = videoCollectionView;
            Debug.Log($"Changed to VideoCollection: {GetVideoCollectionView().GetTitle()}");

            VideoCollection[] videoCollectionViewVideoCollections = GetVideoCollectionView().GetVideoCollections();
            Debug.Log($"List of sub-collections (Count: {videoCollectionViewVideoCollections.Length}):");
            for (int i = 0; i < videoCollectionViewVideoCollections.Length; i++)
            { Debug.Log($"[{i+1}]: {videoCollectionViewVideoCollections[i].GetTitle()}"); }

            VideoDetail[] videoCollectionViewVideoDetails = GetVideoCollectionView().GetVideoDetails();
            Debug.Log($"List of video details (Count: {videoCollectionViewVideoDetails.Length}):");
            for (int i = 0; i < videoCollectionViewVideoDetails.Length; i++)
            { Debug.Log($"[{i + 1}]: {videoCollectionViewVideoDetails[i].GetTitle()}"); }
        }*/
    }
}