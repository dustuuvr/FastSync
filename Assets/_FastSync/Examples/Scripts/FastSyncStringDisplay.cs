
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

        private bool IsValidInput(string input) { return input.Length <= fastSyncString.GetMaxChars(); }

        public void SubmitInput()
        { if (IsValidInput(inputField.text)) { fastSyncString.RequestString(inputField.text); } }
    }
}