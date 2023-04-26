using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Zenject.Internal;

namespace BroWar.Injection
{
    using BroWar.Common;

    /// <summary>
    /// Manager responsible for handling custom injection-based setup.
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    [AddComponentMenu("BroWar/Injection/Injection Manager")]
    public class InjectionManager : StandaloneManager, IInjectionManager
    {
        [Title("Settings")]
        [SerializeField]
        private bool initializeContext = true;
        [SerializeField, EnableIf(nameof(initializeContext), true)]
        private ProjectContext projectContextPrefab;

        private void Awake()
        {
            InitializeContext();
        }

        /// <summary>
        /// Initializes and overrides <see cref="ProjectContext.Instance"/>.
        /// This approach is much faster.
        /// </summary>
        private void InitializeContext()
        {
            if (ProjectContext.HasInstance || !initializeContext)
            {
                return;
            }

            Assert.IsNotNull(ProjectContextPrefab, $"[Injection] {nameof(ProjectContextPrefab)} not available.");
            var prefabWasActive = projectContextPrefab.gameObject.activeSelf;

            ProjectContext instance;
#if UNITY_EDITOR
            if (prefabWasActive)
            {
                //NOTE: this ensures the prefab's Awake() methods don't fire (and, if in the Editor, that the prefab file doesn't get modified)
                instance = Instantiate(projectContextPrefab, ZenUtilInternal.GetOrCreateInactivePrefabParent());
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(null, false);
            }
            else
            {
                instance = Instantiate(projectContextPrefab);
            }
#else
            if (prefabWasActive)
            {
                projectContextPrefab.gameObject.SetActive(false);
                instance = Instantiate(projectContextPrefab);
                projectContextPrefab.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(projectContextPrefab);
            }
#endif

            ProjectContext.Instance = instance;
            instance.name = "Project Context";
            instance.Initialize();

            if (prefabWasActive)
            {
                instance.gameObject.SetActive(true);
            }
        }

        /// <inheritdoc />
        public ProjectContext ProjectContextPrefab
        {
            get => projectContextPrefab;
            set => projectContextPrefab = value;
        }
    }
}