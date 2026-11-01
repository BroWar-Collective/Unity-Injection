using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        static ListPool<T> _instance = new ListPool<T>();

        public ListPool()
        {
            OnDespawnedMethod = OnDespawned;
        }

        public static ListPool<T> Instance
        {
            get { return _instance; }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStaticValues()
        {
            _instance = null;
        }

        void OnDespawned(List<T> list)
        {
            list.Clear();
        }
    }
}