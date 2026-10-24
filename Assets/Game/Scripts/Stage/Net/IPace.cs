using System;
using Game.Stage.Orders;

namespace Game.Stage.Net
{
    public interface IPace : IDisposable
    {
        IPost Post { get; }

        bool Desynced { get; }

        void Advance(float seconds);
    }
}
