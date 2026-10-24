using System;

namespace Game.Core.Economy
{
    public sealed class Lives
    {
        public Lives(int count)
        {
            Left = count;
        }

        public int Left { get; private set; }

        public bool Lost => Left <= 0;

        public void Lose(int count) => Left = Math.Max(0, Left - count);
    }
}
