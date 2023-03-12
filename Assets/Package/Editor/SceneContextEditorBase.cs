using Toolbox.Editor;
using UnityEditor;
using UnityEngine;

namespace BroWar.Injection.Editor
{
    public abstract class SceneContextEditorBase : ToolboxEditor
    {
        private readonly GUIContent installersLabel = new GUIContent("Installers");

        private SerializedProperty autoRun;
        private SerializedProperty installers;
        private SerializedProperty contractNameProperty;
        private SerializedProperty parentContractNamesProperty;
        private SerializedProperty parentNewObjectsUnderSceneContextProperty;

        protected virtual void OnEnable()
        {
            autoRun = serializedObject.FindProperty("_autoRun");
            installers = serializedObject.FindProperty("_monoInstallers");
            contractNameProperty = serializedObject.FindProperty("_contractNames");
            parentContractNamesProperty = serializedObject.FindProperty("_parentContractNames");
            parentNewObjectsUnderSceneContextProperty = serializedObject.FindProperty("_parentNewObjectsUnderSceneContext");
        }

        public override void DrawCustomInspector()
        {
            EditorGUILayout.PropertyField(autoRun);
            EditorGUILayout.PropertyField(parentNewObjectsUnderSceneContextProperty);
            EditorGUILayout.PropertyField(installers, installersLabel, true);
            EditorGUILayout.PropertyField(contractNameProperty, true);
            EditorGUILayout.PropertyField(parentContractNamesProperty, true);
        }
    }
}