using Toolbox.Editor;
using UnityEditor;
using Zenject;

namespace BroWar.Injection.Editor
{
    using BroWar.Injection.Contexts;

    [CustomEditor(typeof(RootContext)), NoReflectionBaking]
    public class RootContextEditor : SceneContextEditorBase
    {
        private SerializedProperty rootsProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            rootsProperty = serializedObject.FindProperty("roots");
        }

        public override void DrawCustomInspector()
        {
            serializedObject.Update();
            base.DrawCustomInspector();
            ToolboxEditorGui.DrawToolboxProperty(rootsProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}