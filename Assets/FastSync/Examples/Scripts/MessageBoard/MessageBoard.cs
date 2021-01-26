
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class MessageBoard : UdonSharpBehaviour
    {
        [SerializeField] private SyncString syncString;
        [SerializeField] private Text dataLabel;
        [SerializeField] private InputField inputField;

        protected void Update() { dataLabel.text = $"Data: {syncString.GetData()}"; }

        public void SubmitInput() { syncString.RequestString(inputField.text); }
    }
}