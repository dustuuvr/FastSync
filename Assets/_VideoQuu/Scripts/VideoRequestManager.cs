
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoRequestManager : UdonSharpBehaviour
    {
        [SerializeField] private VideoStreamer videoStreamer;
        private VideoRequest[] videoRequests;

        public void MakeRequest(VRCUrl url)
        {
            VideoRequest randomUnclaimedVideoRequest = GetRandomUnclaimedVideoRequest();
            if (randomUnclaimedVideoRequest != null) { randomUnclaimedVideoRequest.RequestURL(url); }
            else { Debug.LogError($"[VideoQueuePlayer] VideoRequestManager: Failed to run MakeRequest({url.Get()})."); }
        }

        public void OnVideoRequestChanged()
        {
            Debug.Log("Updating / sorting video requests!");
            GetVideoStreamer().SetVideoRequests(GetSortedValidVideoRequests());
        }

        // Helper functions to organize VideoRequests
        private VideoRequest[] GetSortedValidVideoRequests()
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

        public VideoStreamer GetVideoStreamer() { return videoStreamer; }

        // Lazy loading caches
        private VideoRequest[] GetVideoRequests()
        { return videoRequests != null ? videoRequests : videoRequests = GetComponentsInChildren<VideoRequest>(); }

        // TODO: Move these into a central utility location
        public int GetNetworkTimeMilliseconds() { return Networking.GetServerTimeInMilliseconds(); }
        public string GetLocalUsername() { return Networking.LocalPlayer.displayName; }
    }
}