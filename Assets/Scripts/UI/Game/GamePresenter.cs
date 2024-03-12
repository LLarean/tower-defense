namespace UI
{
    public class GamePresenter
    {
        private GameModel _gameModel;
        private GameView _gameView;

        public GamePresenter(GameModel gameModel, GameView gameView)
        {
            _gameModel = gameModel;
            _gameView = gameView;
        }
    }
}
