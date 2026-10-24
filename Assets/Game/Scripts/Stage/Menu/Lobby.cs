using System;
using System.Threading.Tasks;
using Game.Stage.Net;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Stage.Menu
{
    public sealed class Lobby : IDisposable
    {
        private const int Seats = 4;

        private readonly Match _match;
        private readonly UnityTransport _transport;
        private readonly string _map;
        private readonly ushort _port;
        private readonly Cloud _cloud = new Cloud();
        private readonly VisualElement _doors;
        private readonly VisualElement _hall;
        private readonly Label _status;
        private readonly Label _roster;
        private readonly Button _begin;
        private string _address = string.Empty;
        private int _shownPlayers = -1;

        public Lobby(VisualElement root, Match match, UnityTransport transport, string map, ushort port)
        {
            _match = match;
            _transport = transport;
            _map = map;
            _port = port;
            _doors = root.Q("doors");
            _hall = root.Q("hall");
            _status = root.Q<Label>("status");
            _roster = root.Q<Label>("roster");
            _begin = root.Q<Button>("begin");
            var ip = root.Q<TextField>("ip");
            var code = root.Q<TextField>("code");
            root.Q<Button>("solo").clicked += () => SceneManager.LoadScene(_map);
            root.Q<Button>("host-online").clicked += () => Attempt(HostOnline);
            root.Q<Button>("join-online").clicked += () => Attempt(() => JoinOnline(code.value.Trim().ToUpperInvariant()));
            root.Q<Button>("host-lan").clicked += () => Attempt(HostLan);
            root.Q<Button>("join-lan").clicked += () => Attempt(() => JoinLan(ip.value.Trim()));
            root.Q<Button>("quit").clicked += Application.Quit;
            _begin.clicked += () => _match.Begin(_map);
            root.Q<Button>("leave").clicked += () => Attempt(Leave);
            _match.Manager.OnClientStopped += Stopped;
            Show(false, string.Empty);
        }

        public void Dispose() => _match.Manager.OnClientStopped -= Stopped;

        public void Refresh()
        {
            if (!_match.Online || _match.Players == _shownPlayers)
            {
                return;
            }
            _shownPlayers = _match.Players;
            _roster.text = $"{_address}\nPlayers: {_shownPlayers}/{Seats}";
        }

        private async Task HostOnline()
        {
            _address = $"Code: {await _cloud.Host(Seats)}";
            Open();
        }

        private async Task JoinOnline(string code)
        {
            await _cloud.Join(code);
            _address = $"Joined {code}";
            Open();
        }

        private Task HostLan()
        {
            _transport.SetConnectionData("127.0.0.1", _port, "0.0.0.0");
            _address = $"Port: {_port}";
            Start(_match.Manager.StartHost());
            return Task.CompletedTask;
        }

        private Task JoinLan(string address)
        {
            _transport.SetConnectionData(address, _port);
            _address = $"Joined {address}";
            Start(_match.Manager.StartClient());
            return Task.CompletedTask;
        }

        private async Task Leave()
        {
            _match.Close();
            await _cloud.Leave();
            Show(false, string.Empty);
        }

        private void Start(bool started)
        {
            if (!started)
            {
                throw new InvalidOperationException("Network could not start");
            }
            Open();
        }

        private void Open()
        {
            _match.Open();
            _shownPlayers = -1;
            Show(true, _match.Hosting ? "Waiting for players" : "Waiting for the host to start");
        }

        private void Stopped(bool host) => Show(false, "Disconnected");

        private void Show(bool inside, string status)
        {
            _doors.EnableInClassList("hidden", inside);
            _hall.EnableInClassList("hidden", !inside);
            _begin.EnableInClassList("hidden", !_match.Hosting);
            _status.text = status;
        }

        private async void Attempt(Func<Task> action)
        {
            _status.text = "Connecting…";
            try
            {
                await action();
            }
            catch (Exception error)
            {
                Debug.LogException(error);
                _status.text = error.Message;
            }
        }
    }
}
