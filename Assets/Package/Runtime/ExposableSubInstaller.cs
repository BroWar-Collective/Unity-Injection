using UnityEngine;
using Zenject;

namespace BroWar.Injection
{
    using BroWar.Common;

    /// <summary>
    /// Installer that can be exposed to a parent context, e.g. from <see cref="GameObjectContext"/> to <see cref="SceneContext"/>.
    /// </summary>
    public abstract class ExposableSubInstaller : ISubInstaller
    {
        [SerializeField, Tooltip("If true parent container will be used for binding." +
            " This is useful when we want to expose references outside our context.")]
        private bool bindToParent;

        private DiContainer GetParentContainer(DiContainer target)
        {
            var parents = target.ParentContainers;
            if (parents == null || parents.Length == 0)
            {
                return null;
            }

            return parents[0];
        }

        protected abstract void OnInstall(DiContainer container);

        public void Install(DiContainer container)
        {
            TargetContainer = container;
            ParentContainer = GetParentContainer(container);
            if (BindToParent)
            {
                if (ParentContainer != null)
                {
                    container = ParentContainer;
                }
                else
                {
                    LogHandler.Log("[Injection] Cannot find parent container.", LogType.Warning);
                }
            }

            OnInstall(container);
        }

        protected bool BindToParent
        {
            get => bindToParent;
            set => bindToParent = value;
        }

        /// <summary>
        /// Default <see cref="DiContainer"/> associated to "our" context.
        /// </summary>
        protected DiContainer TargetContainer
        {
            get; private set;
        }

        /// <summary>
        /// Parent <see cref="DiContainer"/> associated with higher context 
        /// (e.g. <see cref="SceneContext"/> if installer is placed within <see cref="GameObjectContext"/>).
        /// </summary>
        protected DiContainer ParentContainer
        {
            get; private set;
        }
    }
}