using System;
using Game.Core.Elements;
using Game.Core.Towers;
using UnityEngine;

namespace Game.Stage.Config
{
    [CreateAssetMenu(menuName = "Game/Tower")]
    public sealed class TowerAsset : ScriptableObject
    {
        [SerializeField] private string _title = "Tower";
        [SerializeField] private Sprite _icon;
        [SerializeField] private Element[] _elements = { Element.Fire };
        [SerializeField] private float _missileSpeed = 25f;
        [SerializeField] private GradeData[] _grades = new GradeData[0];
        [SerializeField] private GameObject _model;
        [SerializeField] private GameObject _missile;

        public string Title => _title;

        public Sprite Icon => _icon;

        public GameObject Model => _model;

        public GameObject Missile => _missile;

        public TowerSpec Spec() => new TowerSpec(_title, _missileSpeed, _elements, Array.ConvertAll(_grades, grade => grade.Grade()));
    }
}
