using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using System.Collections;
using System.Collections.Generic;
using NuklearDotNet;


namespace Chutpot.Nuklear.Loader
{
    public unsafe interface INuklearApp
    {
        public unsafe void Render(nk_context* ctx);
    }

    public unsafe class UnityNuklearRenderer : MonoBehaviour
    {
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetRenderEventFunc();
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ChangeViewport(int width, int height);
        [DllImport("UnityNuklearLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern nk_context* GetContext();


        private static readonly List<INuklearApp> _nuklearApps = new List<INuklearApp>();

        private Coroutine _renderCoroutine;

        private static nk_context* _ctx;

        internal static nk_context* Ctx { get { return _ctx; } }

        private void Awake()
        {
            _ctx = GetContext();
            ChangeViewport(Screen.currentResolution.width, Screen.currentResolution.height);
            _renderCoroutine = StartCoroutine(Render());
        }

        private void OnDestroy()
        {
            StopCoroutine(_renderCoroutine);
        }

        public static void AddNuklearApp(INuklearApp nuklearRender)
        {
            _nuklearApps.Add(nuklearRender);
        }

        public static void RemoveNuklearApp(INuklearApp nuklearRender)
        {
            _nuklearApps.Remove(nuklearRender);
        }


        private IEnumerator Render() 
        {
            while (true) 
            {
                yield return new WaitForEndOfFrame();
                RenderApps();
                GL.IssuePluginEvent(GetRenderEventFunc(), 1);
            }
        }

        private void RenderApps()
        {
            foreach (var app in _nuklearApps)
            {
                app.Render(_ctx);
            }
        }

        [MonoPInvokeCallback(typeof(nk_plugin_filter_t))]
        public static int NkPluginFilterCallback(ref nk_text_edit edit, uint unicode_rune)
        {
            return 1;
        }
    }
}