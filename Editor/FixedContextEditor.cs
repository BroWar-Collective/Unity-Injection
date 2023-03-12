using Toolbox.Editor;
using UnityEditor;
using Zenject;

namespace BroWar.Injection.Editor
{
    using BroWar.Injection.Contexts;

    [CustomEditor(typeof(FixedContext)), NoReflectionBaking]
    public class FixedContextEditor : SceneContextEditorBase
    {
        private SerializedProperty injectableBehavioursProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            injectableBehavioursProperty = serializedObject.FindProperty("injectableBehaviours");
        }

        public override void DrawCustomInspector()
        {
            serializedObject.Update();
            base.DrawCustomInspector();
            ToolboxEditorGui.DrawToolboxProperty(injectableBehavioursProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}