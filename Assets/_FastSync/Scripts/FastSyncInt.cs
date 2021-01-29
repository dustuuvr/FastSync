
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncInt : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncIntChangedSubscribers;
        private FastSyncUInt fastSyncUInt;
        private int data;

        // Call this method to change the int
        public void RequestUInt(int request) { GetFastSyncUInt().RequestUInt(IntToUInt(request)); }

        private FastSyncUInt GetFastSyncUInt()
        { return fastSyncUInt != null ? fastSyncUInt : fastSyncUInt = GetComponentInChildren<FastSyncUInt>(); }

        private void FastSyncIntChanged()
        {
            if (fastSyncIntChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncIntChangedSubscriber in fastSyncIntChangedSubscribers)
            { fastSyncIntChangedSubscriber.SendCustomEvent("OnFastSyncIntChanged"); }
        }

        // This function is called via SendCustomEvent on the child FastSyncUInt
        public void OnFastSyncUIntChanged() { SetData(UIntToInt(GetFastSyncUInt().GetData())); }

        public int GetData() { return data; }
        private void SetData(int data)
        {
            this.data = data;
            FastSyncIntChanged();
        }

        // TODO: This works but it could be prettier
        private uint IntToUInt(int input)
        {
            if ( input == int.MinValue) { return uint.MinValue; }

            uint zero = IntToAbsUInt(int.MaxValue);
            uint offset = IntToAbsUInt(input);
            if ( input < 0) { return zero - offset; }
            else { return zero + offset; }
        }

        // TODO: This works but it could be prettier
        private int UIntToInt(uint input)
        {
            if ( input == uint.MinValue) { return int.MinValue; }

            uint zero = IntToAbsUInt(int.MaxValue);
            if ( input < zero) { return (int)(zero - input) * -1; }
            else { return (int)(input - zero); }
        }

        private uint IntToAbsUInt(int input) { return (uint)Mathf.Abs(input); }
    }
}