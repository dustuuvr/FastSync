using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoCollection : UdonSharpBehaviour
    {
        [SerializeField] private string title;
        private VideoCollection[] videoCollections;
        private VideoDetail[] videoDetails;
        private VideoCollection videoCollectionParent;

        /*public VideoCollection GetVideoCollection(string path)
        {
            string[] pathParts = path.Split(',');
            if (pathParts.Length > 0)
            {
                string pathFirstPart = pathParts[0];
                if (pathParts.Length != 1)
                {
                    VideoCollection videoCollection = GetVideoCollectionByTitle(pathFirstPart);
                    if (videoCollection != null)
                    {
                        string pathRemaining = path.Remove(0, pathFirstPart.Length + 1);
                        return videoCollection.GetVideoCollection(pathRemaining);
                    }
                }
                else { return GetVideoCollectionByTitle(pathFirstPart); }
            }
            return null;
        }

        public VideoDetail GetVideoDetail(string path)
        {
            string[] pathParts = path.Split(',');
            if (pathParts.Length > 0)
            {
                string pathFirstPart = pathParts[0];
                if (pathParts.Length != 1)
                {
                    VideoCollection videoCollection = GetVideoCollectionByTitle(pathFirstPart);
                    if (videoCollection != null)
                    {
                        string pathRemaining = path.Remove(0, pathFirstPart.Length + 1);
                        return videoCollection.GetVideoDetail(pathRemaining);    
                    }
                }
                else { return GetVideoDetailByTitle(pathFirstPart); }
            }
            return null;
        }*/

        /*private VideoCollection GetVideoCollectionByTitle(string videoCollectionTitle)
        {
            foreach (VideoCollection videoCollection in GetVideoCollections()) { if (videoCollection.GetTitle() == videoCollectionTitle) { return videoCollection; } }
            return null;
        }

        private VideoDetail GetVideoDetailByTitle(string videoDetailTitle)
        {
            foreach (VideoDetail videoDetail in GetVideoDetails()) { if (videoDetail.GetTitle() == videoDetailTitle) { return videoDetail; } }
            return null;
        }*/

        public VideoCollection GetVideoCollectionByIndex(int index) { return index < GetVideoCollections().Length ? GetVideoCollections()[index] : null; }
        public VideoDetail GetVideoDetailByIndex(int index) { return index < GetVideoDetails().Length ? GetVideoDetails()[index] : null; }

        public string GetTitle() { return title; }

        // Lazy loading caches
        public VideoCollection[] GetVideoCollections()
        { return videoCollections != null ? videoCollections : videoCollections = GetVideoCollectionsInDirectChildren(); }
        private VideoCollection[] GetVideoCollectionsInDirectChildren()
        {
            int videoCollectionsInDirectChildrenCount = 0;
            foreach ( Transform child in transform) { if (child.GetComponent<VideoCollection>() != null) { videoCollectionsInDirectChildrenCount++; } }

            VideoCollection[] videoCollectionsInDirectChildren = new VideoCollection[videoCollectionsInDirectChildrenCount];
            int videoCollectionsInDirectChildrenNextIndex = 0;
            foreach (Transform child in transform)
            {
                VideoCollection videoCollectionInDirectChild = child.GetComponent<VideoCollection>();
                if (videoCollectionInDirectChild != null) { videoCollectionsInDirectChildren[videoCollectionsInDirectChildrenNextIndex++] = videoCollectionInDirectChild; }
            }
            return videoCollectionsInDirectChildren;
        }
        public VideoDetail[] GetVideoDetails()
        { return videoDetails != null ? videoDetails : videoDetails = GetVideoDetailsInDirectChildren(); }
        private VideoDetail[] GetVideoDetailsInDirectChildren()
        {
            int videoDetailsInDirectChildrenCount = 0;
            foreach (Transform child in transform) { if (child.GetComponent<VideoDetail>() != null) { videoDetailsInDirectChildrenCount++; } }

            VideoDetail[] videoDetailsInDirectChildren = new VideoDetail[videoDetailsInDirectChildrenCount];
            int videoDetailsInDirectChildrenNextIndex = 0;
            foreach (Transform child in transform)
            {
                VideoDetail videoDetailInDirectChild = child.GetComponent<VideoDetail>();
                if (videoDetailInDirectChild != null) { videoDetailsInDirectChildren[videoDetailsInDirectChildrenNextIndex++] = videoDetailInDirectChild; }
            }
            return videoDetailsInDirectChildren;
        }
        public VideoCollection GetVideoCollectionParent()
        { return videoCollectionParent != null ? videoCollectionParent : videoCollectionParent = GetVideoCollectionInDirectParent(); }
        private VideoCollection GetVideoCollectionInDirectParent() { return transform.parent != null ? transform.parent.GetComponent<VideoCollection>() : null; }
    }
}