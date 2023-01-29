using BroWar.Common;
using UnityEngine;

namespace BroWar.Injection
{
    public abstract class ProcessableInstaller<T0, T1> : ContextInstaller where T1 : ISubProcessor<T0>
    {
        [Tooltip("We can use custom processors to run custom logic before and after the installation.")]
        [SerializeReference, ReferencePicker(TypeGrouping = TypeGrouping.ByFlatName), ReorderableList(elementLabel: "Processor")]
        protected T1[] processors;

        protected virtual void BeginInstallation(T0 data)
        {
            if (processors == null)
            {
                return;
            }

            for (var i = 0; i < processors.Length; i++)
            {
                var processor = processors[i];
                if (processor == null)
                {
                    LogHandler.Log($"[Injection][{name}] Processor at '{i}' is invalid.", LogType.Warning);
                    continue;
                }

                BeginInstallation(data, processor);
            }
        }

        protected virtual void CloseInstallation(T0 data)
        {
            if (processors == null)
            {
                return;
            }

            for (var i = 0; i < processors.Length; i++)
            {
                var processor = processors[i];
                if (processor == null)
                {
                    LogHandler.Log($"[Injection][{name}] Processor at '{i}' is invalid.", LogType.Warning);
                    continue;
                }

                CloseInstallation(data, processor);
            }
        }

        protected virtual void InstallBindings(T0 data)
        {
            BeginInstallation(data);
            base.InstallBindings();
            CloseInstallation(data);
        }

        protected abstract void BeginInstallation(T0 data, T1 processor);
        protected abstract void CloseInstallation(T0 data, T1 processor);
        protected abstract T0 GetProcessingData();

        public sealed override void InstallBindings()
        {
            var data = GetProcessingData();
            InstallBindings(data);
        }
    }
}