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
using Game.Stage.Net;
using Game.Stage.Orders;
using Game.Stage.Towers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage
{
    public sealed class BattleScene : MonoBehaviour
    {
        private const float Cell = 4f;
        private const string Menu = "Menu";

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
        [SerializeField] private Mesh _bar;
        [SerializeField] private Material _gauge;
        [SerializeField] private int _players = 1;

        private Battle _battle;
        private IPace _pace;
        private Match _match;
        private Throng _throng;
        private Barrage _barrage;
        private Spectacle _show;
        private Gauges _gauges;
        private Bastion _bastion;
        private Hand _hand;
        private Hotkeys _hotkeys;
        private IWidget[] _widgets;

        private void Awake()
        {
            var routes = Array.ConvertAll(_routes, path => path.Route());
            var bestiary = new Bestiary(transform);
            var armory = new Armory(_level.Arsenal);
            var online = TryMatch(out var match);
            _match = match;
            if (online && !match.Hosting)
            {
                match.Manager.OnClientStopped += Abandoned;
            }
            var players = online ? match.Players : _players;
            var me = online ? match.Me : 0;
            var treasury = new Treasury(players, _level.Gold);
            var lives = new Lives(_level.Lives);
            _gauges = new Gauges(_bar, _gauge);
            _throng = new Throng(bestiary.Stocks, _terrain, new Palette(_spectacle), _gauges);
            _show = new Spectacle(_spectacle, _throng, _tremor, transform);
            var arena = new Arena(_chemistry.Chemistry(), new Horde(Cell), new Feedbacks(new Ledger(treasury, lives), _show));
            var invasion = new Invasion(routes, _level.Rest, Waves(bestiary));
            var plots = _plots.Plots(routes);
            _bastion = new Bastion(armory, transform);
            var garrison = new Garrison(plots, treasury, _bastion);
            _battle = new Battle(arena, invasion, garrison, lives);
            _barrage = new Barrage(armory, transform);
            var timeline = new Timeline(_battle, armory.Specs, treasury);
            _pace = !online ? new SoloPace(timeline) : match.Hosting ? new HostPace(timeline, match.Manager) : (IPace)new GuestPace(timeline, match.Manager);
            var orders = new Courier(_pace.Post, armory.Specs, me);
            var root = _document.rootVisualElement;
            var pick = new Pick(garrison, arena.Horde, _selection, _plots.Cell);
            _hand = new Hand(_camera, new Overlay(root), new Blueprint(plots, armory, orders, _ghost, _plots.Cell), pick);
            _hotkeys = new Hotkeys(armory, orders, _hand, pick);
            var wallet = treasury.Wallet(me);
            _widgets = new IWidget[]
            {
                new Purse(root, wallet, lives),
                new Herald(root, _battle, orders, _pace),
                new Arsenal(root, armory, wallet, _hand),
                new Inspector(root, pick, armory, orders),
                online
                    ? new Verdict(root, _battle, "Leave", () => Leave(match))
                    : new Verdict(root, _battle, "Play again", () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)),
            };
            _ghost.Hide();
            _selection.Hide();
        }

        private void Update()
        {
            _hotkeys.Tick();
            _hand.Tick();
            _pace.Advance(Time.deltaTime);
            _gauges.Face(_camera);
            _throng.Sync(_battle.Arena.Horde.Monsters, Time.deltaTime);
            _bastion.Mark(_gauges, _camera);
            _gauges.Draw();
            _barrage.Sync(_battle.Volley.Missiles);
            _show.Tick(Time.deltaTime);
            foreach (var widget in _widgets)
            {
                widget.Refresh();
            }
        }

        private void OnDestroy()
        {
            _pace?.Dispose();
            if (_match != null)
            {
                _match.Manager.OnClientStopped -= Abandoned;
            }
        }

        private void Abandoned(bool host) => SceneManager.LoadScene(Menu);

        private static bool TryMatch(out Match match)
        {
            var manager = NetworkManager.Singleton;
            match = null;
            return manager != null && manager.IsListening && manager.TryGetComponent(out match);
        }

        private static void Leave(Match match)
        {
            match.Close();
            SceneManager.LoadScene(Menu);
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
