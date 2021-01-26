
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync.Examples
{
    public class Calculator : UdonSharpBehaviour
    {
        [SerializeField] private Text synced;
        [SerializeField] private Text local;
        [SerializeField] private Text input;
        [SerializeField] private SyncInt syncInt;
        private string inputString = string.Empty;

        protected void Update()
        {
            synced.text = $"UdonSync: {syncInt.GetUdonSynced()}";
            local.text = $"FastSync: {syncInt.GetFastSynced()}";
            input.text = $"Input: {GetInputInteger().ToString()}";
        }

        private int GetInputInteger() { return string.IsNullOrEmpty(inputString) ? 0 : int.Parse(inputString); }

        public bool InputStringIsFull() { return inputString.Length >= 5; }

        public void AppendInput(int append)
        {
            if (!InputStringIsFull() && append >= 0 && append <= 9 && (append != 0 || inputString.Length > 0))
            { inputString += append; }
        }

        // public void BackspaceInput() { if (inputString.Length > 0) { inputString = inputString.Substring(0, inputString.Length - 1); } }

        public void ClearInput() { inputString = string.Empty; }

        public void Add() { ApplyOperation(GetInputInteger()); }
        public void Subtract() { ApplyOperation(-GetInputInteger()); }
        private void ApplyOperation(int change)
        {
            int operationResult = syncInt.GetFastSynced() + change;
            syncInt.RequestInt(Mathf.Clamp(operationResult, 0, 9999999));
            ClearInput();
        }
    }
}