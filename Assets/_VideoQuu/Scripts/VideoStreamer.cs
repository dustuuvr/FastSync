
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoStreamer : UdonSharpBehaviour
    {
        [SerializeField] private VideoRequestManager videoRequestManager;

        private VideoRequest[] videoRequests;
        private VideoRequest videoRequestLast;

        // This is called via SendCustomEvent from VideoRequestManager
        public void OnVideoRequestsChanged() { videoRequests = videoRequestManager.GetVideoRequestsSortedCache(); }

        protected void Start() { videoRequestManager.AddEventSubscriberOnVideoRequestsChanged(this); }

        protected void Update()
        {
            VideoRequest videoRequest = videoRequests != null && videoRequests.Length > 0 ? videoRequests[0] : null;

            if (videoRequest != null)
            {
                if (videoRequest != videoRequestLast) { ForceLoad(videoRequest); }
                else
                {
                    if (!GetBaseVRCVideoPlayer().IsReady)
                    {
                        TryLoad(videoRequest);
                    }
                    else
                    {
                        if (!GetBaseVRCVideoPlayer().IsPlaying)
                        {
                            if (Networking.IsOwner(videoRequest.gameObject) && !videoRequest.HasEndTimeMilliseconds())
                            {
                                int currentTimeMilliseconds = videoRequestManager.GetNetworkTimeMilliseconds();
                                // TODO: Add delay here
                                int durationMilliseconds = GetVideoDurationMilliseconds();
                                if (durationMilliseconds != -1)
                                {
                                    int endTimeMilliseconds = currentTimeMilliseconds + durationMilliseconds;
                                    Debug.Log($"Playing video at {currentTimeMilliseconds} for duration {durationMilliseconds} until end time {endTimeMilliseconds}");
                                    videoRequest.SetEndTimeMilliseconds(endTimeMilliseconds);
                                }
                            }

                            if (videoRequest.HasEndTimeMilliseconds())
                            {
                                int durationMilliseconds = GetVideoDurationMilliseconds();

                                if (durationMilliseconds != -1)
                                {
                                    int videoStartTimeMilliseconds = videoRequest.GetEndTimeMilliseconds() - durationMilliseconds;
                                    int videoTimeMilliseconds = videoRequestManager.GetNetworkTimeMilliseconds() - videoStartTimeMilliseconds;
                                    float videoTimeSeconds = videoTimeMilliseconds / 1000f;
                                    if (videoTimeSeconds >= 0) { TryPlay(videoTimeSeconds); }
                                }
                            }
                        }
                        else
                        {
                            // Sync / lag here
                        }
                    }
                }
            }
            else
            {
                GetBaseVRCVideoPlayer().Stop();
            }

            videoRequestLast = videoRequest;
        }

        private int GetVideoDurationMilliseconds()
        { return GetBaseVRCVideoPlayer().IsReady ? Mathf.RoundToInt(GetBaseVRCVideoPlayer().GetDuration() * 1000) : -1; }

        private int loadTimeLast = -1;
        private const int loadTimeCooldownMilliseconds = 10000;
        private void TryLoad(VideoRequest videoRequest)
        {
            int networkTime = videoRequestManager.GetNetworkTimeMilliseconds();
            if (loadTimeLast == -1 || (networkTime - loadTimeLast) >= loadTimeCooldownMilliseconds) { ForceLoad(videoRequest); }
        }
        private void ForceLoad(VideoRequest videoRequest)
        {
            int networkTime = videoRequestManager.GetNetworkTimeMilliseconds();
            VRCUrl url = videoRequest.GetUrl();
            Debug.Log($"Attempting to Load: {url.Get()}");
            GetBaseVRCVideoPlayer().LoadURL(url);
            loadTimeLast = networkTime;
        }

        private int playTimeLast = -1;
        private const int playTimeCooldownMilliseconds = 10000;
        private void TryPlay(float time)
        {
            if (GetBaseVRCVideoPlayer().IsReady && !GetBaseVRCVideoPlayer().IsPlaying)
            {
                int networkTime = videoRequestManager.GetNetworkTimeMilliseconds();
                if (playTimeLast == -1 || (networkTime - playTimeLast) >= playTimeCooldownMilliseconds)
                { ForcePlay(time); }
            }
            
        }
        private void ForcePlay(float time)
        {
            Debug.Log($"Attempting to Play at time: {time}");
            GetBaseVRCVideoPlayer().SetTime(time);
            GetBaseVRCVideoPlayer().Play();
            playTimeLast = videoRequestManager.GetNetworkTimeMilliseconds();
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