
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples.VideoQueuePlayerSystem
{
    public class VideoStreamer : UdonSharpBehaviour
    {
        private BaseVRCVideoPlayer baseVrcVideoPlayer;
        private VideoRequest videoRequest;

        private void Update()
        {
            if (videoRequest != null && videoRequest.IsFull())
            {
                if (!GetBaseVRCVideoPlayer().IsPlaying)
                {
                    VRCUrl url = videoRequest.GetUrl();
                    Debug.Log($"Attempting to PlayVideo: {url.Get()}");
                    GetBaseVRCVideoPlayer().PlayURL(url);
                    videoRequest = null;
                }
            }
        }

        public void SetVideoRequest(VideoRequest videoRequest) { this.videoRequest = videoRequest; }

        // Lazy loading caches
        private BaseVRCVideoPlayer GetBaseVRCVideoPlayer()
        { return baseVrcVideoPlayer != null ? baseVrcVideoPlayer : baseVrcVideoPlayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer)); }
    }
}