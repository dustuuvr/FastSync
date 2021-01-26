
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class SyncInt : UdonSharpBehaviour
    {
        private SyncByte[] syncBytes;

        protected void Start() { syncBytes = GetComponentsInChildren<SyncByte>(); }

        // Call this method to request a new value for the SyncInt
        public void RequestInt(int request)
        {
            byte[] requestBytes = Int32ToBytes(request);
            if (syncBytes.Length == requestBytes.Length)
            {
                for (int i = 0; i < syncBytes.Length; i++) { syncBytes[i].RequestByte(requestBytes[i]); }
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Convert");
            }
            else { Debug.LogError("[FastSync] request bytes length mismatch!"); }
        }

        // Called from RequestInt via SendCustomNetworkEvent
        public void Convert() { foreach (SyncByte syncByte in syncBytes) { syncByte.Convert(); } }

        public int GetUdonSynced() { return GetSynced(false); }
        public int GetFastSynced() { return GetSynced(true); }
        private int GetSynced(bool fast)
        {
            byte[] bytes = new byte[syncBytes.Length];
            for (int i = 0; i < bytes.Length; i++) { bytes[i] = fast ? syncBytes[i].GetFastSynced() : syncBytes[i].GetUdonSynced(); }
            return BytesToInt32(bytes);
        }

        // Assumes Big-Endian order
        private int BytesToInt32(byte[] inputBytes) { return (inputBytes[0] << 24) | (inputBytes[1] << 16) | (inputBytes[2] << 8) | (inputBytes[3]); }

        // Assumes Big-Endian order
        private byte[] Int32ToBytes(int inputInt32)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)((inputInt32 >> 24) % 256);
            bytes[1] = (byte)((inputInt32 >> 16) % 256);
            bytes[2] = (byte)((inputInt32 >> 8) % 256);
            bytes[3] = (byte)(inputInt32 % 256);
            return bytes;
        }
    }
}