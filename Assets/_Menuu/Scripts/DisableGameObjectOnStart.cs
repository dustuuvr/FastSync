
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.Uutils.MenuuSystem
{
    public class DisableGameObjectOnStart : UdonSharpBehaviour
    {
        protected void Start() { gameObject.SetActive(false); }
    }
}