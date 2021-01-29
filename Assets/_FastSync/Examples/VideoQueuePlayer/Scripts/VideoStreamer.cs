
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
        private VideoRequestManager videoRequestManager;

        /*public void MakeRequestVideoDetail(string videoDetailPath)
        {
            VideoDetail request = videoCollectionRoot.GetVideoDetail(videoDetailPath);
            if (request != null) { GetVideoRequestManager().MakeRequest(GetNetworkTime(), GetLocalUsername(), request.GetUrl()); }
            else { Debug.LogError($"[VideoQueuePlayer] VideoStreamer: Invalid videoDetailPath '{videoDetailPath}'."); }
        }*/

        private int GetNetworkTime() { return Networking.GetServerTimeInMilliseconds(); }
        private string GetLocalUsername() { return Networking.LocalPlayer.displayName; }

        // Lazy loading caches
        private BaseVRCVideoPlayer GetBaseVRCVideoPlayer()
        { return baseVrcVideoPlayer != null ? baseVrcVideoPlayer : baseVrcVideoPlayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer)); }
        private VideoRequestManager GetVideoRequestManager()
        { return videoRequestManager != null ? videoRequestManager : videoRequestManager = GetComponentInChildren<VideoRequestManager>(); }
    }
}