
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples.VideoQueuePlayerSystem
{
    public class VideoRequestManager : UdonSharpBehaviour
    {
        [SerializeField] private VideoStreamer videoStreamer;
        private VideoRequest[] videoRequests;
        private VideoRequest[] videoRequestsSorted;

        public void MakeRequest(VRCUrl url)
        {
            int time = GetNetworkTime();
            string username = GetLocalUsername();
            VideoRequest randomEmptyVideoRequest = GetRandomEmptyVideoRequest();
            if (randomEmptyVideoRequest != null) { randomEmptyVideoRequest.MakeRequest(time, username, url); }
            else { Debug.LogError($"[VideoQueuePlayer] VideoRequestManager: Failed to run MakeRequest({time}, {username}, {url.Get()})."); }
        }

        public void OnVideoRequestChanged()
        {
            // TODO: Only play if everything has finished loading in (int,string,url)
            videoRequestsSorted = GetSortedNonEmptyVideoRequests();
            VideoRequest currentVideoRequest = GetCurrentVideoRequest();
            if (currentVideoRequest != null) { currentVideoRequest.TryPlay(); }
            else { Debug.Log("currentVideoRequest is null"); }
        }

        // Helper functions to organize VideoRequests
        private VideoRequest GetCurrentVideoRequest() { return GetVideoRequestsSorted().Length > 0 ? GetVideoRequestsSorted()[0] : null; }
        private VideoRequest[] GetSortedNonEmptyVideoRequests()
        {
            VideoRequest[] nonEmptyVideoRequests = GetNonEmptyVideoRequests();
            VideoRequest temp;
            for (int i = 0; i <= nonEmptyVideoRequests.Length - 2; i++)
            {
                for (int ii = 0; ii <= nonEmptyVideoRequests.Length - 2; ii++)
                {
                    if (nonEmptyVideoRequests[ii].GetTime() > nonEmptyVideoRequests[ii + 1].GetTime())
                    {
                        temp = nonEmptyVideoRequests[ii + 1];
                        nonEmptyVideoRequests[ii + 1] = nonEmptyVideoRequests[ii];
                        nonEmptyVideoRequests[ii] = temp;
                    }
                }
            }
            return nonEmptyVideoRequests;
        }
        private VideoRequest GetRandomEmptyVideoRequest()
        {
            VideoRequest[] emptyVideoRequests = GetEmptyVideoRequests();
            Debug.Log($"emptyVideoRequests.Length: {emptyVideoRequests.Length}");
            int firstIndex = Random.Range(0, emptyVideoRequests.Length);
            for (int i = firstIndex; i < emptyVideoRequests.Length; i++) { if (emptyVideoRequests[i].IsEmpty()) { return emptyVideoRequests[i]; } }
            for (int i = 0; i < firstIndex; i++) { if (emptyVideoRequests[i].IsEmpty()) { return emptyVideoRequests[i]; } }
            return null;
        }
        private VideoRequest[] GetEmptyVideoRequests() { return GetVideoRequestsWithDesiredEmptyStatus(true); }
        private VideoRequest[] GetNonEmptyVideoRequests() { return GetVideoRequestsWithDesiredEmptyStatus(false); }
        private VideoRequest[] GetVideoRequestsWithDesiredEmptyStatus(bool desiredEmptyStatus)
        {
            int videoRequestsWithDesiredEmptyStatusCount = 0;
            Debug.Log($"GetVideoRequests().Length: { GetVideoRequests().Length}");
            foreach (VideoRequest videoRequest in GetVideoRequests()) { if (videoRequest.IsEmpty() == desiredEmptyStatus) { videoRequestsWithDesiredEmptyStatusCount++; } }
            Debug.Log($"videoRequestsWithDesiredEmptyStatusCount: { videoRequestsWithDesiredEmptyStatusCount}");

            VideoRequest[] videoRequestsWithDesiredEmptyStatus = new VideoRequest[videoRequestsWithDesiredEmptyStatusCount];
            int nonEmptyVideoRequestsNextIndex = 0;
            foreach (VideoRequest videoRequest in GetVideoRequests())
            { if (videoRequest.IsEmpty() == desiredEmptyStatus) { videoRequestsWithDesiredEmptyStatus[nonEmptyVideoRequestsNextIndex++] = videoRequest; } }
            return videoRequestsWithDesiredEmptyStatus;
        }

        public VideoStreamer GetVideoStreamer() { return videoStreamer; }

        // Lazy loading caches
        private VideoRequest[] GetVideoRequests()
        { return videoRequests != null ? videoRequests : videoRequests = GetComponentsInChildren<VideoRequest>(); }
        private VideoRequest[] GetVideoRequestsSorted()
        { return videoRequestsSorted != null ? videoRequestsSorted : videoRequestsSorted = new VideoRequest[0]; }

        // TODO: Move these into a central utility location
        public int GetNetworkTime() { return Networking.GetServerTimeInMilliseconds(); }
        public string GetLocalUsername() { return Networking.LocalPlayer.displayName; }
    }
}