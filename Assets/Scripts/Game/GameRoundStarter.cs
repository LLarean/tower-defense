using Game;
using GameUtilities;
using Infrastructure;
using ModalWindows;
using Zenject;

public class GameRoundStarter : RoundStarter, IEnemyHandler
{
    private GameMediator _gameMediator;
    
    [Inject]
    public void Construct(GameMediator gameMediator)
    {
        _gameMediator = gameMediator;
        EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleLoadGame());
    }
    
    protected virtual void FinishMatch()
    {
        base.FinishMatch();
        ConfirmDelegate confirmDelegate = () => { _gameMediator.LoadMainMenu(); };
    
        var modalWindowModel = new NotificationWindowModel
        {
            Title = "The end",
            Message = "The game is over",
            ConfirmLabel = "To Menu",
            ConfirmDelegate = confirmDelegate
        };
        
        _gameMediator.InitializeNotificationWindow(modalWindowModel);
        _gameMediator.ShowNotificationWindow();
    }
}