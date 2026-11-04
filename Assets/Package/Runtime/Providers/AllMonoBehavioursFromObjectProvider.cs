using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject.Internal;

namespace BroWar.Injection.Providers
{
    [Serializable]
    public class AllMonoBehavioursFromObjectProvider : IInjectablesProvider
    {
        [SerializeField]
        private GameObject root;

        public void ProvideInjectables(List<object> injectables)
        {
            var monoBehaviours = ListPool<MonoBehaviour>.Get();
            var transform = root.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child != null)
                {
                    ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(child.gameObject, monoBehaviours);
                }
            }

            ListPool<MonoBehaviour>.Release(monoBehaviours);
        }
    }
}