using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Button _menu;
    [SerializeField] private Button _buildGunTower;
    [SerializeField] private Button _buildArrowTower;
    [SerializeField] private Builder _builder;
    
    private void Start()
    {
        _menu.onClick.AddListener(OpenMenu);
        _buildGunTower.onClick.AddListener(BuildGunTower);
        _buildArrowTower.onClick.AddListener(BuildArrowTower);
    }

    private void BuildGunTower()
    {
        _builder.BuildGunTower();
    }

    private void BuildArrowTower()
    {
        _builder.BuildArrowTower();
    }

    private void OpenMenu()
    {
    }
}
