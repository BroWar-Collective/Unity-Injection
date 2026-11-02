#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zenject
{
    public abstract class Context : MonoBehaviour
    {
        [FormerlySerializedAs("Installers")]
        [FormerlySerializedAs("_installers")]
        [SerializeField]
        List<MonoInstaller> _monoInstallers = new List<MonoInstaller>();

        List<InstallerBase> _normalInstallers = new List<InstallerBase>();
        List<Type> _normalInstallerTypes = new List<Type>();

        public IEnumerable<MonoInstaller> Installers
        {
            get { return _monoInstallers; }
            set
            {
                _monoInstallers.Clear();
                _monoInstallers.AddRange(value);
            }
        }

        // Unlike other installer types this has to be set through code
        public IEnumerable<Type> NormalInstallerTypes
        {
            get { return _normalInstallerTypes; }
            set
            {
                Assert.That(value.All(x => x != null && x.DerivesFrom<InstallerBase>()));

                _normalInstallerTypes.Clear();
                _normalInstallerTypes.AddRange(value);
            }
        }

        // Unlike other installer types this has to be set through code
        public IEnumerable<InstallerBase> NormalInstallers
        {
            get { return _normalInstallers; }
            set
            {
                _normalInstallers.Clear();
                _normalInstallers.AddRange(value);
            }
        }

        public Action<List<MonoBehaviour>> GetInjectableMonoBehavioursCallback 
        { 
            get;
            set;
        }

        public abstract DiContainer Container { get; }
        public abstract IEnumerable<GameObject> GetRootGameObjects();

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            // When Scene Reloading is disabled in Enter The Play Mode settings, we need to reset all non-serialized fields
            // https://docs.unity3d.com/Manual/SceneReloading.html
            EditorApplication.playModeStateChanged += OnEditorPlayModeChanged;
#endif
        }

#if UNITY_EDITOR
        void OnEditorPlayModeChanged(PlayModeStateChange obj)
        {
            if (!EditorSettings.enterPlayModeOptionsEnabled)
                return;

            // If Scene Reload is enabled, we don't need to reset instance fields.
            if ((EditorSettings.enterPlayModeOptions & EnterPlayModeOptions.DisableSceneReload) == 0)
                return;

            if (obj == PlayModeStateChange.EnteredEditMode)
                ResetInstanceFields();
        }

        protected virtual void ResetInstanceFields()
        {
            _normalInstallers.Clear();
            _normalInstallerTypes.Clear();
        }
#endif

        public void AddNormalInstallerType(Type installerType)
        {
            Assert.IsNotNull(installerType);
            Assert.That(installerType.DerivesFrom<InstallerBase>());

            _normalInstallerTypes.Add(installerType);
        }

        public void AddNormalInstaller(InstallerBase installer)
        {
            _normalInstallers.Add(installer);
        }

        protected void InstallInstallers()
        {
            InstallInstallers(
                _normalInstallers, _normalInstallerTypes, _monoInstallers);
        }

        protected void InstallInstallers(
            List<InstallerBase> normalInstallers,
            List<Type> normalInstallerTypes,
            List<MonoInstaller> installers)
        {
            // Ideally we would just have one flat list of all the installers
            // since that way the user has complete control over the order, but
            // that's not possible since Unity does not allow serializing lists of interfaces
            // (and it has to be an inteface since the scriptable object installers only share
            // the interface)
            //
            // So the best we can do is have a hard-coded order in terms of the installer type
            //
            // The order is:
            //      - Normal installers given directly via code
            //      - ScriptableObject installers
            //      - MonoInstallers in the scene
            //      - Prefab Installers
            //
            // We put ScriptableObject installers before the MonoInstallers because
            // ScriptableObjectInstallers are often used for settings (including settings
            // that are injected into other installers like MonoInstallers)

            var allInstallers = normalInstallers.Cast<IInstaller>()
                .Concat(installers.Cast<IInstaller>()).ToList();

            foreach (var installerType in normalInstallerTypes)
            {
                var installer = (InstallerBase)Container.Instantiate(installerType);

#if ZEN_INTERNAL_PROFILING
                using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                {
                    installer.InstallBindings();
                }
            }

            foreach (var installer in allInstallers)
            {
                Assert.IsNotNull(installer,
                    "Found null installer in '{0}'", GetType());

                Container.Inject(installer);

#if ZEN_INTERNAL_PROFILING
                using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                {
                    installer.InstallBindings();
                }
            }
        }

        protected void InstallSceneBindings(List<MonoBehaviour> injectableMonoBehaviours)
        {
            foreach (var binding in injectableMonoBehaviours.OfType<ZenjectBinding>())
            {
                if (binding == null)
                {
                    continue;
                }

                if (binding.Context == null || (binding.UseSceneContext && this is SceneContext))
                {
                    binding.Context = this;
                }
            }

            // TODO: Consider changing this
            // Maybe ZenjectBinding could add itself to a registry class on Awake/OnEnable
            // then we could avoid calling the slow Resources.FindObjectsOfTypeAll here
#if UNITY_2020_1_OR_NEWER
            foreach (var binding in FindObjectsOfType<ZenjectBinding>(true))
#else
            foreach (var binding in Resources.FindObjectsOfTypeAll<ZenjectBinding>())
#endif
            {
                if (binding == null)
                {
                    continue;
                }

                // This is necessary for cases where the ZenjectBinding is inside a GameObjectContext
                // since it won't be caught in the other loop above
                if (this is SceneContext)
                {
                    if (binding.Context == null && binding.UseSceneContext
                                                && binding.gameObject.scene == gameObject.scene)
                    {
                        binding.Context = this;
                    }
                }

                if (binding.Context == this)
                {
                    InstallZenjectBinding(binding);
                }
            }
        }

        void InstallZenjectBinding(ZenjectBinding binding)
        {
            if (!binding.enabled)
            {
                return;
            }

            if (binding.Components == null || binding.Components.IsEmpty())
            {
                Log.Warn("Found empty list of components on ZenjectBinding on object '{0}'", binding.name);
                return;
            }

            string identifier = null;

            if (binding.Identifier.Trim().Length > 0)
            {
                identifier = binding.Identifier;
            }

            foreach (var component in binding.Components)
            {
                var bindType = binding.BindType;

                if (component == null)
                {
                    Log.Warn("Found null component in ZenjectBinding on object '{0}'", binding.name);
                    continue;
                }

                var componentType = component.GetType();

                switch (bindType)
                {
                    case ZenjectBinding.BindTypes.Self:
                        {
                            Container.Bind(componentType).WithId(identifier).FromInstance(component);
                            break;
                        }
                    case ZenjectBinding.BindTypes.BaseType:
                        {
                            Container.Bind(componentType.BaseType()).WithId(identifier).FromInstance(component);
                            break;
                        }
                    case ZenjectBinding.BindTypes.AllInterfaces:
                        {
                            Container.Bind(componentType.Interfaces()).WithId(identifier).FromInstance(component);
                            break;
                        }
                    case ZenjectBinding.BindTypes.AllInterfacesAndSelf:
                        {
                            Container.Bind(componentType.Interfaces().Concat(new[] { componentType }).ToArray())
                                .WithId(identifier).FromInstance(component);
                            break;
                        }
                    default:
                        {
                            throw Assert.CreateException();
                        }
                }
            }
        }

        protected virtual void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            if (GetInjectableMonoBehavioursCallback != null)
            {
                GetInjectableMonoBehavioursCallback(monoBehaviours);
                return;
            }

            GetDefaultInjectableMonoBehaviours(monoBehaviours);
        }

        protected abstract void GetDefaultInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours);
    }
}

#endif