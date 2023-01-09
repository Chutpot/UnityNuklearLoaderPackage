using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using System.Diagnostics;

namespace Chutpot.Nuklear.Loader
{
    public class UnityNuklearInputHandle : MonoBehaviour
    {
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetKeyboardInput(KeyCode key, bool isDown);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetMousePosition(float posX, float posY, float scrollDeltaX, float scrollDeltaY);

        private KeyCode[] _keyCodes;

        private void Awake()
        {
            _keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        }

        private void Update()
        {
            HandleMouseInput();
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput() 
        {
            foreach (KeyCode key in _keyCodes)
            {
                if (Input.GetKey(key))
                    SetKeyboardInput(key, true);
                else
                    SetKeyboardInput(key, false);
            }
        }

        //need reverse y axis because unity y axis reverse to Nuklear
        private void HandleMouseInput() 
        {
            SetMousePosition(Input.mousePosition.x, Screen.currentResolution.height - Input.mousePosition.y, Input.mouseScrollDelta.x, Input.mouseScrollDelta.y);
        }
    }
}
