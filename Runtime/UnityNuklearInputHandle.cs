using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Chutpot.Nuklear.Loader
{
    internal enum NKKeys
    {
        NK_KEY_NONE,
        NK_KEY_SHIFT,
        NK_KEY_CTRL,
        NK_KEY_DEL,
        NK_KEY_ENTER,
        NK_KEY_TAB,
        NK_KEY_BACKSPACE,
        NK_KEY_COPY,
        NK_KEY_CUT,
        NK_KEY_PASTE,
        NK_KEY_UP,
        NK_KEY_DOWN,
        NK_KEY_LEFT,
        NK_KEY_RIGHT,
        /* Shortcuts: text field */
        NK_KEY_TEXT_INSERT_MODE,
        NK_KEY_TEXT_REPLACE_MODE,
        NK_KEY_TEXT_RESET_MODE,
        NK_KEY_TEXT_LINE_START,
        NK_KEY_TEXT_LINE_END,
        NK_KEY_TEXT_START,
        NK_KEY_TEXT_END,
        NK_KEY_TEXT_UNDO,
        NK_KEY_TEXT_REDO,
        NK_KEY_TEXT_SELECT_ALL,
        NK_KEY_TEXT_WORD_LEFT,
        NK_KEY_TEXT_WORD_RIGHT,
        /* Shortcuts: scrollbar */
        NK_KEY_SCROLL_START,
        NK_KEY_SCROLL_END,
        NK_KEY_SCROLL_DOWN,
        NK_KEY_SCROLL_UP,
        NK_KEY_MAX
    };

    internal enum NKButtons
    {
        NK_BUTTON_LEFT,
        NK_BUTTON_MIDDLE,
        NK_BUTTON_RIGHT,
        NK_BUTTON_DOUBLE,
        NK_BUTTON_MAX
    };


    public class UnityNuklearInputHandle : MonoBehaviour
    {
        Dictionary<KeyCode, char> keycodeToChar = new Dictionary<KeyCode, char>()
        {
          //-------------------------LOGICAL mappings-------------------------
  
          //Lower Case Letters
          {KeyCode.A, 'a'},
          {KeyCode.B, 'b'},
          {KeyCode.C, 'c'},
          {KeyCode.D, 'd'},
          {KeyCode.E, 'e'},
          {KeyCode.F, 'f'},
          {KeyCode.G, 'g'},
          {KeyCode.H, 'h'},
          {KeyCode.I, 'i'},
          {KeyCode.J, 'j'},
          {KeyCode.K, 'k'},
          {KeyCode.L, 'l'},
          {KeyCode.M, 'm'},
          {KeyCode.N, 'n'},
          {KeyCode.O, 'o'},
          {KeyCode.P, 'p'},
          {KeyCode.Q, 'q'},
          {KeyCode.R, 'r'},
          {KeyCode.S, 's'},
          {KeyCode.T, 't'},
          {KeyCode.U, 'u'},
          {KeyCode.V, 'v'},
          {KeyCode.W, 'w'},
          {KeyCode.X, 'x'},
          {KeyCode.Y, 'y'},
          {KeyCode.Z, 'z'},
  
          //KeyPad Numbers
          {KeyCode.Keypad0, '0'},
          {KeyCode.Keypad1, '1'},
          {KeyCode.Keypad2, '2'},
          {KeyCode.Keypad3, '3'},
          {KeyCode.Keypad4, '4'},
          {KeyCode.Keypad5, '5'},
          {KeyCode.Keypad6, '6'},
          {KeyCode.Keypad7, '7'},
          {KeyCode.Keypad8, '8'},
          {KeyCode.Keypad9, '9'},
  
          //Other Symbols
          {KeyCode.Exclaim, '!'}, //1
          {KeyCode.DoubleQuote, '"'},
          {KeyCode.Hash, '#'}, //3
          {KeyCode.Dollar, '$'}, //4
          {KeyCode.Ampersand, '&'}, //7
          {KeyCode.Quote, '\''}, //remember the special forward slash rule... this isnt wrong
          {KeyCode.LeftParen, '('}, //9
          {KeyCode.RightParen, ')'}, //0
          {KeyCode.Asterisk, '*'}, //8
          {KeyCode.Plus, '+'},
          {KeyCode.Comma, ','},
          {KeyCode.Minus, '-'},
          {KeyCode.Period, '.'},
          {KeyCode.Slash, '/'},
          {KeyCode.Colon, ':'},
          {KeyCode.Semicolon, ';'},
          {KeyCode.Space, ' '},
          {KeyCode.Less, '<'},
          {KeyCode.Equals, '='},
          {KeyCode.Greater, '>'},
          {KeyCode.Question, '?'},
          {KeyCode.At, '@'}, //2
          {KeyCode.LeftBracket, '['},
          {KeyCode.Backslash, '\\'}, //remember the special forward slash rule... this isnt wrong
          {KeyCode.RightBracket, ']'},
          {KeyCode.Caret, '^'}, //6
          {KeyCode.Underscore, '_'},
          {KeyCode.BackQuote, '`'},
  
          //-------------------------NON-LOGICAL mappings-------------------------

          {KeyCode.Alpha0, '0'},
          {KeyCode.Alpha1, '1'},
          {KeyCode.Alpha2, '2'},
          {KeyCode.Alpha3, '3'},
          {KeyCode.Alpha4, '4'},
          {KeyCode.Alpha5, '5'},
          {KeyCode.Alpha6, '6'},
          {KeyCode.Alpha7, '7'},
          {KeyCode.Alpha8, '8'},
          {KeyCode.Alpha9, '9'}
        };


        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetKeyboardInput(int key, bool isDown);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetMouseInput(float posX, float posY, float scrollDeltaX, float scrollDeltaY);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetCharInput(char c);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetButtonInput(int key, bool isDown);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private extern static void SetInputState(bool isOpen);

        private KeyCode[] _keyCodes;

        static char ch;

        private void Awake()
        {
            _keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        }


        private void Update()
        {
            SetInputState(true);
            HandleMouseInput();
            HandleKeyboardInput();
            SetInputState(false);
        }
        

        private void HandleKeyboardInput() 
        {

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                SetKeyboardInput((int)NKKeys.NK_KEY_COPY, Input.GetKeyDown(KeyCode.C));
                SetKeyboardInput((int)NKKeys.NK_KEY_PASTE, Input.GetKeyDown(KeyCode.V));
                SetKeyboardInput((int)NKKeys.NK_KEY_CUT, Input.GetKeyDown(KeyCode.X));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_UNDO, Input.GetKeyDown(KeyCode.Z));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_REDO, Input.GetKeyDown(KeyCode.R));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_WORD_LEFT, Input.GetKeyDown(KeyCode.LeftArrow));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_WORD_RIGHT, Input.GetKeyDown(KeyCode.RightArrow));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_LINE_START, Input.GetKeyDown(KeyCode.B));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_LINE_END, Input.GetKeyDown(KeyCode.E));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_SELECT_ALL, Input.GetKeyDown(KeyCode.A));
            }
            else
            {
                SetKeyboardInput((int)NKKeys.NK_KEY_DEL, Input.GetKeyDown(KeyCode.Delete));
                SetKeyboardInput((int)NKKeys.NK_KEY_ENTER, Input.GetKeyDown(KeyCode.Return));               
                SetKeyboardInput((int)NKKeys.NK_KEY_TAB, Input.GetKeyDown(KeyCode.Tab));
                SetKeyboardInput((int)NKKeys.NK_KEY_BACKSPACE, Input.GetKeyDown(KeyCode.Backspace));
                SetKeyboardInput((int)NKKeys.NK_KEY_UP, Input.GetKeyDown(KeyCode.UpArrow));
                SetKeyboardInput((int)NKKeys.NK_KEY_DOWN, Input.GetKeyDown(KeyCode.DownArrow));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_START, Input.GetKeyDown(KeyCode.Home));
                SetKeyboardInput((int)NKKeys.NK_KEY_TEXT_END, Input.GetKeyDown(KeyCode.End));
                SetKeyboardInput((int)NKKeys.NK_KEY_SCROLL_START, Input.GetKeyDown(KeyCode.Home));
                SetKeyboardInput((int)NKKeys.NK_KEY_SCROLL_END, Input.GetKeyDown(KeyCode.End));
                SetKeyboardInput((int)NKKeys.NK_KEY_SCROLL_DOWN, Input.GetKeyDown(KeyCode.PageDown));
                SetKeyboardInput((int)NKKeys.NK_KEY_SCROLL_UP, Input.GetKeyDown(KeyCode.PageUp));
                SetKeyboardInput((int)NKKeys.NK_KEY_SHIFT, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));

                foreach (KeyCode key in _keyCodes)
                {
                    if (Input.GetKeyDown(key) && keycodeToChar.TryGetValue(key, out ch))
                        SetCharInput(ch);
                }


                SetKeyboardInput((int)NKKeys.NK_KEY_LEFT, Input.GetKeyDown(KeyCode.RightArrow));
                SetKeyboardInput((int)NKKeys.NK_KEY_RIGHT, Input.GetKeyDown(KeyCode.LeftArrow));
                SetKeyboardInput((int)NKKeys.NK_KEY_COPY, false);
                SetKeyboardInput((int)NKKeys.NK_KEY_PASTE, false);
                SetKeyboardInput((int)NKKeys.NK_KEY_CUT, false);
                SetKeyboardInput((int)NKKeys.NK_KEY_SHIFT, false);
            }


            //SetButtonInput((int)NKButtons.NK_BUTTON_DOUBLE, Input.GetMouseButton(0));
            SetButtonInput((int)NKButtons.NK_BUTTON_LEFT, Input.GetMouseButton(0));

            SetButtonInput((int)NKButtons.NK_BUTTON_RIGHT, Input.GetMouseButton(1));

            SetButtonInput((int)NKButtons.NK_BUTTON_MIDDLE, Input.GetMouseButton(2));
        }

        //need reverse y axis because unity y axis reverse to Nuklear
        private void HandleMouseInput() 
        {
            SetMouseInput(Input.mousePosition.x, Screen.currentResolution.height - Input.mousePosition.y, Input.mouseScrollDelta.x, Input.mouseScrollDelta.y);
        }
    }
}
