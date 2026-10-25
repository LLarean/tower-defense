using System;
using System.Threading.Tasks;
using Game.Stage.Net;
using Game.Stage.Speech;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Stage.Menu
{
    public sealed class Lobby : IDisposable
    {
        private const int Seats = 4;

        private readonly Match _match;
        private readonly Lan _lan;
        private readonly Sortie _sortie;
        private readonly Lexicon _lexicon;
        private readonly Cloud _cloud = new Cloud("default");
        private readonly VisualElement _doors;
        private readonly VisualElement _hall;
        private readonly Label _status;
        private readonly Label _roster;
        private readonly Button _begin;
        private readonly Button _copy;
        private string _address = string.Empty;
        private string _code = string.Empty;
        private int _shownPlayers = -1;

        public Lobby(VisualElement root, Match match, Lan lan, Sortie sortie, Lexicon lexicon)
        {
            _lexicon = lexicon;
            _match = match;
            _lan = lan;
            _sortie = sortie;
            _doors = root.Q("doors");
            _hall = root.Q("hall");
            _status = root.Q<Label>("status");
            _roster = root.Q<Label>("roster");
            _begin = root.Q<Button>("begin");
            var ip = root.Q<TextField>("ip");
            var code = root.Q<TextField>("code");
            code.textEdition.placeholder = lexicon.Say("Enter code");
            code.maxLength = 8;
            _copy = root.Q<Button>("copy");
            _copy.clicked += () => GUIUtility.systemCopyBuffer = _code;
            root.Q<Button>("host-online").clicked += () => Attempt(HostOnline);
            root.Q<Button>("join-online").clicked += () => Attempt(() => JoinOnline(code.value.Trim().ToUpperInvariant()));
            root.Q<Button>("host-lan").clicked += () => Attempt(HostLan);
            root.Q<Button>("join-lan").clicked += () => Attempt(() => JoinLan(ip.value.Trim()));
            _begin.clicked += () => _match.Begin(_sortie.Level.Scene);
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
            _roster.text = _lexicon.Say("{0}\nPlayers: {1}", _address, $"{_shownPlayers}/{Seats}");
        }

        private async Task HostOnline()
        {
            _code = await _cloud.Host(Seats);
            _address = _lexicon.Say("Code: {0}", _code);
            Open();
        }

        private async Task JoinOnline(string code)
        {
            await _cloud.Join(code);
            _address = _lexicon.Say("Joined {0}", code);
            Open();
        }

        private Task HostLan()
        {
            _address = _lexicon.Say("Port: {0}", _lan.Port);
            Start(_lan.Host(_match.Manager));
            return Task.CompletedTask;
        }

        private Task JoinLan(string address)
        {
            _address = _lexicon.Say("Joined {0}", address);
            Start(_lan.Join(_match.Manager, address));
            return Task.CompletedTask;
        }

        private async Task Leave()
        {
            _code = string.Empty;
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
            Show(true, _lexicon.Say(_match.Hosting ? "Waiting for players" : "Waiting for the host to start"));
        }

        private void Stopped(bool host) => Show(false, _lexicon.Say("Disconnected"));

        private void Show(bool inside, string status)
        {
            _doors.EnableInClassList("hidden", inside);
            _hall.EnableInClassList("hidden", !inside);
            _begin.EnableInClassList("hidden", !_match.Hosting);
            _copy.EnableInClassList("hidden", !inside || _code.Length == 0 || !_match.Hosting);
            _status.text = status;
        }

        private async void Attempt(Func<Task> action)
        {
            _status.text = _lexicon.Say("Connecting…");
            try
            {
                await action();
            }
            catch (Exception error)
            {
                Debug.LogException(error);
                _status.text = _lexicon.Say("Could not connect: {0}", error.Message);
            }
        }
    }
}
