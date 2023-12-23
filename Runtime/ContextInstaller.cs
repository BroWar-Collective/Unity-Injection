using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BroWar.Injection
{
    using BroWar.Common;

    [AddComponentMenu("BroWar/Injection/Context Installer")]
    public class ContextInstaller : MonoInstaller<ContextInstaller>
    {
        [SerializeReference, ReferencePicker, ReorderableList(elementLabel: "Installer")]
        private ISubInstaller[] installers;

        protected virtual void HandleInstallers()
        {
            var count = installers?.Length ?? 0;
            for (var i = 0; i < count; i++)
            {
                var installer = installers[i];
                if (installer == null)
                {
                    LogHandler.Log($"[Injection][{name}] Installer at '{i}' is invalid.", LogType.Warning);
                    continue;
                }

                installer.Install(Container);
            }
        }

        public override void InstallBindings()
        {
            HandleInstallers();
        }

        public IReadOnlyList<ISubInstaller> Installers => installers;
    }
}