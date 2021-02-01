
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.VideoQuuSystem
{
    public class VideoRequest : UdonSharpBehaviour
    {
        // Variables required to view
        [UdonSynced] private bool isClaimed;
        [UdonSynced] private int requestTimeMilliseconds;
        [UdonSynced] private string requestorUsername;
        [UdonSynced] private VRCUrl url = VRCUrl.Empty;
        // Variables required to play
        [UdonSynced] private int startTimeMilliseconds;
        [UdonSynced] private int durationMilliseconds;
        // Other
        private bool changesRegistered;
        // Caches
        private VideoRequestManager videoRequestManager;

        protected void Start()
        {
            ClearAttributes();
        }

        public override void OnDeserialization()
        {
            Debug.Log("OnDeserialization() run");
            HandleChanges();
        }

        private void HandleChanges()
        {
            if (!changesRegistered && IsValid())
            {
                GetVideoRequestManager().OnVideoRequestsChanged();
                changesRegistered = true;
            }
        }

        private void ClearAttributes()
        {
            isClaimed = false;
            requestTimeMilliseconds = -1;
            requestorUsername = null;
            url = VRCUrl.Empty;
            startTimeMilliseconds = -1;
            durationMilliseconds = -1;
            changesRegistered = false;
            HandleChanges();
        }

        public bool IsValid()
        { return isClaimed = true && requestTimeMilliseconds != -1 && !string.IsNullOrEmpty(requestorUsername) && url != VRCUrl.Empty; }

        public void RequestURL(VRCUrl requestUrl)
        {
            if (!isClaimed)
            {
                // Traditional Syncing
                SetOwnerLocal(gameObject);
                isClaimed = true;
                requestTimeMilliseconds = GetVideoRequestManager().GetNetworkTimeMilliseconds();
                requestorUsername = GetVideoRequestManager().GetLocalUsername();
                url = requestUrl;
                HandleChanges();
            }
            else { Debug.LogError("[VideoQueuePlayer] VideoRequest: Tried to call RequestURL but was claimed."); }
        }

        public bool IsClaimed() { return isClaimed; }
        public int GetRequestTimeMilliseconds() { return requestTimeMilliseconds; }
        public string GetRequestorUsername() { return requestorUsername; }
        public VRCUrl GetUrl() { return url; }

        public bool IsStarted() { return startTimeMilliseconds != -1 && GetVideoRequestManager().GetNetworkTimeMilliseconds() >= startTimeMilliseconds; }
        public int GetTimeSinceStartedMilliseconds()
        {
            if (IsStarted()) { return GetVideoRequestManager().GetNetworkTimeMilliseconds() - startTimeMilliseconds; }
            else
            {
                Debug.LogError("[VideoQueuePlayer] VideoRequest: Tried to call GetTimeSinceStartedMilliseconds but was not Started.");
                return 0;
            }
        }
        public bool IsEnded() { return durationMilliseconds != -1 && GetTimeSinceStartedMilliseconds() >= durationMilliseconds; }

        /*public int GetTimeRequested() { return requestTimeMilliseconds.GetData(); }
        public string GetUsername() { return requestorUsername; }
        public VRCUrl GetUrl() { return url; }
        public int GetStartTimeMilliseconds() { return startTimeMilliseconds; }
        public void TrySetStartTime() { if (Networking.IsOwner(gameObject) && !HasStarted()) { startTimeMilliseconds = GetVideoRequestManager().GetNetworkTimeMilliseconds(); } }
        public bool HasStarted() { return GetStartTimeMilliseconds() != 0; }

        public int GetTimeSinceRequested() { return GetVideoRequestManager().GetNetworkTimeMilliseconds() - GetTimeRequested(); }
        public float GetTimeSinceRequestedSeconds() { return GetTimeSinceRequested() / 1000f; }

        public int GetTimeSinceStarted() { return GetVideoRequestManager().GetNetworkTimeMilliseconds() - GetStartTimeMilliseconds(); }
        public float GetTimeSinceStartedSeconds() { return GetTimeSinceStarted() / 1000f; }

        public void ClearRequest() { MakeRequest(0, string.Empty, VRCUrl.Empty); }

        public bool IsEmpty() { return requestTimeMilliseconds.GetData() == 0 && string.IsNullOrEmpty(requestorUsername) && url.Get() == string.Empty; }
        public bool IsFull() { return requestTimeMilliseconds.GetData() != 0 && !string.IsNullOrEmpty(requestorUsername) && url.Get() != string.Empty; }*/

        public VideoRequestManager GetVideoRequestManager()
        { return videoRequestManager != null ? videoRequestManager : videoRequestManager = GetComponentInParent<VideoRequestManager>(); }

        private void SetOwnerLocal(GameObject target) { Networking.SetOwner(Networking.LocalPlayer, target); }
    }
}