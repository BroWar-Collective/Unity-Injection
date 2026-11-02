#if !ODIN_INSPECTOR

namespace Zenject
{
    [NoReflectionBaking]
    public class ContextEditor : UnityInspectorListEditor
    {
        protected override string[] PropertyNames
        {
            get
            {
                return new string[]
                {
                    "_monoInstallers"
                };
            }
        }

        protected override string[] PropertyDisplayNames
        {
            get
            {
                return new string[]
                {
                    "Mono Installers",
                };
            }
        }

        protected override string[] PropertyDescriptions
        {
            get
            {
                return new string[]
                {
                    "Drag any MonoInstallers that you have added to your Scene Hierarchy here."
                };
            }
        }
    }
}

#endif