using Game.Core.Reactions;

namespace Game.Core.Arenas
{
    public sealed class Arena
    {
        public Arena(Chemistry chemistry, Horde horde, IFeedback feedback)
        {
            Chemistry = chemistry;
            Horde = horde;
            Feedback = feedback;
            Ground = new Ground();
        }

        public Chemistry Chemistry { get; }

        public Horde Horde { get; }

        public Ground Ground { get; }

        public IFeedback Feedback { get; }

        public void Tick(float seconds)
        {
            Ground.Tick(seconds, this);
            Horde.Tick(seconds, this);
        }
    }
}
