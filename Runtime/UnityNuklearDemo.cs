using NuklearDotNet;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.XR;
using static NuklearDotNet.Nuklear;

namespace Chutpot.Nuklear.Loader
{
    public unsafe class UnityNuklearDemo : MonoBehaviour, INuklearApp
    {
        [HideInInspector]
        public bool Rendering = false;

        private bool op = false;

        private nk_colorf _color;

        private void Start()
        {
            UnityNuklearRenderer.AddNuklearApp(this);
            _color = new nk_colorf();
        }

        private void OnDestroy()
        {
            UnityNuklearRenderer.RemoveNuklearApp(this);
        }

        public void Render(nk_context* ctx)
        {
            if (!Rendering)
                return;

            const NkPanelFlags flags = NkPanelFlags.Border | NkPanelFlags.Movable |
        NkPanelFlags.Minimizable | NkPanelFlags.Title | NkPanelFlags.NoScrollbar;

            if (nk_begin(ctx, "Demo", new NkRect(50, 50, 230, 250),(uint)flags) != 0)
            {
                nk_layout_row_static(ctx, 30, 80, 1);
                if (nk_button_label(ctx, "button") != 0)
                    Debug.Log("Demo Button Pressed");
                nk_layout_row_dynamic(ctx, 30, 2);
                if (nk_option_label(ctx, "Easy", Convert.ToInt32(op == false)) != 0) 
                    op = false;
                if (nk_option_label(ctx, "Hard", Convert.ToInt32(op == true)) != 0) 
                    op = true;
                nk_layout_row_dynamic(ctx, 22, 1);

                nk_layout_row_dynamic(ctx, 20, 1);
                nk_label(ctx, "background", (uint)NkTextAlignment.NK_TEXT_LEFT);
                nk_layout_row_dynamic(ctx, 25, 1);
                if (nk_combo_begin_color(ctx, _color, new nk_vec2(nk_widget_width(ctx), 400f)) != 0)
                {
                    nk_layout_row_dynamic(ctx, 120, 1);
                    _color = nk_color_picker(ctx, _color, nk_color_format.NK_RGBA);
                    nk_layout_row_dynamic(ctx, 25, 1);
                    _color.r = nk_propertyf(ctx, "#R:", 0, _color.r, 1.0f, 0.01f, 0.005f);
                    _color.g = nk_propertyf(ctx, "#G:", 0, _color.g, 1.0f, 0.01f, 0.005f);
                    _color.b = nk_propertyf(ctx, "#B:", 0, _color.b, 1.0f, 0.01f, 0.005f);
                    _color.a = nk_propertyf(ctx, "#A:", 0, _color.a, 1.0f, 0.01f, 0.005f);
                    nk_combo_end(ctx);
                }
            }
             nk_end(ctx);
        }
    }
}
