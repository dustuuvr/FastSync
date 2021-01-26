
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class SyncString : UdonSharpBehaviour
    {
        private SyncChar[] syncChars;
        private string data = "";

        protected void Start() { syncChars = GetComponentsInChildren<SyncChar>(); }

        // Call this method to request a new value for the SyncString
        public void RequestString(string request)
        {
            // Shorten the string if it's too long
            if ( request.Length > GetMaxChars()) { request = request.Substring(0, GetMaxChars()); }
            char[] requestChars = request.ToCharArray();
            for (int i = 0; i < syncChars.Length; i++) { syncChars[i].RequestChar(i < requestChars.Length ? requestChars[i] : '\0'); }
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "CombineData");
        }

        public string GetData() { return data; }

        public int GetMaxChars() { return syncChars.Length; }

        // TODO: Implement this cache pattern on other sync types
        // Called from RequestString via SendCustomNetworkEvent
        public void CombineData()
        {
            char[] dataChars = new char[syncChars.Length];
            for (int i = 0; i < dataChars.Length; i++) { dataChars[i] = syncChars[i].GetFastSynced(); }
            data = new string(dataChars);
        }
    }
}