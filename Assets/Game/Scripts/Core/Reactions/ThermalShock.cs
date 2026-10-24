using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public sealed class ThermalShock : IReaction
    {
        private readonly float _multiplier;

        public ThermalShock(float multiplier)
        {
            _multiplier = multiplier;
        }

        public ReactionKind Kind => ReactionKind.ThermalShock;

        public void Trigger(Monster target, Hit hit, Arena arena)
        {
            target.Wound(hit.Damage * _multiplier, Element.Fire, arena);
            arena.Feedback.Reacted(Kind, target.Position, 0f);
        }
    }
}
