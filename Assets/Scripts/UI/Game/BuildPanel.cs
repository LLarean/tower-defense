using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class BuildPanel : MonoBehaviour
    {
        [SerializeField] private Button _fireTower;
        [SerializeField] private Button _airTower;
        [SerializeField] private Button _waterTower;
        [SerializeField] private Button _iceTower;

        public event Action OnFireTowerClicked;
        public event Action OnAirTowerClicked;
        public event Action OnWaterTowerClicked;
        public event Action OnIceTowerClicked;

        private void Start()
        {
            _fireTower.onClick.AddListener(OnFireTowerClick);
            _airTower.onClick.AddListener(OnAirTowerClick);
            _waterTower.onClick.AddListener(OnWaterTowerClick);
            _iceTower.onClick.AddListener(OnIceTowerClick);
        }

        private void OnFireTowerClick() => OnFireTowerClicked?.Invoke();
        
        private void OnAirTowerClick() => OnAirTowerClicked?.Invoke();
        
        private void OnWaterTowerClick() => OnWaterTowerClicked?.Invoke();

        private void OnIceTowerClick() => OnIceTowerClicked?.Invoke();
    }
}