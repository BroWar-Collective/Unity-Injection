using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BroWar.Injection.Contexts
{
    /// <summary>
    /// Optimized version of the <see cref="SceneContext"/> that limits searching for injectable objects to a single <see cref="GameObject"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("BroWar/Injection/Contexts/Fixed Context")]
    public class FixedContext : SceneContext
    {
        [Title("References")]
        [SerializeField, ReorderableList]
        private MonoBehaviour[] injectableBehaviours;

        protected override void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            monoBehaviours.AddRange(injectableBehaviours);
        }
    }
}