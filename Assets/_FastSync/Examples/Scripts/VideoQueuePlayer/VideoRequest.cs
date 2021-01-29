
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples.VideoQueuePlayerSystem
{
    public class VideoRequest : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncUInt time;
        [SerializeField] private FastSyncString username;
        [UdonSynced] private VRCUrl url;

        public void MakeRequest(uint time, string username, VRCUrl url)
        {
            if (IsEmpty())
            {
                // Traditional Syncing
                SetOwnerLocal(gameObject);
                this.url = url;
                // Fast Syncing
                this.time.RequestUInt(time);
                this.username.RequestString(username);
            }
            else { Debug.LogError("[VideoQueuePlayer] VideoRequest: Tried to call MakeRequest but was not empty."); }
        }

        public void ClearRequest() { MakeRequest(uint.MinValue, string.Empty, VRCUrl.Empty); }

        public bool IsEmpty()
        { return time.GetData() == uint.MinValue && username.GetData() == string.Empty && url.Get() == string.Empty; }

        private uint GetTime() { return (uint)Networking.GetServerTimeInMilliseconds(); }

        private void SetOwnerLocal(GameObject target) { Networking.SetOwner(Networking.LocalPlayer, target); }
    }
}