using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage
{
    public sealed class Stock<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _root;
        private readonly Stack<T> _idle = new Stack<T>();

        public Stock(T prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }

        public T Take(Vector3 position, Quaternion rotation)
        {
            if (_idle.Count == 0)
            {
                return Object.Instantiate(_prefab, position, rotation, _root);
            }
            var item = _idle.Pop();
            item.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.SetActive(true);
            return item;
        }

        public void Give(T item)
        {
            item.gameObject.SetActive(false);
            _idle.Push(item);
        }
    }
}
