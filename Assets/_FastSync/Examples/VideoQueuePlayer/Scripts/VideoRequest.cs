
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples.VideoQueuePlayerSystem
{
    public class VideoRequest : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncInt time;
        [SerializeField] private FastSyncString username;
        [UdonSynced] private VRCUrl url = VRCUrl.Empty;
        private VideoRequestManager videoRequestManager;

        public void MakeRequest(int time, string username, VRCUrl url)
        {
            if (IsEmpty())
            {
                // Traditional Syncing
                SetOwnerLocal(gameObject);
                this.url = url;
                // Fast Syncing
                this.time.RequestInt(time);
                this.username.RequestString(username);
            }
            else { Debug.LogError("[VideoQueuePlayer] VideoRequest: Tried to call MakeRequest but was not empty."); }
        }

        public int GetTime() { return time.GetData(); }
        public string GetUsername() { return username.GetData(); }
        public VRCUrl GetUrl() { return url; }

        public void ClearRequest() { MakeRequest(0, string.Empty, VRCUrl.Empty); }

        public bool IsEmpty() { return time.GetData() == 0 && username.GetData() == string.Empty && url.Get() == string.Empty; }
        public bool IsFull() { return time.GetData() != 0 && username.GetData() != string.Empty && url.Get() != string.Empty; }

        // Called from FastSyncInt via SendCustomEvent
        public void OnFastSyncIntChanged()
        {
            Debug.Log("OnFastSyncIntChanged");
            VideoRequestChanged();
        }

        // Called from FastSyncString via SendCustomEvent
        public void OnFastSyncStringChanged()
        {
            Debug.Log("OnFastSyncStringChanged");
            VideoRequestChanged();
        }

        private void VideoRequestChanged() { if (GetVideoRequestManager() != null) { GetVideoRequestManager().OnVideoRequestChanged(); } }

        private VideoRequestManager GetVideoRequestManager()
        { return videoRequestManager != null ? videoRequestManager : videoRequestManager = GetComponentInParent<VideoRequestManager>(); }

        private void SetOwnerLocal(GameObject target) { Networking.SetOwner(Networking.LocalPlayer, target); }
    }
}