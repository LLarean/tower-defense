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
using Game.Stage.Sound;
using Game.Stage.Story;
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
        [SerializeField] private DifficultyAsset _difficulty;
        [SerializeField] private CampaignAsset _campaign;
        [SerializeField] private ChemistryAsset _chemistry;
        [SerializeField] private SpectacleAsset _spectacle;
        [SerializeField] private LoreAsset _lore;
        [SerializeField] private SoundboardAsset _soundboard;
        [SerializeField] private Tremor _tremor;
        [SerializeField] private RoutePath[] _routes = new RoutePath[0];
        [SerializeField] private PlotGrid _plots;
        [SerializeField] private Terrain _terrain;
        [SerializeField] private Camera _camera;
        [SerializeField] private Rig _rig;
        [SerializeField] private Ghost _ghost;
        [SerializeField] private Ring _selection;
        [SerializeField] private UIDocument _document;
        [SerializeField] private Mesh _bar;
        [SerializeField] private Material _gauge;
        [SerializeField] private int _players = 1;
        [SerializeField] private float[] _speeds = { 1f, 2f, 3f };

        private Battle _battle;
        private IPace _pace;
        private Match _match;
        private Throng _throng;
        private Barrage _barrage;
        private Spectacle _show;
        private Callouts _callouts;
        private Gauges _gauges;
        private Bastion _bastion;
        private Hand _hand;
        private Hotkeys _hotkeys;
        private IWidget[] _widgets;

        private void Awake()
        {
            var routes = Array.ConvertAll(_routes, path => path.Route());
            var briefed = TryMatch(out var match);
            var online = briefed && match.Online;
            _match = match;
            if (online && !match.Hosting)
            {
                match.Manager.OnClientStopped += Abandoned;
            }
            var players = online ? match.Players : _players;
            var me = online ? match.Me : 0;
            var tier = briefed ? match.Difficulty : _difficulty.Default;
            var bestiary = new Bestiary(transform, _difficulty.Threat(tier, players));
            var armory = new Armory(_level.Arsenal);
            var treasury = new Treasury(players, _difficulty.Gold(tier, _level.Gold));
            var lives = new Lives(_difficulty.Lives(tier, _level.Lives));
            _gauges = new Gauges(_bar, _gauge);
            _throng = new Throng(bestiary.Stocks, _terrain, new Palette(_spectacle), _gauges);
            _show = new Spectacle(_spectacle, _throng, _tremor, transform);
            var root = _document.rootVisualElement;
            root.pickingMode = PickingMode.Ignore;
            _callouts = Callouts(root);
            var voices = new Voices(transform, _soundboard.Voices, new Setting(root.Q<Button>("sound"), "Sound", "sound"));
            var acoustics = new Acoustics(_soundboard, voices);
            var chemistry = _chemistry.Chemistry();
            var arena = new Arena(chemistry, new Horde(Cell), new Feedbacks(new Ledger(treasury, lives), _show, _callouts, acoustics));
            var invasion = new Invasion(routes, _level.Rest, Waves(bestiary));
            var plots = _plots.Plots(routes);
            _bastion = new Bastion(armory, transform);
            var garrison = new Garrison(plots, treasury, new GarrisonFeedbacks(_bastion, acoustics));
            _battle = new Battle(arena, invasion, garrison, lives);
            _barrage = new Barrage(armory, transform, acoustics);
            var timeline = new Timeline(_battle, armory.Specs, treasury);
            _pace = !online ? new SoloPace(timeline) : match.Hosting ? new HostPace(timeline, match.Manager) : (IPace)new GuestPace(timeline, match.Manager);
            var orders = new Courier(_pace.Post, armory.Specs, me);
            var pick = new Pick(garrison, arena.Horde, _selection, _plots.Cell);
            var overlay = new Overlay(root);
            _rig.Attend(overlay);
            _hand = new Hand(_camera, overlay, new Blueprint(plots, armory, orders, _ghost, _plots.Cell), pick);
            var recipes = new Recipes(chemistry, _lore);
            var codex = new Codex(root, recipes, _lore);
            root.Q<Button>("codex").clicked += codex.Open;
            var pause = new Pause(root, new Tempo(root.Q<Button>("speed"), _speeds, online), online, () => Leave(match));
            _hotkeys = new Hotkeys(armory, orders, _hand, pause);
            var wallet = treasury.Wallet(me);
            new Cheats(root, _battle, wallet, orders);
            _widgets = new IWidget[]
            {
                new Purse(root, wallet, lives),
                new Herald(root, _battle, orders, _pace),
                new Arsenal(root, armory, wallet, _hand),
                new Inspector(root, pick, armory, orders),
                new Briefing(root, _battle, _level, _difficulty.Title(tier)),
                new Coach(root, _battle),
                new Bossbar(root, arena.Horde, bestiary),
                Debrief(root, online, match),
                new Fanfare(_battle, _soundboard, voices),
                new Melody(transform, _soundboard.Battle, _soundboard.Music, new Setting(root.Q<Button>("music"), "Music", "music")),
            };
            new Tooltip(root, armory, recipes, _lore);
            new Chime(root, voices, _soundboard.Click);
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
            _callouts.Tick(Time.deltaTime);
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
            return manager != null && manager.TryGetComponent(out match);
        }

        private static void Leave(Match match)
        {
            if (match != null && match.Online)
            {
                match.Close();
            }
            SceneManager.LoadScene(Menu);
        }

        private Verdict Debrief(VisualElement root, bool online, Match match)
        {
            var again = root.Q<Button>("again");
            again.text = online ? "Leave" : "Play again";
            again.clicked += online ? () => Leave(match) : () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            var exit = root.Q<Button>("verdict-menu");
            exit.EnableInClassList("hidden", online);
            exit.clicked += () => Leave(match);
            var next = root.Q<Button>("verdict-next");
            if (_campaign.TryNext(_level, out var following) && (!online || match.Hosting))
            {
                next.clicked += online ? () => match.Begin(following.Scene) : () => SceneManager.LoadScene(following.Scene);
            }
            else
            {
                next.RemoveFromHierarchy();
            }
            var chronicle = new Chronicle();
            return new Verdict(root, _battle, (_level.Victory, _campaign.Defeat), () => chronicle.Record(_level));
        }

        private Callouts Callouts(VisualElement root)
        {
            var layer = root.Q("captions");
            var numbers = new Setting(root.Q<Button>("numbers"), "Damage numbers", "numbers");
            var tally = new Tally(_spectacle, new Captions(layer, _camera, _spectacle.Numbers), numbers);
            return new Callouts(_spectacle, _lore, new Captions(layer, _camera, _spectacle.Titles), tally);
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
