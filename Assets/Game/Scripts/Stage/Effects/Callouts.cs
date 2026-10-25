using Game.Core.Arenas;
using Game.Core.Elements;
using Game.Core.Monsters;
using Game.Core.Reactions;
using Game.Stage.Config;
using UnityEngine;

namespace Game.Stage.Effects
{
    public sealed class Callouts : IFeedback
    {
        private const string Look = "caption--reaction";

        private static readonly Vector3 Lift = new Vector3(0f, 3f, 0f);

        private readonly SpectacleAsset _spectacle;
        private readonly LoreAsset _lore;
        private readonly Captions _titles;
        private readonly Tally _tally;
        private readonly float[] _spoken;
        private readonly Vector3[] _spots;
        private float _time;

        public Callouts(SpectacleAsset spectacle, LoreAsset lore, Captions titles, Tally tally)
        {
            _spectacle = spectacle;
            _lore = lore;
            _titles = titles;
            _tally = tally;
            var kinds = System.Enum.GetValues(typeof(ReactionKind)).Length;
            _spoken = new float[kinds];
            _spots = new Vector3[kinds];
            for (var kind = 0; kind < kinds; kind++)
            {
                _spoken[kind] = float.NegativeInfinity;
            }
        }

        public void Wounded(Monster monster, float amount, Element element) => _tally.Add(monster, amount, element);

        public void Reacted(ReactionKind kind, System.Numerics.Vector3 position, float radius) => Announce(kind, position.ToUnity());

        public void Chained(System.Numerics.Vector3 from, System.Numerics.Vector3 to)
        {
        }

        public void Zoned(ReactionKind kind, System.Numerics.Vector3 position, float radius, float seconds) => Announce(kind, position.ToUnity());

        public void Died(Monster monster)
        {
        }

        public void Escaped(Monster monster)
        {
        }

        public void Tick(float seconds)
        {
            _time += seconds;
            _titles.Tick(seconds);
            _tally.Tick(seconds);
        }

        private void Announce(ReactionKind kind, Vector3 position)
        {
            var index = (int)kind;
            var recent = _time - _spoken[index] < _spectacle.Titles.Life;
            if (recent && (position - _spots[index]).sqrMagnitude < _spectacle.Hush * _spectacle.Hush)
            {
                return;
            }
            _spoken[index] = _time;
            _spots[index] = position;
            _titles.Say(_lore.Title(kind), _spectacle.Color(kind), position + Lift, Look);
        }
    }
}
