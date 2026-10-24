using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;

namespace Game.Core.Economy
{
    public sealed class Ledger : IFeedback
    {
        private readonly Treasury _treasury;
        private readonly Lives _lives;

        public Ledger(Treasury treasury, Lives lives)
        {
            _treasury = treasury;
            _lives = lives;
        }

        public void Died(Monster monster) => _treasury.Share(monster.Breed.Bounty);

        public void Escaped(Monster monster) => _lives.Lose(monster.Breed.Harm);

        public void Wounded(Monster monster, float amount, Element element)
        {
        }

        public void Reacted(ReactionKind kind, Vector3 position, float radius)
        {
        }

        public void Chained(Vector3 from, Vector3 to)
        {
        }

        public void Zoned(ReactionKind kind, Vector3 position, float radius, float seconds)
        {
        }
    }
}
