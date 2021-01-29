
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class FastSyncUIntDisplay : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncUInt fastSyncUInt;
        [SerializeField] private Text dataLabelText;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button submitButton;

        protected void Update()
        {
            dataLabelText.text = $"FastSyncUInt: '{fastSyncUInt.GetData()}'";

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
            uint result;
            return uint.TryParse(input, out result);
        }

        public void SubmitInput()
        { if (IsValidInput(inputField.text)) { fastSyncUInt.RequestUInt(uint.Parse(inputField.text)); } }
    }
}