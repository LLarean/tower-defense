namespace Game.Core.Lockstep
{
    public interface IDownlink
    {
        void Broadcast(byte[] data);
    }
}
