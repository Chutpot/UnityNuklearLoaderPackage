using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using NuklearDotNet;
using static NuklearDotNet.Nuklear;
using System.Linq;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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


    public unsafe class UnityNuklearInputHandle : MonoBehaviour
    {
#if ENABLE_LEGACY_INPUT_MANAGER
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

        private KeyCode[] _keyCodes;
#elif ENABLE_INPUT_SYSTEM
        Dictionary<Key, char> keycodeToChar = new Dictionary<Key, char>()
        {
          //-------------------------LOGICAL mappings-------------------------
  
          //Lower Case Letters
          {Key.A, 'a'},
          {Key.B, 'b'},
          {Key.C, 'c'},
          {Key.D, 'd'},
          {Key.E, 'e'},
          {Key.F, 'f'},
          {Key.G, 'g'},
          {Key.H, 'h'},
          {Key.I, 'i'},
          {Key.J, 'j'},
          {Key.K, 'k'},
          {Key.L, 'l'},
          {Key.M, 'm'},
          {Key.N, 'n'},
          {Key.O, 'o'},
          {Key.P, 'p'},
          {Key.Q, 'q'},
          {Key.R, 'r'},
          {Key.S, 's'},
          {Key.T, 't'},
          {Key.U, 'u'},
          {Key.V, 'v'},
          {Key.W, 'w'},
          {Key.X, 'x'},
          {Key.Y, 'y'},
          {Key.Z, 'z'},
  
          //KeyPad Numbers
          {Key.Numpad0, '0'},
          {Key.Numpad1, '1'},
          {Key.Numpad2, '2'},
          {Key.Numpad3, '3'},
          {Key.Numpad4, '4'},
          {Key.Numpad5, '5'},
          {Key.Numpad6, '6'},
          {Key.Numpad7, '7'},
          {Key.Numpad8, '8'},
          {Key.Numpad9, '9'},
  
          //Other Symbols
          //{Key.Exclaim, '!'}, //1
          //{Key.DoubleQuote, '"'},
          //{Key.Hash, '#'}, //3
          //{Key.Dollar, '$'}, //4
          //{Key.Ampersand, '&'}, //7
          {Key.Quote, '\''}, //remember the special forward slash rule... this isnt wrong
          //{Key.LeftParen, '('}, //9
          //{Key.RightParen, ')'}, //0
          //{Key.Asterisk, '*'}, //8
          {Key.NumpadPlus, '+'},
          {Key.Comma, ','},
          {Key.Minus, '-'},
          {Key.Period, '.'},
          {Key.Slash, '/'},
          //{Key.Colon, ':'},
          {Key.Semicolon, ';'},
          {Key.Space, ' '},
          //{Key.Less, '<'},
          {Key.Equals, '='},
          //{Key.Greater, '>'},
          //{Key.Question, '?'},
          //{Key.At, '@'}, //2
          {Key.LeftBracket, '['},
          {Key.Backslash, '\\'}, //remember the special forward slash rule... this isnt wrong
          {Key.RightBracket, ']'},
          //{Key.Caret, '^'}, //6
          //{Key.Underscore, '_'},
          {Key.Backquote, '`'},
  
          //-------------------------NON-LOGICAL mappings-------------------------

          {Key.Digit0, '0'},
          {Key.Digit1, '1'},
          {Key.Digit2, '2'},
          {Key.Digit3, '3'},
          {Key.Digit4, '4'},
          {Key.Digit5, '5'},
          {Key.Digit6, '6'},
          {Key.Digit7, '7'},
          {Key.Digit8, '8'},
          {Key.Digit9, '9'}
        };

        private Keyboard _keyboard = Keyboard.current;
        private Mouse _mouse = Mouse.current;
        private Key[] _keyCodes;
#endif


        static char ch;
        private nk_context* _ctx;

        private void Awake()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            _keyCodes = keycodeToChar.Keys.ToArray();
#elif ENABLE_INPUT_SYSTEM
            _keyCodes = keycodeToChar.Keys.ToArray();
#endif
        }

        private void Start()
        {
            _ctx = UnityNuklearRenderer.Ctx;
        }


        private void Update()
        {
            nk_input_begin(UnityNuklearRenderer.Ctx);
            HandleMouseInput();
            HandleKeyboardInput();
            nk_input_end(UnityNuklearRenderer.Ctx);
        }

        private void HandleKeyboardInput() 
        {

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                nk_input_key(_ctx, NkKeys.Copy, Convert.ToInt32(Input.GetKeyDown(KeyCode.C)));
                nk_input_key(_ctx, NkKeys.Paste, Convert.ToInt32(Input.GetKeyDown(KeyCode.V)));
                nk_input_key(_ctx, NkKeys.Cut, Convert.ToInt32(Input.GetKeyDown(KeyCode.X)));
                nk_input_key(_ctx, NkKeys.TextUndo, Convert.ToInt32(Input.GetKeyDown(KeyCode.Z)));
                nk_input_key(_ctx, NkKeys.TextRedo, Convert.ToInt32(Input.GetKeyDown(KeyCode.R)));
                nk_input_key(_ctx, NkKeys.TextWordRight, Convert.ToInt32(Input.GetKeyDown(KeyCode.RightArrow)));
                nk_input_key(_ctx, NkKeys.TextWordLeft, Convert.ToInt32(Input.GetKeyDown(KeyCode.LeftArrow)));
                nk_input_key(_ctx, NkKeys.LineStart, Convert.ToInt32(Input.GetKeyDown(KeyCode.B)));
                nk_input_key(_ctx, NkKeys.LineEnd, Convert.ToInt32(Input.GetKeyDown(KeyCode.E)));
                nk_input_key(_ctx, NkKeys.TextSelectAll, Convert.ToInt32(Input.GetKeyDown(KeyCode.A)));
            }
            else
            {
                nk_input_key(_ctx, NkKeys.Del, Convert.ToInt32(Input.GetKeyDown(KeyCode.Delete)));
                nk_input_key(_ctx, NkKeys.Enter, Convert.ToInt32(Input.GetKeyDown(KeyCode.Return)));
                nk_input_key(_ctx, NkKeys.Tab, Convert.ToInt32(Input.GetKeyDown(KeyCode.Tab)));
                nk_input_key(_ctx, NkKeys.Backspace, Convert.ToInt32(Input.GetKeyDown(KeyCode.Backspace)));
                nk_input_key(_ctx, NkKeys.Up, Convert.ToInt32(Input.GetKeyDown(KeyCode.UpArrow)));
                nk_input_key(_ctx, NkKeys.Down, Convert.ToInt32(Input.GetKeyDown(KeyCode.DownArrow)));
                nk_input_key(_ctx, NkKeys.TextStart, Convert.ToInt32(Input.GetKeyDown(KeyCode.Home)));
                nk_input_key(_ctx, NkKeys.TextEnd, Convert.ToInt32(Input.GetKeyDown(KeyCode.End)));
                nk_input_key(_ctx, NkKeys.ScrollStart, Convert.ToInt32(Input.GetKeyDown(KeyCode.Home)));
                nk_input_key(_ctx, NkKeys.ScrollEnd, Convert.ToInt32(Input.GetKeyDown(KeyCode.End)));
                nk_input_key(_ctx, NkKeys.ScrollDown, Convert.ToInt32(Input.GetKeyDown(KeyCode.PageDown)));
                nk_input_key(_ctx, NkKeys.ScrollUp, Convert.ToInt32(Input.GetKeyDown(KeyCode.PageUp)));
                nk_input_key(_ctx, NkKeys.Shift, Convert.ToInt32(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));

                foreach (KeyCode key in _keyCodes)
                {
                    if (Input.GetKeyDown(key) && keycodeToChar.TryGetValue(key, out ch))
                        nk_input_unicode(_ctx, ch);
                }

                nk_input_key(_ctx, NkKeys.Left, Convert.ToInt32(Input.GetKeyDown(KeyCode.LeftArrow)));
                nk_input_key(_ctx, NkKeys.Right, Convert.ToInt32(Input.GetKeyDown(KeyCode.RightArrow)));
                nk_input_key(_ctx, NkKeys.Copy, 0);
                nk_input_key(_ctx, NkKeys.Paste, 0);
                nk_input_key(_ctx, NkKeys.Cut, 0);
                nk_input_key(_ctx, NkKeys.Shift, 0);
            }


            //SetButtonInput((int)NKButtons.NK_BUTTON_DOUBLE, Input.GetMouseButton(0));

            nk_input_button(_ctx, nk_buttons.NK_BUTTON_LEFT, (int)Input.mousePosition.x, (Screen.currentResolution.height - (int)Input.mousePosition.y), Convert.ToInt32(Input.GetMouseButton(0)));
            nk_input_button(_ctx, nk_buttons.NK_BUTTON_RIGHT, (int)Input.mousePosition.x, (Screen.currentResolution.height - (int)Input.mousePosition.y), Convert.ToInt32(Input.GetMouseButton(1)));
            nk_input_button(_ctx, nk_buttons.NK_BUTTON_MIDDLE, (int)Input.mousePosition.x, (Screen.currentResolution.height - (int)Input.mousePosition.y), Convert.ToInt32(Input.GetMouseButton(2)));

#elif ENABLE_INPUT_SYSTEM
            if (_keyboard.leftCtrlKey.wasPressedThisFrame || _keyboard.rightCtrlKey.wasPressedThisFrame)
            {
                nk_input_key(_ctx, NkKeys.Copy, Convert.ToInt32(_keyboard.cKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Paste, Convert.ToInt32(_keyboard.vKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Cut, Convert.ToInt32(_keyboard.xKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextUndo, Convert.ToInt32(_keyboard.zKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextRedo, Convert.ToInt32(_keyboard.rKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextWordRight, Convert.ToInt32(_keyboard.rightAppleKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextWordLeft, Convert.ToInt32(_keyboard.leftArrowKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.LineStart, Convert.ToInt32(_keyboard.bKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.LineEnd, Convert.ToInt32(_keyboard.eKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextSelectAll, Convert.ToInt32(_keyboard.aKey.wasPressedThisFrame));
            }
            else
            {
                nk_input_key(_ctx, NkKeys.Del, Convert.ToInt32(_keyboard.deleteKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Enter, Convert.ToInt32(_keyboard.enterKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Tab, Convert.ToInt32(_keyboard.tabKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Backspace, Convert.ToInt32(_keyboard.backspaceKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Up, Convert.ToInt32(_keyboard.upArrowKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Down, Convert.ToInt32(_keyboard.downArrowKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextStart, Convert.ToInt32(_keyboard.homeKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.TextEnd, Convert.ToInt32(_keyboard.endKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.ScrollStart, Convert.ToInt32(_keyboard.homeKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.ScrollEnd, Convert.ToInt32(_keyboard.endKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.ScrollDown, Convert.ToInt32(_keyboard.pageDownKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.ScrollUp, Convert.ToInt32(_keyboard.pageUpKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Shift, Convert.ToInt32(_keyboard.leftShiftKey.wasPressedThisFrame || _keyboard.rightShiftKey.wasPressedThisFrame));

                foreach (Key key in _keyCodes)
                {
                    if (_keyboard[key].wasPressedThisFrame && keycodeToChar.TryGetValue(key, out ch))
                        nk_input_unicode(_ctx, ch);
                }

                nk_input_key(_ctx, NkKeys.Left, Convert.ToInt32(_keyboard.leftArrowKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Right, Convert.ToInt32(_keyboard.rightArrowKey.wasPressedThisFrame));
                nk_input_key(_ctx, NkKeys.Copy, 0);
                nk_input_key(_ctx, NkKeys.Paste, 0);
                nk_input_key(_ctx, NkKeys.Cut, 0);
                nk_input_key(_ctx, NkKeys.Shift, 0);
            }


            //SetButtonInput((int)NKButtons.NK_BUTTON_DOUBLE, Input.GetMouseButton(0));

            nk_input_button(_ctx, nk_buttons.NK_BUTTON_LEFT, (int)_mouse.position.x.value, (Screen.currentResolution.height - (int)_mouse.position.y.value), Convert.ToInt32(_mouse.leftButton.isPressed));
            nk_input_button(_ctx, nk_buttons.NK_BUTTON_RIGHT, (int)_mouse.position.x.value, (Screen.currentResolution.height - (int)_mouse.position.y.value), Convert.ToInt32(_mouse.rightButton.isPressed));
            nk_input_button(_ctx, nk_buttons.NK_BUTTON_MIDDLE, (int)_mouse.position.x.value, (Screen.currentResolution.height - (int)_mouse.position.y.value), Convert.ToInt32(_mouse.middleButton.isPressed));
#endif
        }

        //need reverse y axis because unity y axis reverse to Nuklear
        private void HandleMouseInput() 
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            nk_input_motion(UnityNuklearRenderer.Ctx, (int)Input.mousePosition.x, (Screen.currentResolution.height - (int)Input.mousePosition.y));
            nk_input_scroll(UnityNuklearRenderer.Ctx, new nk_vec2(Input.mouseScrollDelta.x, Input.mouseScrollDelta.y));
#elif ENABLE_INPUT_SYSTEM
            nk_input_motion(UnityNuklearRenderer.Ctx, (int)_mouse.position.x.value, (Screen.currentResolution.height - (int)_mouse.position.y.value));
            nk_input_scroll(UnityNuklearRenderer.Ctx, new nk_vec2(_mouse.scroll.value.x * Time.deltaTime, _mouse.scroll.value.y * Time.deltaTime));
#endif
        }
    }
}
