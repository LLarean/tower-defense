using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] private InfoPanel _infoPanel;
    
    [SerializeField] private Button _menu;
    [SerializeField] private Button _buildGunTower;
    [SerializeField] private Button _buildArrowTower;
    
    private GameMediator _gameMediator;

    [Inject]
    public void Construction(GameMediator gameMediator)
    {
        _gameMediator = gameMediator;
    }

    public void ShowInfo(BuildModel buildModel) => _infoPanel.ShowInfo(buildModel);
    
    private void Start()
    {
        _menu.onClick.AddListener(OpenMenu);
        _buildGunTower.onClick.AddListener(BuildFireTower);
        _buildArrowTower.onClick.AddListener(BuildArrowTower);
    }

    private void BuildFireTower()
    {
        BuildModel buildModel = new BuildModel
        {
            Name = "FireTower",
            Damage = 100,
        };

        ShowInfo(buildModel);
        _gameMediator.BuildFireTower();
    }

    private void BuildArrowTower()
    {
        BuildModel buildModel = new BuildModel
        {
            Name = "Water Tower",
            Damage = 50,
        };

        ShowInfo(buildModel);
        _gameMediator.BuildWaterTower();
    }

    private void OpenMenu()
    {
    }
}
