
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncCharManager : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncCharManagerChangedSubscribers;
        private char[] data;
        private FastSyncChar[] fastSyncChars;
        private bool[] fastSyncCharsChanged;

        private void FastSyncCharManagerChanged()
        {
            Debug.Log("FastSyncCharManagerChanged");
            if (fastSyncCharManagerChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncCharManagerChangedSubscriber in fastSyncCharManagerChangedSubscribers)
            { fastSyncCharManagerChangedSubscriber.SendCustomEvent("OnFastSyncCharManagerChanged"); }
            Debug.Log("FastSyncCharManagerChanged Complete");
        }

        // Call this method to change the chars
        public void RequestChars(char[] request)
        {
            if (GetFastSyncChars().Length == request.Length) { for (int i = 0; i < request.Length; i++) { GetFastSyncChars()[i].RequestChar(request[i]); } }
            else { Debug.LogError("[FastSync] FastSyncCharManager: Request chars length mismatch!"); }
        }

        public void HandleChange(FastSyncChar fastSyncCharChanged)
        {
            int indexOfFastSyncCharChanged = GetIndexOfFastSyncChar(fastSyncCharChanged);
            if (indexOfFastSyncCharChanged != -1)
            {
                GetFastSyncCharsChanged()[indexOfFastSyncCharChanged] = true;
                if (AllFastSyncCharsChanged())
                {
                    Debug.Log("[FastSync] FastSyncCharManager: HandleChange, ALL changed!");
                    // Update the data
                    for (int i = 0; i < GetData().Length; i++) { GetData()[i] = GetFastSyncChars()[i].GetData(); }
                    // Reset the change list
                    for (int i = 0; i < GetFastSyncCharsChanged().Length; i++) { GetFastSyncCharsChanged()[i] = false; }
                    FastSyncCharManagerChanged();
                }
                else { Debug.Log("[FastSync] FastSyncCharManager: HandleChange, NOT all changed!"); }
            }
            else { Debug.LogError("[FastSync] FastSyncCharManager: Attempted to call HandleChange with unrelated FastSyncChar"); }
        }

        private int GetIndexOfFastSyncChar(FastSyncChar fastSyncChar)
        {
            for (int i = 0; i < GetFastSyncChars().Length; i++) { if (GetFastSyncChars()[i] == fastSyncChar) { return i; } }
            return -1;
        }

        private bool AllFastSyncCharsChanged()
        {
            for (int i = 0; i < GetFastSyncCharsChanged().Length; i++) { if (!GetFastSyncCharsChanged()[i]) { return false; } }
            return true;
        }

        public char[] GetData() { return data != null ? data : data = new char[GetFastSyncChars().Length]; }

        private FastSyncChar[] GetFastSyncChars()
        { return fastSyncChars != null ? fastSyncChars : fastSyncChars = GetComponentsInChildren<FastSyncChar>(); }

        private bool[] GetFastSyncCharsChanged()
        { return fastSyncCharsChanged != null ? fastSyncCharsChanged : fastSyncCharsChanged = new bool[GetFastSyncChars().Length]; }

        public int GetMaxChars() { return GetFastSyncChars().Length; }
    }
}