
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class FastSyncByteDisplay : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncByte fastSyncByte;
        [SerializeField] private Text dataLabelText;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button submitButton;

        protected void Update()
        {
            dataLabelText.text = $"FastSyncByte: '{fastSyncByte.GetData()}'";

            if (IsValidInput(inputField.text))
            {
                inputField.textComponent.color = Color.green;
                submitButton.interactable = true;
            }
            else
            {
                inputField.textComponent.color = Color.red;
                submitButton.interactable = false;
            }
        }

        private bool IsValidInput(string input)
        {
            byte result;
            return byte.TryParse(input, out result);
        }

        public void SubmitInput()
        { if (IsValidInput(inputField.text)) { fastSyncByte.RequestByte(byte.Parse(inputField.text)); } }
    }
}