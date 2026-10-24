using Game.Stage.Config;
using Game.Stage.Hud;
using Game.Stage.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Menu
{
    public sealed class MenuScene : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Match _match;
        [SerializeField] private UnityTransport _transport;
        [SerializeField] private string[] _maps = { "Meadow" };
        [SerializeField] private ushort _port = 7777;
        [SerializeField] private ChemistryAsset _chemistry;
        [SerializeField] private LoreAsset _lore;

        private Lobby _lobby;

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
            _lobby = new Lobby(root, _match, new Lan(_transport, _port), new Atlas(_maps));
            var codex = new Codex(root, new Recipes(_chemistry.Chemistry(), _lore), _lore);
            root.Q<Button>("codex").clicked += codex.Open;
        }

        private void Update() => _lobby.Refresh();

        private void OnDestroy() => _lobby.Dispose();
    }
}
