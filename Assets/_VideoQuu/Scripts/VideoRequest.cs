
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
        [UdonSynced] private int endTimeMilliseconds;
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
        private int endTimeMillisecondsLast;

        public int GetEndTimeMilliseconds() { return endTimeMilliseconds; }
        public void SetEndTimeMilliseconds(int endTimeMilliseconds)
        { if (Networking.IsOwner(gameObject)) { this.endTimeMilliseconds = endTimeMilliseconds; } }
        public bool HasEndTimeMilliseconds() { return GetEndTimeMilliseconds() != -1; }

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
                endTimeMilliseconds != endTimeMillisecondsLast;
        }

        private void SetLasts()
        {
            isClaimedLast = isClaimed;
            requestTimeMillisecondsLast = requestTimeMilliseconds;
            requestorUsernameLast = requestorUsername;
            urlLast = url.Get();
            endTimeMillisecondsLast = endTimeMilliseconds;
        }

        private void HandleChanges() { GetVideoRequestManager().OnVideoRequestsChanged(); }

        public void ClearAttributes()
        {
            if (Networking.IsOwner(gameObject))
            {
                ClearAttributesInternal();
                HandleChanges();
            }
        }
        private void ClearAttributesInternal()
        {
            // Traditional Syncing
            if (Networking.IsOwner(gameObject))
            {
                isClaimed = false;
                requestTimeMilliseconds = -1;
                requestorUsername = null;
                url = VRCUrl.Empty;
                endTimeMilliseconds = -1;
            }
        }

        public bool IsValid()
        { return isClaimed = true && requestTimeMilliseconds != -1 && !string.IsNullOrEmpty(requestorUsername) && url != VRCUrl.Empty; }

        public void RequestURL(VRCUrl requestUrl)
        {
            // Traditional Syncing
            if (Networking.IsOwner(gameObject))
            {
                if (!isClaimed)
                {
                    isClaimed = true;
                    requestTimeMilliseconds = GetVideoRequestManager().GetNetworkTimeMilliseconds();
                    requestorUsername = GetVideoRequestManager().GetLocalUsername();
                    url = requestUrl;
                    HandleChanges();
                }
                else { Debug.LogError("[VideoQueuePlayer] VideoRequest: Tried to call RequestURL but was claimed."); }
            }
        }

        public bool IsClaimed() { return isClaimed; }
        public int GetRequestTimeMilliseconds() { return requestTimeMilliseconds; }
        public string GetRequestorUsername() { return requestorUsername; }
        public VRCUrl GetUrl() { return url; }

        private VideoRequestManager GetVideoRequestManager()
        { return videoRequestManager != null ? videoRequestManager : videoRequestManager = GetComponentInParent<VideoRequestManager>(); }

        // private void SetOwnerLocal(GameObject target) { Networking.SetOwner(Networking.LocalPlayer, target); }
    }
}