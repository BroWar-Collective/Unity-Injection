using UnityEngine;
using Zenject;

namespace BroWar.Injection
{
    [AddComponentMenu("BroWar/Injection/General Installer")]
    public class GeneralInstaller : MonoInstaller<GeneralInstaller>
    {
        [SerializeReference, ReferencePicker(TypeGrouping = TypeGrouping.ByFlatName), ReorderableList(elementLabel: "Installer")]
        private IMinorInstaller[] installers;

        public override void InstallBindings()
        {
            if (installers == null)
            {
                return;
            }

            for (var i = 0; i < installers.Length; i++)
            {
                installers[i].Install(Container);
            }
        }
    }
}