
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncString : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncStringChangedSubscribers;
        private FastSyncCharManager fastSyncCharManager;
        private string data;

        private FastSyncCharManager GetFastSyncCharManager()
        { return fastSyncCharManager != null ? fastSyncCharManager : fastSyncCharManager = GetComponentInChildren<FastSyncCharManager>(); }

        // Call this method to change the string
        public void RequestString(string request)
        {
            char[] requestChars = new char[GetFastSyncCharManager().GetMaxChars()];
            for (int i = 0; i < requestChars.Length; i++)
            { requestChars[i] = i < request.Length ? request.ToCharArray()[i] : char.MinValue; }
            GetFastSyncCharManager().RequestChars(requestChars);
        }

        private void FastSyncStringChanged()
        {
            if (fastSyncStringChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncStringChangedSubscriber in fastSyncStringChangedSubscribers)
            { fastSyncStringChangedSubscriber.SendCustomEvent("OnFastSyncStringChanged"); }
        }

        // This function is called via SendCustomEvent on the child FastSyncCharManager
        public void OnFastSyncCharManagerChanged()
        {
            char[] dataChars = GetFastSyncCharManager().GetData();
            string dataString = "";
            for ( int i = 0; i < dataChars.Length && dataChars[i] != char.MinValue; i++ ) { dataString += dataChars[i]; }
            SetData(dataString);
        }

        public string GetData() { return data; }
        private void SetData(string data)
        {
            this.data = data;
            FastSyncStringChanged();
        }

        public int GetMaxChars() { return GetFastSyncCharManager().GetMaxChars(); }
    }
}