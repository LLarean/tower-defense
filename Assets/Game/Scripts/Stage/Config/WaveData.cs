using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage.Config
{
    [Serializable]
    public struct WaveData
    {
        [SerializeField] private SquadData[] _squads;

        public IReadOnlyList<SquadData> Squads => _squads;
    }
}
