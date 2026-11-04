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
    [AddComponentMenu("BroWar/Injection/Contexts/Root Scene Context")]
    public class RootSceneContext : SceneContext
    {
        [Title("References")]
        [SerializeField, ReorderableList]
        private GameObject[] roots;

        protected override void GetDefaultInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            for (var i = 0; i < roots.Length; i++)
            {
                var root = roots[i];
                if (root == null)
                {
                    continue;
                }

                ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(root, monoBehaviours);
            }
        }
    }
}