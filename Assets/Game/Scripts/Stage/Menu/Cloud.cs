using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;

namespace Game.Stage.Menu
{
    public sealed class Cloud
    {
        private readonly string _profile;
        private ISession _session;

        public Cloud(string profile)
        {
            _profile = profile;
        }

        public async Task<string> Host(int players)
        {
            await Ready();
            var options = new SessionOptions { MaxPlayers = players, IsPrivate = true }.WithRelayNetwork();
            var session = await MultiplayerService.Instance.CreateSessionAsync(options);
            _session = session;
            return session.Code;
        }

        public async Task Join(string code)
        {
            await Ready();
            _session = await MultiplayerService.Instance.JoinSessionByCodeAsync(code);
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
