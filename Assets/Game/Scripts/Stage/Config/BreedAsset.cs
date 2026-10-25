using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Stage.Crowd;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Breed")]
    public sealed class BreedAsset : ScriptableObject
    {
        [SerializeField] private string _title = "Monster";
        [SerializeField] private float _health = 100f;
        [SerializeField, Range(0f, 0.9f)] private float _armor;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private int _bounty = 5;
        [SerializeField] private int _harm = 1;
        [SerializeField] private Element[] _immunities = new Element[0];
        [SerializeField] private Body _model;
        [Header("Boss")]
        [SerializeField] private bool _boss;
        [SerializeField] private BreedAsset _follower;
        [SerializeField] private int _followers;
        [SerializeField] private int _calls;
        [SerializeField] private float _spacing = 2f;

        public string Title => _title;

        public bool Boss => _boss;

        public Body Model => _model;

        public bool TryFollower(out BreedAsset follower)
        {
            follower = _follower;
            return _follower != null && _follower != this && _followers > 0 && _calls > 0;
        }

        public Retinue Retinue(Breed follower) => new Retinue(follower, _followers, _calls, _spacing);

        public Breed Breed(Retinue retinue) => new Breed(name, _health, _armor, _speed, _bounty, _harm, _immunities).Leading(retinue);
    }
}
