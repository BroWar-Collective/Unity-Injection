using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Internal;

namespace BroWar.Injection.Contexts
{
    /// <summary>
    /// Optimized version of the <see cref="SceneContext"/> that limits searching for injectable objects to a single <see cref="GameObject"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("BroWar/Injection/Contexts/Root Context")]
    public class RootContext : SceneContext
    {
        [SerializeField]
        private GameObject root;

        protected override void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            if (root == null)
            {
                base.GetInjectableMonoBehaviours(monoBehaviours);
                return;
            }

            ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(root, monoBehaviours);
        }
    }
}