using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public interface IReaction
    {
        ReactionKind Kind { get; }

        void Trigger(Monster target, Hit hit, Arena arena);
    }
}
