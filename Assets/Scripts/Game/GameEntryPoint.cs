using Game;
using Infrastructure;
using UnityEngine;
using Utilities;
using Zenject;

public class GameEntryPoint : MonoBehaviour
{
    [Inject] private GameMediator _gameMediator;

    private void Start()
    {
        CustomLogger.Log("The game scene is loaded", LogPriority.Low);
        
        EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleLoadGameScene());
        
        _gameMediator.StartMatch();
    }
}