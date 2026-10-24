using System.Numerics;
using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Paths;
using Game.Core.Reactions;

namespace Game.Core.Tests
{
    public sealed class Field
    {
        private readonly Route _route = new Route(new Vector3(0, 0, 0), new Vector3(100, 0, 0));

        public Field()
            : this(new Chemistry())
        {
        }

        public Field(Chemistry chemistry)
            : this(chemistry, new Feedbacks())
        {
        }

        public Field(Chemistry chemistry, IFeedback listener)
        {
            Feedback = new FakeFeedback();
            Arena = new Arena(chemistry, new Horde(4f), new Feedbacks(Feedback, listener));
        }

        public Arena Arena { get; }

        public FakeFeedback Feedback { get; }

        public Monster Spawn(float distance = 0f, float armor = 0f, params Element[] immunities)
        {
            var monster = new Monster(new Breed("Dummy", 100f, armor, 1f, 5, 1, immunities), _route);
            monster.Push(-distance);
            Arena.Horde.Add(monster);
            return monster;
        }

        public void Tick(float seconds) => Arena.Tick(seconds);
    }
}
