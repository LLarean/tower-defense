using Game.Core.Elements;
using Game.Core.Monsters;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Breed")]
    public sealed class BreedAsset : ScriptableObject
    {
        [SerializeField] private float _health = 100f;
        [SerializeField, Range(0f, 0.9f)] private float _armor;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private int _bounty = 5;
        [SerializeField] private int _harm = 1;
        [SerializeField] private Element[] _immunities = new Element[0];
        [SerializeField] private Transform _model;

        public Transform Model => _model;

        public Breed Breed() => new Breed(name, _health, _armor, _speed, _bounty, _harm, _immunities);
    }
}
