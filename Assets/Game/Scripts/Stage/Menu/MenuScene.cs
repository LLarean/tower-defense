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
        [SerializeField] private string _map = "Meadow";
        [SerializeField] private ushort _port = 7777;

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
            _lobby = new Lobby(_document.rootVisualElement, _match, _transport, _map, _port);
        }

        private void Update() => _lobby.Refresh();

        private void OnDestroy() => _lobby.Dispose();
    }
}
