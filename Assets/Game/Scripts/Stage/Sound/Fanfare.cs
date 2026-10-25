using Game.Core.Battles;
using Game.Stage.Config;
using Game.Stage.Hud;

namespace Game.Stage.Sound
{
    public sealed class Fanfare : IWidget
    {
        private readonly Battle _battle;
        private readonly SoundboardAsset _board;
        private readonly Voices _voices;
        private int _wave;
        private Outcome _outcome = Outcome.Ongoing;

        public Fanfare(Battle battle, SoundboardAsset board, Voices voices)
        {
            _battle = battle;
            _board = board;
            _voices = voices;
        }

        public void Refresh()
        {
            if (_battle.Invasion.Wave > _wave)
            {
                _wave = _battle.Invasion.Wave;
                _voices.Play(_board.Wave);
            }
            if (_battle.Outcome != _outcome)
            {
                _outcome = _battle.Outcome;
                _voices.Play(_outcome == Outcome.Won ? _board.Victory : _board.Defeat);
            }
        }
    }
}
