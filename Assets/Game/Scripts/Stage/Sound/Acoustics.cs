using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Core.Towers;
using Game.Stage.Config;

namespace Game.Stage.Sound
{
    public sealed class Acoustics : IFeedback, IGarrisonFeedback
    {
        private readonly SoundboardAsset _board;
        private readonly Voices _voices;

        public Acoustics(SoundboardAsset board, Voices voices)
        {
            _board = board;
            _voices = voices;
        }

        public void Fired(TowerSpec spec) => _voices.Play(_board.Shot(spec.Elements[0]));

        public void Wounded(Monster monster, float amount, Element element)
        {
        }

        public void Reacted(ReactionKind kind, System.Numerics.Vector3 position, float radius) => _voices.Play(_board.Reaction(kind));

        public void Chained(System.Numerics.Vector3 from, System.Numerics.Vector3 to)
        {
        }

        public void Zoned(ReactionKind kind, System.Numerics.Vector3 position, float radius, float seconds) => _voices.Play(_board.Reaction(kind));

        public void Died(Monster monster) => _voices.Play(_board.Death);

        public void Escaped(Monster monster) => _voices.Play(_board.Leak);

        public void Built(Tower tower) => _voices.Play(_board.Build);

        public void Upgraded(Tower tower) => _voices.Play(_board.Upgrade);

        public void Sold(Tower tower) => _voices.Play(_board.Sell);
    }
}
