using Game.Stage.Config;
using Game.Stage.Hud;
using Game.Stage.Net;
using Game.Stage.Sound;
using Game.Stage.Story;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage.Menu
{
    public sealed class MenuScene : MonoBehaviour
    {
        private const string Front = "front";
        private const string Prologue = "prologue";
        private const string Board = "missions";
        private const string Coop = "coop-page";
        private const string Settings = "settings-page";

        [SerializeField] private UIDocument _document;
        [SerializeField] private Match _match;
        [SerializeField] private UnityTransport _transport;
        [SerializeField] private ushort _port = 7777;
        [SerializeField] private ChemistryAsset _chemistry;
        [SerializeField] private LoreAsset _lore;
        [SerializeField] private DifficultyAsset _difficulty;
        [SerializeField] private CampaignAsset _campaign;
        [SerializeField] private SoundboardAsset _soundboard;

        private Lobby _lobby;
        private Melody _melody;

        private void Awake()
        {
            var survivor = NetworkManager.Singleton;
            if (survivor != null && survivor != _match.Manager)
            {
                Destroy(_match.gameObject);
                _match = survivor.GetComponent<Match>();
                _transport = survivor.GetComponent<UnityTransport>();
            }
            _match.Prepare();
            var root = _document.rootVisualElement;
            var pages = new Pages(root.Q("card"));
            var chronicle = new Chronicle();
            var sortie = new Sortie(_campaign, _difficulty, _match, chronicle);
            new Missions(root, _campaign, _difficulty, sortie, chronicle);
            _lobby = new Lobby(root, _match, new Lan(_transport, _port), sortie);
            var codex = new Codex(root, new Recipes(_chemistry.Chemistry(), _lore), _lore);
            root.Q<Label>("campaign-title").text = _campaign.Title;
            root.Q<Label>("prologue-text").text = _campaign.Prologue;
            root.Q<Button>("campaign").clicked += () => pages.Open(chronicle.Prologued ? Board : Prologue);
            root.Q<Button>("prologue-continue").clicked += () =>
            {
                chronicle.Hear();
                pages.Open(Board);
            };
            root.Q<Button>("story").clicked += () => pages.Open(Prologue);
            root.Q<Button>("solo").clicked += () => SceneManager.LoadScene(sortie.Level.Scene);
            var mission = root.Q<Label>("coop-mission");
            root.Q<Button>("coop").clicked += () => Gather(pages, mission, sortie);
            root.Q<Button>("to-coop").clicked += () => Gather(pages, mission, sortie);
            root.Q<Button>("codex").clicked += codex.Open;
            root.Q<Button>("quit").clicked += Application.Quit;
            root.Q<Button>("settings").clicked += () => pages.Open(Settings);
            new Setting(root.Q<Button>("numbers"), "Damage numbers", "numbers");
            var voices = new Voices(transform, _soundboard.Voices, new Setting(root.Q<Button>("sound"), "Sound", "sound"));
            _melody = new Melody(transform, _soundboard.Menu, _soundboard.Music, new Setting(root.Q<Button>("music"), "Music", "music"));
            new Chime(root, voices, _soundboard.Click);
            root.Query<Button>(className: "back").ForEach(back => back.clicked += () => pages.Open(Front));
            pages.Open(Front);
        }

        private void Update()
        {
            _lobby.Refresh();
            _melody.Refresh();
        }

        private void OnDestroy() => _lobby.Dispose();

        private static void Gather(Pages pages, Label mission, Sortie sortie)
        {
            mission.text = $"Hosting: {sortie.Summary}";
            pages.Open(Coop);
        }
    }
}
