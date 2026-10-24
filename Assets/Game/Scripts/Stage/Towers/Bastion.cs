using System.Collections.Generic;
using Game.Core.Towers;
using Game.Stage.Crowd;
using UnityEngine;

namespace Game.Stage.Towers
{
    public sealed class Bastion : IGarrisonFeedback
    {
        private const float Growth = 0.12f;
        private const float Height = 6.5f;
        private const float Pip = 0.45f;

        private static readonly Vector2 Bar = new Vector2(2f, 0.2f);
        private static readonly Vector2 Pips = new Vector2(0.35f, 0.25f);
        private static readonly Color Charge = new Color(0.35f, 0.8f, 1f);
        private static readonly Color Rank = new Color(1f, 0.82f, 0.3f);

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

        public void Mark(Gauges gauges, Camera camera)
        {
            var right = camera.transform.right;
            foreach (var tower in _models.Keys)
            {
                var top = tower.Position.ToUnity() + Vector3.up * Height;
                gauges.Add(top, Bar, tower.Readiness, Charge);
                for (var level = 0; level <= tower.Level; level++)
                {
                    var offset = (level - tower.Level * 0.5f) * Pip;
                    gauges.Add(top + Vector3.up * 0.35f + right * offset, Pips, 1f, Rank);
                }
            }
        }

        public void Sold(Tower tower)
        {
            Object.Destroy(_models[tower]);
            _models.Remove(tower);
        }
    }
}
