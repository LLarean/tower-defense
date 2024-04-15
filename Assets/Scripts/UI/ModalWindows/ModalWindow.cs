using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ModalWindows
{
    public class ModalWindow : MonoBehaviour
    {
        [SerializeField] private Image _blackout;
        [SerializeField] private Transform _modalWindow;
        
        public void Show()
        {
            _blackout.gameObject.SetActive(true);
            _modalWindow.gameObject.SetActive(true);
            _modalWindow.DOScale(0, 0);
            _modalWindow.DOScale(1, GlobalParams.DOScaleDuration);
        }

        public void Hide()
        {
            _blackout.gameObject.SetActive(false);
            _modalWindow.gameObject.SetActive(false);
        }
    }
}