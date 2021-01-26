
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    [RequireComponent(typeof(Text))]
    public class AlphabetButton : UdonSharpBehaviour
    {
        [SerializeField] private Alphabet alphabet;

        // Called from Button via SendCustomEvent
        public void OnClick()
        {
            string textString = GetComponent<Text>().text;
            if (textString.Length == 1)
            {
                char c = textString[0];
                Debug.Log($"Trying to set char: {c}");
                alphabet.SetChar(c);
            }
        }
    }
}