using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;

namespace Game.Stage.Menu
{
    public sealed class Cloud
    {
        private ISession _session;

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

        private static async Task Ready()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}
