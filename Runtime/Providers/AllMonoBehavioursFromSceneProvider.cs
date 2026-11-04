using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject.Internal;

namespace BroWar.Injection.Providers
{
    [Serializable]
    public class AllMonoBehavioursFromSceneProvider : IInjectablesProvider
    {
        [SerializeField]
        private GameObject root;

        public void ProvideInjectables(List<object> injectables)
        {
            var scene = root.scene;

            var monoBehaviours = ListPool<MonoBehaviour>.Get();
            ZenUtilInternal.GetInjectableMonoBehavioursInScene(scene, monoBehaviours);
            foreach (var monoBehaviour in monoBehaviours)
            {
                injectables.Add(monoBehaviour);
            }

            ListPool<MonoBehaviour>.Release(monoBehaviours);
        }
    }
}