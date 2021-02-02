
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
        [UdonSynced] private string requestorUsername; //TODO: Sync id not username
        [UdonSynced] private int videoDetailID;
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
        private int videoDetailIDLast;
        // Variables required to play
        private int endTimeMillisecondsLast;

        public int GetEndTimeMilliseconds() { return endTimeMilliseconds; }
        public void SetEndTimeMilliseconds(int endTimeMilliseconds)
        { if (Networking.IsOwner(gameObject)) { this.endTimeMilliseconds = endTimeMilliseconds; } }
        public bool HasEndTimeMilliseconds() { return GetEndTimeMilliseconds() != -1; }
        public bool HasEnded() { return HasEndTimeMilliseconds() && GetEndTimeMilliseconds() <= videoRequestManager.GetNetworkTimeMilliseconds(); }

        /*private bool firstDeserialization = true;
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
        }*/

        private bool firstUpdate = true;
        protected void Update()
        {
            if (firstUpdate)
            {
                SetLasts();
                firstUpdate = false;
            }

            if (HasChanged())
            {
                Debug.Log("Update() changes detected!");
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
                videoDetailID != videoDetailIDLast ||
                endTimeMilliseconds != endTimeMillisecondsLast;
        }

        private void SetLasts()
        {
            isClaimedLast = isClaimed;
            requestTimeMillisecondsLast = requestTimeMilliseconds;
            requestorUsernameLast = requestorUsername;
            videoDetailIDLast = videoDetailID;
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
                videoDetailID = -1;
                endTimeMilliseconds = -1;
            }
        }

        public bool IsValid()
        { return isClaimed = true && requestTimeMilliseconds != -1 && !string.IsNullOrEmpty(requestorUsername) && videoDetailID != -1; }

        public void RequestVideoDetail(VideoDetail videoDetail)
        {
            // Traditional Syncing
            if (Networking.IsOwner(gameObject))
            {
                if (!isClaimed)
                {
                    isClaimed = true;
                    requestTimeMilliseconds = GetVideoRequestManager().GetNetworkTimeMilliseconds();
                    requestorUsername = GetVideoRequestManager().GetLocalUsername();
                    videoDetailID = videoDetail.GetID();
                    HandleChanges();
                }
                else { Debug.LogError("[VideoQueuePlayer] VideoRequest: Tried to call RequestURL but was claimed."); }
            }
        }

        public bool IsClaimed() { return isClaimed; }
        public int GetRequestTimeMilliseconds() { return requestTimeMilliseconds; }
        public string GetRequestorUsername() { return requestorUsername; }
        public int GetVideoDetailID() { return videoDetailID; }
        public VRCUrl GetUrl() { return GetVideoRequestManager().GetVideoCollectionRoot().GetVideoDetailByID(GetVideoDetailID()).GetUrl(); }
        // public VRCUrl GetUrl() { return videoRequestManager.; }

        private VideoRequestManager GetVideoRequestManager()
        { return videoRequestManager != null ? videoRequestManager : videoRequestManager = GetComponentInParent<VideoRequestManager>(); }

        // private void SetOwnerLocal(GameObject target) { Networking.SetOwner(Networking.LocalPlayer, target); }
    }
}