using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using System.Diagnostics;

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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            RegisterDebugCallback(OnDebugCalled);
            ChangeViewport(Screen.currentResolution.width, Screen.currentResolution.height);
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

        //Render Over Everything, if Gizmos are enabled nothing will be displayed!
        private void OnGUI()
        {
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
        }
    }
}