
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class FastSyncIntDisplay : UdonSharpBehaviour
    {
        [SerializeField] private FastSyncInt fastSyncInt;
        [SerializeField] private Text dataLabelText;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button submitButton;

        protected void Update()
        {
            dataLabelText.text = $"FastSyncInt: '{fastSyncInt.GetData()}'";
            bool isValidInput = IsValidInput(inputField.text);
            inputField.textComponent.color = isValidInput ? Color.green : Color.red;
            submitButton.GetComponentInChildren<Text>().color = isValidInput ? Color.green : Color.red;
            submitButton.interactable = isValidInput;
        }

        private bool IsValidInput(string input)
        {
            int result;
            return int.TryParse(input, out result);
        }

        public void SubmitInput()
        {
            if (IsValidInput(inputField.text))
            {
                fastSyncInt.RequestUInt(int.Parse(inputField.text));
                inputField.text = string.Empty;
            }
        }
    }
}