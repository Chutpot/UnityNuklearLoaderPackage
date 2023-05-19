using UnityEditor;
using UnityEngine;
using Chutpot.Nuklear.Loader;

namespace Chutpot.Nuklear.Loader.Editor
{
    [CustomEditor(typeof(UnityNuklearDemo))]
    public class UnityNuklearDemoInspector : UnityEditor.Editor
    {
        UnityNuklearDemo target;

        private void Awake()
        {
            target = (UnityNuklearDemo)base.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable Demo"))
            {
                target.Rendering = true;
            }

            if (GUILayout.Button("Disable Demo"))
            {
                target.Rendering = false;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
