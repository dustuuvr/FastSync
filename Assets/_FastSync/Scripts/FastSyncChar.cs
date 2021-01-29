
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncChar : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncCharChangedSubscribers;
        private char data;
        private FastSyncCharManager fastSyncCharManager;
        private FastSyncByteManager fastSyncByteManager;

        // Call this method to change the char
        public void RequestChar(char request) { GetFastSyncByteManager().RequestBytes(CharToBytes(request)); }

        private void FastSyncCharChanged()
        {
            if (fastSyncCharChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncCharChangedSubscriber in fastSyncCharChangedSubscribers)
            { fastSyncCharChangedSubscriber.SendCustomEvent("OnFastSyncCharChanged"); }
        }

        // This function is called via SendCustomEvent on the child FastSyncByteManager
        public void OnFastSyncByteManagerChanged() { SetData(BytesToChar(GetFastSyncByteManager().GetData())); }

        public char GetData() { return data; }
        private void SetData(char data)
        {
            this.data = data;
            if (GetFastSyncCharManager() != null) { GetFastSyncCharManager().HandleChange(this); }
            FastSyncCharChanged();
        }

        public FastSyncCharManager GetFastSyncCharManager()
        { return fastSyncCharManager != null ? fastSyncCharManager : fastSyncCharManager = GetComponentInParent<FastSyncCharManager>(); }

        private FastSyncByteManager GetFastSyncByteManager()
        { return fastSyncByteManager != null ? fastSyncByteManager : fastSyncByteManager = GetComponentInChildren<FastSyncByteManager>(); }

        // Assumes Big-Endian order
        private char BytesToChar(byte[] input) { return (char)((input[0] << 8) | (input[1])); }

        // Assumes Big-Endian order
        private byte[] CharToBytes(char input)
        {
            byte[] output = new byte[2];
            output[0] = (byte)((input >> 8) % 256);
            output[1] = (byte)(input % 256);
            return output;
        }
    }
}