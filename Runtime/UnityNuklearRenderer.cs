using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using System.Diagnostics;
using System.Collections;

namespace Chutpot.Nuklear.Loader
{
    public class UnityNuklearRenderer : MonoBehaviour
    {
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetRenderEventFunc();
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RegisterDebugCallback(DebugLogCallback callback);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ChangeViewport(int width, int height);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsDemoRendering(bool isRendering);

        public delegate void DebugLogCallback(IntPtr log, int size);

        private Coroutine _renderCoroutine;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            RegisterDebugCallback(OnDebugCalled);
            ChangeViewport(Screen.currentResolution.width, Screen.currentResolution.height);
            _renderCoroutine = StartCoroutine(Render());
        }

        private void OnDestroy()
        {
            StopCoroutine(_renderCoroutine);
        }

#if ENABLE_MONO
        [MonoPInvokeCallback(typeof(DebugLogCallback))]
#elif ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
#endif
        private void OnDebugCalled(IntPtr log, int size)
        {
            string debug_string = Marshal.PtrToStringAnsi(log, size);
            UnityEngine.Debug.Log(debug_string);
        }

        IEnumerator Render() 
        {
            while (true) 
            {
                yield return new WaitForEndOfFrame();
                GL.IssuePluginEvent(GetRenderEventFunc(), 1);
            }
        }
    }
}