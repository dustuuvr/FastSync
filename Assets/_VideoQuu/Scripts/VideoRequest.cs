
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
        // Caches
        private VideoRequestManager videoRequestManager;

        protected void Start()
        {
            if (Networking.IsOwner(gameObject)) { ClearAttributesInternal(); }
        }

        // Variables required to view
        private bool isClaimedLast;
        private int requestTimeMillisecondsLast;
        private string requestorUsernameLast;
        private string urlLast;
        // Variables required to play
       private int startTimeMillisecondsLast;
       private int durationMillisecondsLast;

        private bool firstDeserialization = true;
        public override void OnDeserialization()
        {
            if (firstDeserialization)
            {
                SetLasts();
                firstDeserialization = false;
            }

            if ( HasChanged() )
            {
                Debug.Log("OnDeserialization() changes detected!");
                HandleChanges();
                SetLasts();
            }
        }

        private bool HasChanged()
        {
            return
                isClaimed != isClaimedLast ||
                requestTimeMilliseconds != requestTimeMillisecondsLast ||
                requestorUsername != requestorUsernameLast ||
                url.Get() != urlLast ||
                startTimeMilliseconds != startTimeMillisecondsLast ||
                durationMilliseconds != durationMillisecondsLast;
        }

        private void SetLasts()
        {
            isClaimedLast = isClaimed;
            requestTimeMillisecondsLast = requestTimeMilliseconds;
            requestorUsernameLast = requestorUsername;
            urlLast = url.Get();
            startTimeMillisecondsLast = startTimeMilliseconds;
            durationMillisecondsLast = durationMilliseconds;
        }

        private void HandleChanges() { GetVideoRequestManager().OnVideoRequestsChanged(); }

        public void ClearAttributes()
        {
            ClearAttributesInternal();
            HandleChanges();
        }
        private void ClearAttributesInternal()
        {
            // Traditional Syncing
            SetOwnerLocal(gameObject);
            isClaimed = false;
            requestTimeMilliseconds = -1;
            requestorUsername = null;
            url = VRCUrl.Empty;
            startTimeMilliseconds = -1;
            durationMilliseconds = -1;
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