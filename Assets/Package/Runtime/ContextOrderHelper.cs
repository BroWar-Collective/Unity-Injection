using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;

namespace BroWar.Injection
{
    /// <summary>
    /// Custom helper class that can be used to prewarm data or injection order before the installation process.
    /// </summary>
    [DefaultExecutionOrder(-100000)]
    [AddComponentMenu("BroWar/Injection/Context Order Helper")]
    public class ContextOrderHelper : MonoBehaviour
    {
        [SerializeField]
        private SceneContext sceneContext;

        [Line]
        [SerializeField, ReorderableList(HasLabels = false)]
        [EditorButton(nameof(FindContexts))]
        private Object[] objectsToQueue;

        private void Awake()
        {
            Assert.IsNotNull(sceneContext, "[Injection] Context not assigned.");
            Assert.IsFalse(sceneContext.Initialized, "[Injection] Context is already initialized.");
            sceneContext.OnPreInstall.AddListener(OnPreInstall);
        }

        private void QueueObjects()
        {
            var container = sceneContext.Container;
            foreach (var target in objectsToQueue)
            {
                if (target == null)
                {
                    continue;
                }

                container.QueueForInject(target);
            }
        }

        private void OnPreInstall()
        {
            QueueObjects();
        }

        private void FindContexts()
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Find Contexts");
#endif
            var targets = new List<Object>(objectsToQueue);
            targets.AddRange(FindObjectsOfType<GameObjectContext>());
            objectsToQueue = targets.ToArray();
        }
    }
}