using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static Chutpot.Nuklear.Loader.UnityNuklearRenderer;

namespace Chutpot.Nuklear.Loader
{
    public class UnityNuklearLogger : MonoBehaviour
    {
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RegisterDebugCallback(DebugLogCallback callback);

        public delegate void DebugLogCallback(IntPtr log, int size);

        private void Start()
        {
            RegisterDebugCallback(OnDebugCalled);
        }


        [AOT.MonoPInvokeCallback(typeof(DebugLogCallback))]
        private static void OnDebugCalled(IntPtr log, int size)
        {
            string debug_string = Marshal.PtrToStringAnsi(log, size);
            UnityEngine.Debug.Log(debug_string);
        }
    }
}
