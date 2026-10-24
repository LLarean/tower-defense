using UnityEngine;

namespace Game.Stage.Crowd
{
    public sealed class Puppet
    {
        private const float Turn = 10f;

        private readonly Transform _body;

        public Puppet(GameObject body, Stock stock)
        {
            _body = body.transform;
            Body = body;
            Stock = stock;
        }

        public GameObject Body { get; }

        public Stock Stock { get; }

        public int Seen { get; set; }

        public void Follow(Vector3 position, float seconds)
        {
            var heading = position - _body.position;
            heading.y = 0f;
            var rotation = _body.rotation;
            if (heading.sqrMagnitude > 0.0001f)
            {
                rotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(heading), Turn * seconds);
            }
            _body.SetPositionAndRotation(position, rotation);
        }
    }
}
