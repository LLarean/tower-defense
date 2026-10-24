using System.Numerics;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;

namespace Game.Core.Arenas
{
    public interface IFeedback
    {
        void Wounded(Monster monster, float amount, Element element);

        void Reacted(ReactionKind kind, Vector3 position, float radius);

        void Chained(Vector3 from, Vector3 to);

        void Zoned(ReactionKind kind, Vector3 position, float radius, float seconds);

        void Died(Monster monster);

        void Escaped(Monster monster);
    }
}
