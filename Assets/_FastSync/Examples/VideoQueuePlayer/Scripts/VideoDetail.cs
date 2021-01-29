
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples.VideoQueuePlayerSystem
{
    public class VideoDetail : UdonSharpBehaviour
    {
        [SerializeField] private string title;
        [SerializeField] private VRCUrl url;

        public string GetTitle() { return title; }
        public VRCUrl GetUrl() { return url; }
    }
}