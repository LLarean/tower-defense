using System.Collections.Generic;
using Game.Core.Towers;
using UnityEngine;

namespace Game.Stage.Towers
{
    public sealed class Bastion : IGarrisonFeedback
    {
        private const float Growth = 0.12f;

        private readonly Armory _armory;
        private readonly Transform _root;
        private readonly Dictionary<Tower, GameObject> _models = new Dictionary<Tower, GameObject>();

        public Bastion(Armory armory, Transform root)
        {
            _armory = armory;
            _root = root;
        }

        public void Built(Tower tower)
        {
            var prefab = _armory.Asset(tower.Spec).Model;
            _models.Add(tower, Object.Instantiate(prefab, tower.Position.ToUnity(), Quaternion.identity, _root));
        }

        public void Upgraded(Tower tower)
        {
            var model = _models[tower].transform;
            var prefab = _armory.Asset(tower.Spec).Model.transform;
            model.localScale = prefab.localScale * (1f + Growth * tower.Level);
        }

        public void Sold(Tower tower)
        {
            Object.Destroy(_models[tower]);
            _models.Remove(tower);
        }
    }
}
