
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class FastSyncCharDisplay : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncChar fastSyncChar;
        [SerializeField] private Text dataLabelText;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button submitButton;

        protected void Update()
        {
            char displayData = fastSyncChar.GetData();
            // char.MinValue ('\0') causes the Unity UI Text to glitch, so we need to remove it here
            dataLabelText.text = $"FastSyncChar: '{(displayData != char.MinValue ? displayData.ToString() : "")}'";

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
            char result;
            return char.TryParse(input, out result);
        }

        public void SubmitInput()
        { if (IsValidInput(inputField.text)) { fastSyncChar.RequestChar(char.Parse(inputField.text)); } }
    }
}