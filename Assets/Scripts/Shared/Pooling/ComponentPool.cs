using System.Collections.Generic;
using UnityEngine;

namespace Project.Shared.Pooling
{
    /// <summary>
    /// Simple component pool for UI/game objects.
    /// Uses Stack for LIFO reuse, and Rent/Release naming.
    /// </summary>
    public sealed class ComponentPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Stack<T> _stack;

        public int CountInactive => _stack.Count;

        public ComponentPool(T prefab, int preloadCount = 0, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _stack = new Stack<T>(Mathf.Max(0, preloadCount));

            Preload(preloadCount);
        }

        private void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(_prefab, _parent);
                instance.gameObject.SetActive(false);
                _stack.Push(instance);
            }
        }

        public T Rent()
        {
            if (_stack.Count > 0)
            {
                var instance = _stack.Pop();
                instance.gameObject.SetActive(true);
                return instance;
            }

            return Object.Instantiate(_prefab, _parent);
        }

        public void Release(T instance)
        {
            if (instance == null) return;

            instance.gameObject.SetActive(false);
            _stack.Push(instance);
        }

        public void DisposeAll()
        {
            while (_stack.Count > 0)
            {
                var instance = _stack.Pop();
                if (instance != null)
                    Object.Destroy(instance.gameObject);
            }
        }
    }
}
