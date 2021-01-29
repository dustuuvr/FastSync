
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class FastSyncStringDisplay : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncString fastSyncString;
        [SerializeField] private Text dataLabelText;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button submitButton;

        protected void Update()
        {
            dataLabelText.text = $"FastSyncString: '{fastSyncString.GetData()}'";
            bool isValidInput = IsValidInput(inputField.text);
            inputField.textComponent.color = isValidInput ? Color.green : Color.red;
            submitButton.GetComponentInChildren<Text>().color = isValidInput ? Color.green : Color.red;
            submitButton.interactable = isValidInput;
        }

        private bool IsValidInput(string input) { return input.Length <= fastSyncString.GetMaxChars(); }

        public void SubmitInput()
        {
            if (IsValidInput(inputField.text))
            {
                fastSyncString.RequestString(inputField.text);
                inputField.text = string.Empty;
            }
        }
    }
}