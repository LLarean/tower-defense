using System;
using Game.Core.Arenas;
using Game.Core.Battles;
using Game.Core.Economy;
using Game.Core.Towers;
using Game.Core.Waves;
using Game.Stage.Config;
using Game.Stage.Crowd;
using Game.Stage.Map;
using UnityEngine;

namespace Game.Stage
{
    public sealed class BattleScene : MonoBehaviour
    {
        private const float Step = 1f / 30f;
        private const int MaxSteps = 5;
        private const float Cell = 4f;

        [SerializeField] private LevelAsset _level;
        [SerializeField] private ChemistryAsset _chemistry;
        [SerializeField] private RoutePath[] _routes = new RoutePath[0];
        [SerializeField] private PlotGrid _plots;
        [SerializeField] private Terrain _terrain;
        [SerializeField] private int _players = 1;

        private Battle _battle;
        private Throng _throng;
        private float _pending;

        private void Awake()
        {
            var routes = Array.ConvertAll(_routes, path => path.Route());
            var bestiary = new Bestiary(transform);
            var treasury = new Treasury(_players, _level.Gold);
            var lives = new Lives(_level.Lives);
            var arena = new Arena(_chemistry.Chemistry(), new Horde(Cell), new Feedbacks(new Ledger(treasury, lives)));
            var invasion = new Invasion(routes, _level.Rest, Waves(bestiary));
            var garrison = new Garrison(_plots.Plots(routes), treasury);
            _battle = new Battle(arena, invasion, garrison, lives);
            _throng = new Throng(bestiary.Stocks, _terrain);
        }

        private void Start() => _battle.Begin();

        private void Update()
        {
            _pending = Math.Min(_pending + Time.deltaTime, Step * MaxSteps);
            while (_pending >= Step)
            {
                _battle.Tick(Step);
                _pending -= Step;
            }
            _throng.Sync(_battle.Arena.Horde.Monsters, Time.deltaTime);
        }

        private Wave[] Waves(Bestiary bestiary)
        {
            var waves = new Wave[_level.Waves.Count];
            for (var index = 0; index < waves.Length; index++)
            {
                var data = _level.Waves[index].Squads;
                var squads = new Squad[data.Count];
                for (var squad = 0; squad < squads.Length; squad++)
                {
                    squads[squad] = data[squad].Squad(bestiary.Breed(data[squad].Asset));
                }
                waves[index] = new Wave(squads);
            }
            return waves;
        }
    }
}
