using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage
{
    public sealed class Stock
    {
        private readonly GameObject _prefab;
        private readonly Transform _root;
        private readonly Stack<GameObject> _idle = new Stack<GameObject>();

        public Stock(GameObject prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }

        public GameObject Take(Vector3 position, Quaternion rotation)
        {
            if (_idle.Count == 0)
            {
                return Object.Instantiate(_prefab, position, rotation, _root);
            }
            var item = _idle.Pop();
            item.transform.SetPositionAndRotation(position, rotation);
            item.SetActive(true);
            return item;
        }

        public void Give(GameObject item)
        {
            item.SetActive(false);
            _idle.Push(item);
        }
    }
}
