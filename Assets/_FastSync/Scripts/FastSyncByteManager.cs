
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncByteManager : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncByteManagerChangedSubscribers;
        private byte[] data;
        private FastSyncByte[] fastSyncBytes;
        private bool[] fastSyncBytesChanged;

        private void FastSyncByteManagerChanged()
        {
            if (fastSyncByteManagerChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncByteManagerChangedSubscriber in fastSyncByteManagerChangedSubscribers)
            { fastSyncByteManagerChangedSubscriber.SendCustomEvent("OnFastSyncByteManagerChanged"); }
        }

        // Call this method to change the bytes
        public void RequestBytes(byte[] request)
        {
            if (GetFastSyncBytes().Length == request.Length) { for (int i = 0; i < request.Length; i++) { GetFastSyncBytes()[i].RequestByte(request[i]); } }
            else { Debug.LogError("[FastSync] FastSyncByteManager: Request bytes length mismatch!"); }
        }

        public void HandleChange(FastSyncByte fastSyncByteChanged)
        {
            int indexOfFastSyncByteChanged = GetIndexOfFastSyncByte(fastSyncByteChanged);
            if (indexOfFastSyncByteChanged != -1)
            {
                GetFastSyncBytesChanged()[indexOfFastSyncByteChanged] = true;
                if (AllFastSyncBytesChanged())
                {
                    Debug.Log("[FastSync] FastSyncByteManager: HandleChange, ALL changed!");
                    // Update the data
                    for (int i = 0; i < GetData().Length; i++) { GetData()[i] = GetFastSyncBytes()[i].GetData(); }
                    // Reset the change list
                    for (int i = 0; i < GetFastSyncBytesChanged().Length; i++) { GetFastSyncBytesChanged()[i] = false; }
                    FastSyncByteManagerChanged();
                }
                else { Debug.Log("[FastSync] FastSyncByteManager: HandleChange, NOT all changed!"); }
            }
            else { Debug.LogError("[FastSync] FastSyncByteManager: Attempted to call HandleChange with unrelated FastSyncByte"); }
        }

        private int GetIndexOfFastSyncByte(FastSyncByte fastSyncByte)
        {
            for (int i = 0; i < GetFastSyncBytes().Length; i++) { if (GetFastSyncBytes()[i] == fastSyncByte) { return i; } }
            return -1;
        }

        private bool AllFastSyncBytesChanged()
        {
            for (int i = 0; i < GetFastSyncBytesChanged().Length; i++) { if (!GetFastSyncBytesChanged()[i]) { return false; } }
            return true;
        }

        public byte[] GetData() { return data != null ? data : data = new byte[GetFastSyncBytes().Length]; }

        private FastSyncByte[] GetFastSyncBytes()
        { return fastSyncBytes != null ? fastSyncBytes : fastSyncBytes = GetComponentsInChildren<FastSyncByte>(); }

        private bool[] GetFastSyncBytesChanged()
        { return fastSyncBytesChanged != null ? fastSyncBytesChanged : fastSyncBytesChanged = new bool[GetFastSyncBytes().Length]; }
    }
}