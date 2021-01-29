
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
            // char.MinValue ('\0') causes the Unity UI Text to glitch, so we need to remove it here
            dataLabelText.text = $"FastSyncChar: '{(fastSyncChar.GetData() != char.MinValue ? fastSyncChar.GetData().ToString() : string.Empty)}'";
            bool isValidInput = IsValidInput(inputField.text);
            inputField.textComponent.color = isValidInput ? Color.green : Color.red;
            submitButton.GetComponentInChildren<Text>().color = isValidInput ? Color.green : Color.red;
            submitButton.interactable = isValidInput;
        }

        private bool IsValidInput(string input)
        {
            char result;
            return char.TryParse(input, out result);
        }

        public void SubmitInput()
        {
            if (IsValidInput(inputField.text))
            {
                fastSyncChar.RequestChar(char.Parse(inputField.text));
                inputField.text = string.Empty;
            }
        }
    }
}