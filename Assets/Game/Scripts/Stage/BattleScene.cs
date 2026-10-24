using System;
using Game.Core.Arenas;
using Game.Core.Battles;
using Game.Core.Economy;
using Game.Core.Towers;
using Game.Core.Waves;
using Game.Stage.Config;
using Game.Stage.Controls;
using Game.Stage.Crowd;
using Game.Stage.Effects;
using Game.Stage.Hud;
using Game.Stage.Map;
using Game.Stage.Orders;
using Game.Stage.Towers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage
{
    public sealed class BattleScene : MonoBehaviour
    {
        private const int MaxSteps = 5;
        private const float Cell = 4f;
        private const int Player = 0;

        [SerializeField] private LevelAsset _level;
        [SerializeField] private ChemistryAsset _chemistry;
        [SerializeField] private SpectacleAsset _spectacle;
        [SerializeField] private Tremor _tremor;
        [SerializeField] private RoutePath[] _routes = new RoutePath[0];
        [SerializeField] private PlotGrid _plots;
        [SerializeField] private Terrain _terrain;
        [SerializeField] private Camera _camera;
        [SerializeField] private Ghost _ghost;
        [SerializeField] private Ring _selection;
        [SerializeField] private UIDocument _document;
        [SerializeField] private int _players = 1;

        private Battle _battle;
        private Timeline _timeline;
        private Throng _throng;
        private Barrage _barrage;
        private Spectacle _show;
        private Hand _hand;
        private Hotkeys _hotkeys;
        private IWidget[] _widgets;
        private float _pending;

        private void Awake()
        {
            var routes = Array.ConvertAll(_routes, path => path.Route());
            var bestiary = new Bestiary(transform);
            var armory = new Armory(_level.Arsenal);
            var treasury = new Treasury(_players, _level.Gold);
            var lives = new Lives(_level.Lives);
            _throng = new Throng(bestiary.Stocks, _terrain, new Palette(_spectacle));
            _show = new Spectacle(_spectacle, _throng, _tremor, transform);
            var arena = new Arena(_chemistry.Chemistry(), new Horde(Cell), new Feedbacks(new Ledger(treasury, lives), _show));
            var invasion = new Invasion(routes, _level.Rest, Waves(bestiary));
            var plots = _plots.Plots(routes);
            var garrison = new Garrison(plots, treasury, new Bastion(armory, transform));
            _battle = new Battle(arena, invasion, garrison, lives);
            _barrage = new Barrage(armory, transform);
            _timeline = new Timeline(_battle, armory.Specs, treasury);
            var orders = new Courier(new LocalPost(_timeline), armory.Specs, Player);
            var root = _document.rootVisualElement;
            var pick = new Pick(garrison, _selection, _plots.Cell);
            _hand = new Hand(_camera, new Overlay(root), new Blueprint(plots, armory, orders, _ghost, _plots.Cell), pick);
            _hotkeys = new Hotkeys(armory, orders, _hand, pick);
            var wallet = treasury.Wallet(Player);
            _widgets = new IWidget[]
            {
                new Purse(root, wallet, lives),
                new Herald(root, _battle, orders),
                new Arsenal(root, armory, wallet, _hand),
                new Inspector(root, pick, armory, orders),
                new Verdict(root, _battle),
            };
            _ghost.Hide();
            _selection.Hide();
        }

        private void Update()
        {
            _hotkeys.Tick();
            _hand.Tick();
            _pending = Math.Min(_pending + Time.deltaTime, Timeline.Step * MaxSteps);
            while (_pending >= Timeline.Step && _timeline.TryStep())
            {
                _pending -= Timeline.Step;
            }
            _throng.Sync(_battle.Arena.Horde.Monsters, Time.deltaTime);
            _barrage.Sync(_battle.Volley.Missiles);
            _show.Tick(Time.deltaTime);
            foreach (var widget in _widgets)
            {
                widget.Refresh();
            }
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
