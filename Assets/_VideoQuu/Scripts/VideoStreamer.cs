
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoStreamer : UdonSharpBehaviour
    {
        private VideoRequest[] videoRequests;
        private VideoRequest videoRequestLast;

        // TODO: Just set current?
        public void SetVideoRequests(VideoRequest[] videoRequests) { this.videoRequests = videoRequests; }

        protected void Update()
        {
            VideoRequest videoRequest = videoRequests != null && videoRequests.Length > 0 ? videoRequests[0] : null;

            if (videoRequest != null)
            {

                // if (videoRequest != videoRequestLast) {  }

                if (!GetBaseVRCVideoPlayer().IsReady)
                {
                    TryLoad(videoRequest);
                }
                else
                {
                    if (!GetBaseVRCVideoPlayer().IsPlaying)
                    {
                        GetBaseVRCVideoPlayer().Play();
                    }
                }
            }
            else
            {
                // Stop player etc
            }

            videoRequestLast = videoRequest;
        }

        private int lastLoadAttemptTime = -1;
        private const int loadTimeCooldownMilliseconds = 10000;
        private void TryLoad(VideoRequest videoRequest)
        {
            int networkTime = videoRequest.GetVideoRequestManager().GetNetworkTimeMilliseconds();
            if (lastLoadAttemptTime == -1 || (networkTime - lastLoadAttemptTime) >= loadTimeCooldownMilliseconds)
            {
                VRCUrl url = videoRequest.GetUrl();
                Debug.Log($"Attempting to Load: {url}");
                GetBaseVRCVideoPlayer().LoadURL(url);
                lastLoadAttemptTime = networkTime;
            }
        }

        // Lazy loading caches
        private BaseVRCVideoPlayer baseVrcVideoPlayer;
        private BaseVRCVideoPlayer GetBaseVRCVideoPlayer()
        { return baseVrcVideoPlayer != null ? baseVrcVideoPlayer : baseVrcVideoPlayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer)); }

        /*private BaseVRCVideoPlayer baseVrcVideoPlayer;
        private VideoRequest videoRequest;
        private VideoRequest videoRequestLastPlayed;

        private int lastLoadAttemptTime = -1;
        private const int loadTimeCooldownMilliseconds = 10000;
        private void TryLoad()
        {
            if (VideoRequestIsPlayable())
            {
                int networkTime = videoRequest.GetVideoRequestManager().GetNetworkTime();
                if (lastLoadAttemptTime == -1 || (networkTime - lastLoadAttemptTime) >= loadTimeCooldownMilliseconds)
                {
                    VRCUrl url = videoRequest.GetUrl();
                    Debug.Log($"Attempting to Load: {url}");
                    GetBaseVRCVideoPlayer().LoadURL(url);
                    lastLoadAttemptTime = networkTime;
                }
            }
        }

        private int lastSyncAttemptTime = -1;
        private const int syncTimeCooldownMilliseconds = 10000;
        private void TrySync()
        {
            if (VideoRequestIsPlayable())
            {
                float videoPlayerTimeSeconds = GetBaseVRCVideoPlayer().GetTime();
                float videoRequestTimeSinceStartedSeconds = videoRequest.GetTimeSinceStartedSeconds();
                float lag = Mathf.Abs(videoPlayerTimeSeconds - videoRequestTimeSinceStartedSeconds);
                if (lag > 2f)
                {
                    int networkTime = videoRequest.GetVideoRequestManager().GetNetworkTime();
                    if (lastSyncAttemptTime == -1 || (networkTime - lastSyncAttemptTime) >= syncTimeCooldownMilliseconds)
                    {
                        GetBaseVRCVideoPlayer().SetTime(videoRequestTimeSinceStartedSeconds);
                        Debug.Log($"Lag ({lag}) too high, Syncing...!");
                        lastSyncAttemptTime = networkTime;
                    }
                }
            }
        }

        private bool VideoRequestIsPlayable() { return videoRequest != null && videoRequest.IsFull(); }

        private void Update()
        {
            if (VideoRequestIsPlayable())
            {
                if (videoRequest != videoRequestLastPlayed)
                {
                    if (GetBaseVRCVideoPlayer().IsPlaying) { GetBaseVRCVideoPlayer().Stop(); }

                    if (!GetBaseVRCVideoPlayer().IsReady) { TryLoad(); }
                    else
                    {
                        // TODO: Add delay here
                        bool videoRequestShouldBePlaying = videoRequest.GetTimeSinceRequestedSeconds() > 0;

                        if (videoRequestShouldBePlaying)
                        {
                            videoRequest.TrySetStartTime();
                            GetBaseVRCVideoPlayer().Play();
                            videoRequestLastPlayed = videoRequest;
                        }
                        else
                        {
                            // TODO: Update UI here LOADING...
                        }
                    }
                }
                else
                {
                    if (GetBaseVRCVideoPlayer().IsPlaying) { TrySync(); }
                }
            }
        }

        public void SetVideoRequest(VideoRequest videoRequest) { this.videoRequest = videoRequest; }

        // Lazy loading caches
        private BaseVRCVideoPlayer GetBaseVRCVideoPlayer()
        { return baseVrcVideoPlayer != null ? baseVrcVideoPlayer : baseVrcVideoPlayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer)); }*/
    }
}