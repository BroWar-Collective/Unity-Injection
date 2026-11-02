#if !ODIN_INSPECTOR

using System.Linq;
using UnityEditor;

namespace Zenject
{
    [CustomEditor(typeof(SceneDecoratorContext))]
    [NoReflectionBaking]
    public class SceneDecoratorContextEditor : ContextEditor
    {
        SerializedProperty _decoratedContractNameProperty;

        protected override string[] PropertyNames
        {
            get
            {
                return base.PropertyNames.Concat(new string[]
                    {
                        "_lateInstallers"
                    })
                    .ToArray();
            }
        }

        protected override string[] PropertyDisplayNames
        {
            get
            {
                return base.PropertyDisplayNames.Concat(new string[]
                    {
                        "Late Installers"
                    })
                    .ToArray();
            }
        }

        protected override string[] PropertyDescriptions
        {
            get
            {
                return base.PropertyDescriptions.Concat(new string[]
                    {
                        "Drag any MonoInstallers that you have added to your Scene Hierarchy here. They'll be installed after the target installs its bindings"
                    })
                    .ToArray();
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            _decoratedContractNameProperty = serializedObject.FindProperty("_decoratedContractName");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_decoratedContractNameProperty);
        }
    }
}

#endif