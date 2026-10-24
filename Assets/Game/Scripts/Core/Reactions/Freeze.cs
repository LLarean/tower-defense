using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class Freeze : IReaction
    {
        private readonly float _seconds;

        public Freeze(float seconds)
        {
            _seconds = seconds;
        }

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            target.Inflict(Condition.Frozen, 1f, _seconds);
            arena.Feedback.Reacted(ReactionKind.Freeze, target.Position, 0f);
        }
    }
}
