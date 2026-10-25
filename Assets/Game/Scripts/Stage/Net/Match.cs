using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Stage.Net
{
    public sealed class Match : MonoBehaviour
    {
        private const string Channel = "td.roster";

        [SerializeField] private NetworkManager _manager;

        private readonly List<ulong> _clients = new List<ulong>();
        private bool _started;

        public int Players { get; private set; } = 1;

        public int Me { get; private set; }

        public int Difficulty { get; private set; }

        public bool Online => _manager.IsListening;

        public bool Hosting => _manager.IsServer;

        public NetworkManager Manager => _manager;

        public void Prepare()
        {
            _manager.NetworkConfig.ConnectionApproval = true;
            _manager.NetworkConfig.EnableSceneManagement = true;
            _manager.ConnectionApprovalCallback = (request, response) =>
            {
                response.Approved = !_started;
                response.CreatePlayerObject = false;
            };
            _manager.OnClientStarted -= Listen;
            _manager.OnClientStarted += Listen;
        }

        public void Open()
        {
            _started = false;
            Listen();
            if (_manager.IsServer)
            {
                _manager.OnClientConnectedCallback += Announce;
                _manager.OnClientDisconnectCallback += Announce;
                Announce(NetworkManager.ServerClientId);
            }
        }

        public void Choose(int difficulty)
        {
            Difficulty = difficulty;
            if (_manager.IsServer)
            {
                Announce(NetworkManager.ServerClientId);
            }
        }

        public void Begin(string scene)
        {
            _started = true;
            Announce(NetworkManager.ServerClientId);
            _manager.SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        public void Close()
        {
            _manager.OnClientConnectedCallback -= Announce;
            _manager.OnClientDisconnectCallback -= Announce;
            if (_manager.CustomMessagingManager != null)
            {
                _manager.CustomMessagingManager.UnregisterNamedMessageHandler(Channel);
            }
            _manager.Shutdown();
            Players = 1;
            Me = 0;
        }

        private void Listen()
        {
            _manager.CustomMessagingManager.UnregisterNamedMessageHandler(Channel);
            _manager.CustomMessagingManager.RegisterNamedMessageHandler(Channel, Receive);
        }

        private void Announce(ulong changed)
        {
            _clients.Clear();
            _clients.AddRange(_manager.ConnectedClientsIds);
            _clients.Sort();
            Players = _clients.Count;
            Me = 0;
            for (var index = 0; index < _clients.Count; index++)
            {
                if (_clients[index] == NetworkManager.ServerClientId)
                {
                    continue;
                }
                using var writer = new FastBufferWriter(sizeof(int) * 3, Allocator.Temp);
                writer.WriteValueSafe(Players);
                writer.WriteValueSafe(index);
                writer.WriteValueSafe(Difficulty);
                _manager.CustomMessagingManager.SendNamedMessage(Channel, _clients[index], writer, NetworkDelivery.ReliableSequenced);
            }
        }

        private void Receive(ulong sender, FastBufferReader reader)
        {
            reader.ReadValueSafe(out int players);
            reader.ReadValueSafe(out int me);
            reader.ReadValueSafe(out int difficulty);
            Players = players;
            Me = me;
            Difficulty = difficulty;
        }
    }
}
