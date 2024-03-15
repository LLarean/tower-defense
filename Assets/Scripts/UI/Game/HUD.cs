using UI.Game;
using UnityEngine;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] private TopPanel _topPanel;
    [SerializeField] private BuildPanel _buildPanel;
    [SerializeField] private InfoPanel _infoPanel;

    private GameMediator _gameMediator;
    private PlayerModel _playerModel;
    
    private TowerParams _towerParams;
    private BuildModel _currentBuildModel;


    [Inject]
    public void Construction(GameMediator gameMediator, PlayerModel playerModel)
    {
        _gameMediator = gameMediator;
        _playerModel = playerModel;
    }

    public void ShowInfo(BuildModel buildModel) => _infoPanel.ShowInfo(buildModel);
    
    public void ClearInfo() => _infoPanel.ClearInfo();
    
    public void StartClock() => _topPanel.StartClock();

    public void PauseClock() => _topPanel.PauseClock();
    
    public void ResetCounting() => _topPanel.ResetClock();

    public void AddGold()
    {
        _playerModel.Gold += 50;
        _topPanel.ShowGold(_playerModel.Gold);
    }

    public void RemoveGold()
    {
        _playerModel.Gold -= _currentBuildModel.Price;
        _topPanel.ShowGold(_playerModel.Gold);
    }

    private void Start()
    {
        StartClock();

        _topPanel.OnMenuClicked += OpenMenu;

        _buildPanel.OnFireTowerClicked += BuildFireTower;
        _buildPanel.OnIceTowerClicked += BuildIceTower;
        
        _topPanel.ShowGold(_playerModel.Gold);
    }

    private void BuildFireTower()
    {
        BuildModel buildModel = new BuildModel
        {
            Title = GlobalStrings.FireTower,
            CastType = CastType.Fire,
            Damage = GlobalParams.FireTowerDamage,
            Price = GlobalParams.FireTowerPrice,
        };

        _currentBuildModel = buildModel;

        ShowInfo(buildModel);
        _gameMediator.BuildFireTower();
    }

    private void BuildIceTower()
    {
        BuildModel buildModel = new BuildModel
        {
            Title = GlobalStrings.IceTower,
            CastType = CastType.Ice,
            Damage = GlobalParams.IceTowerDamage,
            Price = GlobalParams.IceTowerPrice,
        };
        
        _currentBuildModel = buildModel;

        ShowInfo(buildModel);
        _gameMediator.BuildIceTower();
    }

    private void OpenMenu()
    {
    }

}