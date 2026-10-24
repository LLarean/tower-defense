using System;
using Game.Core.Monsters;
using Game.Core.Waves;
using UnityEngine;

namespace Game.Stage.Config
{
    [Serializable]
    public struct SquadData
    {
        [SerializeField] private BreedAsset _breed;
        [SerializeField] private int _count;
        [SerializeField] private float _interval;
        [SerializeField] private float _delay;
        [SerializeField] private int _route;

        public BreedAsset Asset => _breed;

        public Squad Squad(Breed breed) => new Squad(breed, _count, _interval, _delay, _route);
    }
}
