
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class SyncString : UdonSharpBehaviour
    {
        private SyncByte[] syncBytes;

        protected void Start() { syncBytes = GetComponentsInChildren<SyncByte>(); }

        // Call this method to request a new value for the SyncString
        public void RequestString(string request)
        {
            /*byte[] requestBytes = Int32ToBytes(request);
            if (syncBytes.Length == requestBytes.Length)
            {
                for (int i = 0; i < syncBytes.Length; i++) { syncBytes[i].RequestByte(requestBytes[i]); }
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Convert");
            }
            else { Debug.LogError("[FastSync] request bytes length mismatch!"); }*/
        }
    }
}