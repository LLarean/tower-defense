using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;

namespace Game.Core.Reactions
{
    public interface IReaction
    {
        void Trigger(Monster target, Hit hit, Arena arena);
    }
}
