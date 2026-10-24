using System.Numerics;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;

namespace Game.Core.Arenas
{
    public sealed class Feedbacks : IFeedback
    {
        private readonly IFeedback[] _listeners;

        public Feedbacks(params IFeedback[] listeners)
        {
            _listeners = listeners;
        }

        public void Wounded(Monster monster, float amount, Element element)
        {
            foreach (var listener in _listeners)
            {
                listener.Wounded(monster, amount, element);
            }
        }

        public void Reacted(ReactionKind kind, Vector3 position, float radius)
        {
            foreach (var listener in _listeners)
            {
                listener.Reacted(kind, position, radius);
            }
        }

        public void Chained(Vector3 from, Vector3 to)
        {
            foreach (var listener in _listeners)
            {
                listener.Chained(from, to);
            }
        }

        public void Zoned(ReactionKind kind, Vector3 position, float radius, float seconds)
        {
            foreach (var listener in _listeners)
            {
                listener.Zoned(kind, position, radius, seconds);
            }
        }

        public void Died(Monster monster)
        {
            foreach (var listener in _listeners)
            {
                listener.Died(monster);
            }
        }

        public void Escaped(Monster monster)
        {
            foreach (var listener in _listeners)
            {
                listener.Escaped(monster);
            }
        }
    }
}
