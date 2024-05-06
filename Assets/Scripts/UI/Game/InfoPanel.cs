using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _castType;
        [SerializeField] private TMP_Text _damage;
        [SerializeField] private TMP_Text _price;

        public void ShowInfo(BuildModel buildModel)
        {
            _portrait.sprite = buildModel.Portrait;
            _title.text = buildModel.Title;
            _castType.text = $"{GlobalStrings.DamageType}: {buildModel.ElementalType}";
            _damage.text = $"{GlobalStrings.Damage}: {buildModel.Damage}";
            _price.text = $"{GlobalStrings.Price}: {buildModel.Price}";
        }

        public void ClearInfo()
        {
            _portrait.sprite = null;
            _title.text = "";
            _castType.text = "";
            _damage.text = "";
            _price.text = "";
        }

        private void Start()
        {
            ClearInfo();
        }
    }
}