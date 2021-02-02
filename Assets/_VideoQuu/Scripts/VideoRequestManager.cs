
using Dustuu.VRChat.Uutils.VideoQuuSystem.UI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoRequestManager : UdonSharpBehaviour
    {
        [SerializeField] private VideoCollection videoCollectionRoot;
        public VideoCollection GetVideoCollectionRoot() { return videoCollectionRoot; }

        public readonly int MAX_ACTIVE_REQUESTS_TOTAL = 15;

        private VideoRequest[] videoRequests;
        private VideoRequest[] videoRequestsFilteredCache;

        public void MakeRequest(VideoDetail videoDetail)
        {
            VideoRequest randomUnclaimedVideoRequest = GetRandomUnclaimedVideoRequest();
            if (randomUnclaimedVideoRequest != null)
            {
                Networking.SetOwner(Networking.LocalPlayer, randomUnclaimedVideoRequest.gameObject);
                randomUnclaimedVideoRequest.RequestVideoDetail(videoDetail);
            }
            else { Debug.LogError($"[VideoQueuePlayer] VideoRequestManager: Failed to run MakeRequest({videoDetail.GetUrl().Get()})."); }
        }

        public void AddEventSubscriberOnVideoRequestsChanged(UdonSharpBehaviour eventSubscriberOnVideoRequestsChanged)
        {
            if (onVideoRequestsChangedSubscribersIndex >= GetOnVideoRequestsChangedSubscribers().Length)
            {
                Debug.LogError("Max onVideoRequestsChangedSubscribers reached");
                return;
            }
            if (eventSubscriberOnVideoRequestsChanged == null)
            {
                Debug.LogError("Can't add null subscriber!");
                return;
            }

            GetOnVideoRequestsChangedSubscribers()[onVideoRequestsChangedSubscribersIndex++] = eventSubscriberOnVideoRequestsChanged;
        }

        public VideoRequest[] GetVideoRequestsSortedCache() { return videoRequestsFilteredCache != null ? videoRequestsFilteredCache : videoRequestsFilteredCache = new VideoRequest[0]; }

        public void OnVideoRequestsChanged()
        {
            Debug.Log("Updating / sorting video requests!");
            videoRequestsFilteredCache = GetFilteredVideoRequests();
            for (int i = 0; i < GetOnVideoRequestsChangedSubscribers().Length && GetOnVideoRequestsChangedSubscribers()[i] != null; i++)
            { GetOnVideoRequestsChangedSubscribers()[i].SendCustomEvent("OnVideoRequestsChanged"); }
        }

        // Helper functions to organize VideoRequests
        private VideoRequest[] GetFilteredVideoRequests()
        {
            VideoRequest[] validVideoRequests = GetValidVideoRequests();
            VideoRequest temp;
            for (int i = 0; i <= validVideoRequests.Length - 2; i++)
            {
                for (int ii = 0; ii <= validVideoRequests.Length - 2; ii++)
                {
                    if (validVideoRequests[ii].GetRequestTimeMilliseconds() > validVideoRequests[ii + 1].GetRequestTimeMilliseconds())
                    {
                        temp = validVideoRequests[ii + 1];
                        validVideoRequests[ii + 1] = validVideoRequests[ii];
                        validVideoRequests[ii] = temp;
                    }
                }
            }
            return validVideoRequests;
        }
        private VideoRequest[] GetValidVideoRequests()
        {
            int validVideoRequestsCount = 0;
            foreach (VideoRequest videoRequest in GetVideoRequests()) { if (videoRequest.IsValid()) { validVideoRequestsCount++; } }
            VideoRequest[] validVideoRequests = new VideoRequest[validVideoRequestsCount];
            int validVideoRequestsNextIndex = 0;
            foreach (VideoRequest videoRequest in GetVideoRequests()) { if (videoRequest.IsValid()) { validVideoRequests[validVideoRequestsNextIndex++] = videoRequest; } }
            return validVideoRequests;
        }

        private VideoRequest GetRandomUnclaimedVideoRequest()
        {
            VideoRequest[] unclaimedVideoRequests = GetUnclaimedVideoRequests();
            if (unclaimedVideoRequests.Length > 0)
            {
                int unclaimedVideoRequestsRandomIndex = Random.Range(0, unclaimedVideoRequests.Length);
                return unclaimedVideoRequests[unclaimedVideoRequestsRandomIndex];
            }
            else { return null; }
        }
        private VideoRequest[] GetUnclaimedVideoRequests()
        {
            int unclaimedVideoRequestsCount = 0;
            foreach (VideoRequest videoRequest in GetVideoRequests()) { if (!videoRequest.IsClaimed()) { unclaimedVideoRequestsCount++; } }
            VideoRequest[] unclaimedVideoRequests = new VideoRequest[unclaimedVideoRequestsCount];
            int unclaimedVideoRequestsNextIndex = 0;
            foreach (VideoRequest videoRequest in GetVideoRequests()) { if (!videoRequest.IsClaimed()) { unclaimedVideoRequests[unclaimedVideoRequestsNextIndex++] = videoRequest; } }
            return unclaimedVideoRequests;
        }

        // Lazy loading caches
        private VideoRequest[] GetVideoRequests()
        { return videoRequests != null ? videoRequests : videoRequests = GetComponentsInChildren<VideoRequest>(); }

        // TODO: Use this style of event subscription in FastSync instead of SerializedFields
        private UdonSharpBehaviour[] onVideoRequestsChangedSubscribers;
        private int onVideoRequestsChangedSubscribersIndex = 0;
        private UdonSharpBehaviour[] GetOnVideoRequestsChangedSubscribers()
        { return onVideoRequestsChangedSubscribers != null ? onVideoRequestsChangedSubscribers : onVideoRequestsChangedSubscribers = new UdonSharpBehaviour[10]; }

        // TODO: Move these into a central utility location
        public int GetNetworkTimeMilliseconds() { return Networking.GetServerTimeInMilliseconds(); }
        public string GetLocalUsername() { return Networking.LocalPlayer.displayName; }
    }
}