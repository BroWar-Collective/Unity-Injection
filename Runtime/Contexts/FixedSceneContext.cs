using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BroWar.Injection.Contexts
{
    /// <summary>
    /// Optimized version of the <see cref="SceneContext"/> that limits searching for injectable objects to a single <see cref="GameObject"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("BroWar/Injection/Contexts/Fixed Scene Context")]
    public class FixedSceneContext : SceneContext
    {
        [Title("References")]
        [SerializeField, ReorderableList]
        private MonoBehaviour[] injectableBehaviours;

        protected override void GetDefaultInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            monoBehaviours.AddRange(injectableBehaviours);
        }
    }
}