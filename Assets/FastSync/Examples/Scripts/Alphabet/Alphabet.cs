
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class Alphabet : UdonSharpBehaviour
    {
        [SerializeField] private Text synced;
        [SerializeField] private Text local;
        [SerializeField] private SyncChar syncChar;

        protected void Update()
        {
            synced.text = $"UdonSync: {syncChar.GetUdonSynced()}";
            local.text = $"FastSync: {syncChar.GetFastSynced()}";
        }

        public void SetChar(char c) { syncChar.RequestChar(c); }
    }
}