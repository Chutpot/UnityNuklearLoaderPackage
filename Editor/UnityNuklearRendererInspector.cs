using UnityEditor;
using UnityEngine;
using Chutpot.Nuklear.Loader;

namespace Chutpot.Nuklear.Loader.Editor
{
    [CustomEditor(typeof(UnityNuklearRenderer))]
    public class UnityNuklearRendererInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable Demo"))
            {
                UnityNuklearRenderer.SetIsDemoRendering(true);
            }

            if (GUILayout.Button("Disable Demo"))
            {
                UnityNuklearRenderer.SetIsDemoRendering(false);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
