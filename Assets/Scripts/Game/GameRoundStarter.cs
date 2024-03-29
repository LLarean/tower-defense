using Infrastructure;
using Zenject;

public class GameRoundStarter : RoundStarter, IEnemyHandler
{
    private GameMediator _gameMediator;
    
    [Inject]
    public void Construct(GameMediator gameMediator)
    {
        _gameMediator = gameMediator;
    }
    
    protected virtual void FinishMatch()
    {
        var modalWindowModel = new ModalWindowModel
        {
            Message = "The game is over",
            Label = "To the menu",
        };
        
        _gameMediator.ShowModalWindow(modalWindowModel);
    }
}