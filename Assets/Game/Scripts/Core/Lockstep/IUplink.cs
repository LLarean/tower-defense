namespace Game.Core.Lockstep
{
    public interface IUplink
    {
        void Send(byte[] data);
    }
}
