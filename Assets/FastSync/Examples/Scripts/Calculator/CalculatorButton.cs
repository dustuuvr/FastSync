
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    [RequireComponent(typeof(Text))]
    public class CalculatorButton : UdonSharpBehaviour
    {
        [SerializeField] private Calculator calculator;

        // Called from Button via SendCustomEvent
        public void OnClick()
        {
            string textString = GetComponent<Text>().text;

            if (textString == "0") { calculator.AppendInput(0); }
            else if (textString == "1") { calculator.AppendInput(1); }
            else if (textString == "2") { calculator.AppendInput(2); }
            else if (textString == "3") { calculator.AppendInput(3); }
            else if (textString == "4") { calculator.AppendInput(4); }
            else if (textString == "5") { calculator.AppendInput(5); }
            else if (textString == "6") { calculator.AppendInput(6); }
            else if (textString == "7") { calculator.AppendInput(7); }
            else if (textString == "8") { calculator.AppendInput(8); }
            else if (textString == "9") { calculator.AppendInput(9); }
            else if (textString == "+") { calculator.Add(); }
            else if (textString == "-") { calculator.Subtract(); }
            else { Debug.LogError($"Error: Invalid CalculatorButton Text: '{textString}'"); }
        }
    }
}