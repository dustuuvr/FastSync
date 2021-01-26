
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class SyncChar : UdonSharpBehaviour
    {
        private SyncByte[] syncBytes;

        protected void Start() { syncBytes = GetComponentsInChildren<SyncByte>(); }

        // Call this method to request a new value for the SyncChar
        public void RequestChar(char request)
        {
            byte[] requestBytes = CharToBytes(request);
            if (syncBytes.Length == requestBytes.Length)
            {
                for (int i = 0; i < syncBytes.Length; i++) { syncBytes[i].RequestByte(requestBytes[i]); }
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Convert");
            }
            else { Debug.LogError("[FastSync] request bytes length mismatch!"); }
        }

        // Called from RequestChar via SendCustomNetworkEvent
        public void Convert() { foreach (SyncByte syncByte in syncBytes) { syncByte.Convert(); } }

        public char GetUdonSynced() { return GetSynced(false); }
        public char GetFastSynced() { return GetSynced(true); }
        private char GetSynced(bool fast)
        {
            if (syncBytes == null) { return '\0'; }

            byte[] bytes = new byte[syncBytes.Length];
            for (int i = 0; i < bytes.Length; i++) { bytes[i] = fast ? syncBytes[i].GetFastSynced() : syncBytes[i].GetUdonSynced(); }
            return BytesToChar(bytes);
        }

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