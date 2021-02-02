
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

                            if (HasVideoTimeMilliseconds(videoRequest))
                            {
                                int videoTimeMilliseconds = GetVideoTimeMilliseconds(videoRequest);
                                float videoTimeSeconds = videoTimeMilliseconds / 1000f;
                                if (videoTimeSeconds >= 0) { TryPlay(videoTimeSeconds); }
                            }
                        }
                        else
                        {
                            if (HasVideoTimeMilliseconds(videoRequest))
                            {
                                int videoTimeMilliseconds = GetVideoTimeMilliseconds(videoRequest);
                                float videoTimeSeconds = videoTimeMilliseconds / 1000f;

                                float videoPlayerTimeSeconds = GetBaseVRCVideoPlayer().GetTime();

                                float lag = Mathf.Abs(videoTimeSeconds - videoPlayerTimeSeconds);
                                // Debug.Log($"lag: {lag}");
                                // TODO: Test this
                                if (lag > 5f) { TrySync(videoTimeSeconds); }
                            }
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
        private bool HasVideoDurationMilliseconds() { return GetVideoDurationMilliseconds() != -1; }

        private int GetVideoStartTimeMilliseconds(VideoRequest videoRequest)
        { return videoRequest.HasEndTimeMilliseconds() && HasVideoDurationMilliseconds() ? videoRequest.GetEndTimeMilliseconds() - GetVideoDurationMilliseconds() : -1; }
        private bool HasVideoStartTimeMilliseconds(VideoRequest videoRequest) { return GetVideoStartTimeMilliseconds(videoRequest) != -1; }

        private int GetVideoTimeMilliseconds(VideoRequest videoRequest)
        { return HasVideoStartTimeMilliseconds(videoRequest) ? videoRequestManager.GetNetworkTimeMilliseconds() - GetVideoStartTimeMilliseconds(videoRequest) : -1; }
        private bool HasVideoTimeMilliseconds(VideoRequest videoRequest) { return GetVideoTimeMilliseconds(videoRequest) != -1; }
        // int videoStartTimeMilliseconds = videoRequest.GetEndTimeMilliseconds() - durationMilliseconds;
        // int videoTimeMilliseconds = videoRequestManager.GetNetworkTimeMilliseconds() - videoStartTimeMilliseconds;

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
            Debug.Log($"Forcing Play at time: {time}");
            GetBaseVRCVideoPlayer().SetTime(time);
            GetBaseVRCVideoPlayer().Play();
            playTimeLast = videoRequestManager.GetNetworkTimeMilliseconds();
        }

        private int syncTimeLast = -1;
        private const int syncTimeCooldownMilliseconds = 10000;
        private void TrySync(float time)
        {
            Debug.Log($"Trying to sync to time: {time}");
            if (GetBaseVRCVideoPlayer().IsPlaying)
            {
                int networkTime = videoRequestManager.GetNetworkTimeMilliseconds();
                if (syncTimeLast == -1 || (networkTime - syncTimeLast) >= syncTimeCooldownMilliseconds)
                { ForceSync(time); }
            }
        }
        private void ForceSync(float time)
        {
            Debug.Log($"Forcing Sync to time: {time}");
            GetBaseVRCVideoPlayer().SetTime(time);
            syncTimeLast = videoRequestManager.GetNetworkTimeMilliseconds();
        }

        // Lazy loading caches
        private BaseVRCVideoPlayer baseVrcVideoPlayer;
        private BaseVRCVideoPlayer GetBaseVRCVideoPlayer()
        { return baseVrcVideoPlayer != null ? baseVrcVideoPlayer : baseVrcVideoPlayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer)); }
    }
}