using System;
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

    public event Action OnBuildGunTowerClicked;
    public event Action OnBuildArrowTowerClicked;

    [Inject]
    public void Construction(GameMediator gameMediator)
    {
        _gameMediator = gameMediator;
    }
    
    private void Start()
    {
        _menu.onClick.AddListener(OpenMenu);
        _buildGunTower.onClick.AddListener(BuildGunTower);
        _buildArrowTower.onClick.AddListener(BuildArrowTower);
    }

    private void BuildGunTower() => OnBuildGunTowerClicked?.Invoke();

    private void BuildArrowTower() => OnBuildArrowTowerClicked?.Invoke();

    private void OpenMenu()
    {
    }
}
