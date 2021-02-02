
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoDetail : UdonSharpBehaviour
    {
        [SerializeField] private string title;
        [SerializeField] private VRCUrl url;
        private VideoCollection videoCollectionParent;
        private int id;

        public string GetTitle() { return title; }
        public VRCUrl GetUrl() { return url; }
        public int GetID() { return id; }

        protected void Start() { id = GetVideoCollectionParent().RegisterVideoDetail(this); }

        public VideoCollection GetVideoCollectionParent()
        { return videoCollectionParent != null ? videoCollectionParent : videoCollectionParent = GetVideoCollectionInDirectParent(); }
        private VideoCollection GetVideoCollectionInDirectParent() { return transform.parent != null ? transform.parent.GetComponent<VideoCollection>() : null; }
    }
}