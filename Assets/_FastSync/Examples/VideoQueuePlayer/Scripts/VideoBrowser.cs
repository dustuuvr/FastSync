
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples.VideoQueuePlayerSystem
{
    public class VideoBrowser : UdonSharpBehaviour
    {
        [SerializeField] private VideoCollection videoCollectionRoot;
        // private string browserPath;
        private VideoCollection videoCollectionView;

        protected void Start()
        {
            SetVideoCollectionView(videoCollectionRoot);
        }

        protected void Update()
        {
            VideoCollection videoCollectionViewNew = null;
            if (Input.GetKeyDown(KeyCode.Alpha1)) { videoCollectionViewNew = Test(0); }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) { videoCollectionViewNew = Test(1); }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) { videoCollectionViewNew = Test(2); }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) { videoCollectionViewNew = Test(3); }
            else if (Input.GetKeyDown(KeyCode.Alpha5)) { videoCollectionViewNew = Test(4); }
            else if (Input.GetKeyDown(KeyCode.Backspace)) { videoCollectionViewNew = GetVideoCollectionView().GetVideoCollectionParent(); }

            if (videoCollectionViewNew!= null) { SetVideoCollectionView(videoCollectionViewNew); }
        }

        private VideoCollection Test(int index)
        {
            if (index < GetVideoCollectionView().GetVideoCollections().Length)
            { return GetVideoCollectionView().GetVideoCollections()[index]; }
            else { return null; }
        }

        /*public string GetBrowserPath() { return browserPath; }
        private void SetBrowserPath(string browserPath)
        {
            this.browserPath = browserPath;
            SetVideoCollectionView(string.IsNullOrEmpty(GetBrowserPath()) ? videoCollectionRoot : videoCollectionRoot.GetVideoCollection(GetBrowserPath()));
        }*/

        private VideoCollection GetVideoCollectionView() { return videoCollectionView != null ? videoCollectionView : videoCollectionView = videoCollectionRoot; }
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
        }
    }
}