using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Game.Stage.Menu
{
    public sealed class Lan
    {
        private const string Everyone = "0.0.0.0";
        private const string Loopback = "127.0.0.1";

        private readonly UnityTransport _transport;
        private readonly ushort _port;

        public Lan(UnityTransport transport, ushort port)
        {
            _transport = transport;
            _port = port;
        }

        public ushort Port => _port;

        public bool Host(NetworkManager manager)
        {
            _transport.SetConnectionData(Loopback, _port, Everyone);
            return manager.StartHost();
        }

        public bool Join(NetworkManager manager, string address)
        {
            _transport.SetConnectionData(address, _port);
            return manager.StartClient();
        }
    }
}
