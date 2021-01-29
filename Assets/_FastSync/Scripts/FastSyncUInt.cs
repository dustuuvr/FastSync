
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncUInt : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncUIntChangedSubscribers;
        private FastSyncByteManager fastSyncByteManager;
        private uint data;

        // Call this method to change the uint
        public void RequestUInt(uint request) { GetFastSyncByteManager().RequestBytes(UInt32ToBytes(request)); }

        private FastSyncByteManager GetFastSyncByteManager()
        { return fastSyncByteManager != null ? fastSyncByteManager : fastSyncByteManager = GetComponentInChildren<FastSyncByteManager>(); }

        private void FastSyncUIntChanged()
        {
            if (fastSyncUIntChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncUIntChangedSubscriber in fastSyncUIntChangedSubscribers)
            { fastSyncUIntChangedSubscriber.SendCustomEvent("OnFastSyncUIntChanged"); }
        }

        // This function is called via SendCustomEvent on the child FastSyncByteManager
        public void OnFastSyncByteManagerChanged() { SetData(BytesToUInt32(GetFastSyncByteManager().GetData())); }

        public uint GetData() { return data; }
        private void SetData(uint data)
        {
            this.data = data;
            FastSyncUIntChanged();
        }

        // Assumes Big-Endian order
        private uint BytesToUInt32(byte[] input) { return ((uint)input[0] << 24) | ((uint)input[1] << 16) | ((uint)input[2] << 8) | ((uint)input[3]); }

        // Assumes Big-Endian order
        private byte[] UInt32ToBytes(uint input)
        {
            byte[] output = new byte[4];
            output[0] = (byte)ModuloUInt(input >> 24, 256U);
            output[1] = (byte)ModuloUInt(input >> 16, 256U);
            output[2] = (byte)ModuloUInt(input >> 8, 256U);
            output[3] = (byte)ModuloUInt(input, 256U);
            return output;
        }

        // TODO: Replace this with standard % once U# supports % for uint variables
        private uint ModuloUInt(uint a, uint b) { return a - (a / b) * b; }
    }
}