using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _damage;

    public void ShowInfo(BuildModel buildModel)
    {
        _name.text = buildModel.Name;
        _health.text = $"Здоровье: {buildModel.CurrentHealth}/{buildModel.MaximumHealth}";
        _damage.text = $"Урон: {buildModel.Damage}";
    }
}