using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;

namespace Game.Stage.Menu
{
    public sealed class Cloud
    {
        private readonly string _profile;
        private readonly UnityTransport _transport;
        private ISession _session;

        public Cloud(string profile, UnityTransport transport)
        {
            _profile = profile;
            _transport = transport;
        }

        public async Task<string> Host(int players)
        {
            await Ready();
            var options = new SessionOptions { MaxPlayers = players, IsPrivate = true }.WithRelayNetwork().WithNetworkOptions(Sockets());
            var session = await MultiplayerService.Instance.CreateSessionAsync(options);
            _session = session;
            return session.Code;
        }

        public async Task Join(string code)
        {
            await Ready();
            _session = await MultiplayerService.Instance.JoinSessionByCodeAsync(code, new JoinSessionOptions().WithNetworkOptions(Sockets()));
        }

        public async Task Leave()
        {
            if (_session == null)
            {
                return;
            }
            var session = _session;
            _session = null;
            await session.LeaveAsync();
        }

        // Browsers only speak secure WebSockets, so every peer uses them to share one Relay session.
        private NetworkOptions Sockets()
        {
            _transport.UseWebSockets = true;
            return new NetworkOptions { RelayProtocol = RelayProtocol.WSS };
        }

        private async Task Ready()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync(new InitializationOptions().SetProfile(_profile));
            }
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}
