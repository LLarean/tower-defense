using System;
using Game.Core.Lockstep;
using Unity.Collections;
using Unity.Netcode;

namespace Game.Stage.Net
{
    public sealed class NgoLink : IUplink, IDownlink, IDisposable
    {
        private const string Channel = "td.lockstep";
        private const int Header = sizeof(int);

        private readonly NetworkManager _manager;
        private Action<byte[]> _receive = data => { };

        public NgoLink(NetworkManager manager)
        {
            _manager = manager;
        }

        public void Listen(Action<byte[]> receive)
        {
            _receive = receive;
            _manager.CustomMessagingManager.RegisterNamedMessageHandler(Channel, Handle);
        }

        public void Send(byte[] data) => Post(NetworkManager.ServerClientId, data);

        public void Broadcast(byte[] data)
        {
            foreach (var client in _manager.ConnectedClientsIds)
            {
                if (client != NetworkManager.ServerClientId)
                {
                    Post(client, data);
                }
            }
        }

        public void Dispose()
        {
            if (_manager.CustomMessagingManager != null)
            {
                _manager.CustomMessagingManager.UnregisterNamedMessageHandler(Channel);
            }
        }

        private void Post(ulong client, byte[] data)
        {
            using var writer = new FastBufferWriter(data.Length + Header, Allocator.Temp);
            writer.WriteValueSafe(data.Length);
            writer.WriteBytesSafe(data);
            _manager.CustomMessagingManager.SendNamedMessage(Channel, client, writer, NetworkDelivery.ReliableFragmentedSequenced);
        }

        private void Handle(ulong sender, FastBufferReader reader)
        {
            reader.ReadValueSafe(out int length);
            var data = new byte[length];
            reader.ReadBytesSafe(ref data, length);
            _receive(data);
        }
    }
}
