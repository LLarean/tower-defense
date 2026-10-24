namespace Game.Core.Lockstep
{
    public enum PacketKind : byte
    {
        Turn,
        Command,
        Sum,
        Desync,
    }
}
