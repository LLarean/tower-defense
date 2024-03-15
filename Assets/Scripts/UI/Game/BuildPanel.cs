using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class BuildPanel : MonoBehaviour
    {
        [SerializeField] private Button _fireTower;
        [SerializeField] private Button _iceTower;

        public event Action OnFireTowerClicked;
        public event Action OnIceTowerClicked;

        private void Start()
        {
            _fireTower.onClick.AddListener(OnFireTowerClick);
            _iceTower.onClick.AddListener(OnIceTowerClick);
        }

        private void OnFireTowerClick() => OnFireTowerClicked?.Invoke();

        private void OnIceTowerClick() => OnIceTowerClicked?.Invoke();
    }
}