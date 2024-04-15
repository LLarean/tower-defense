using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModalWindows
{
    public class ConfirmationWindow : MonoBehaviour
    {
        [SerializeField] private Image _blackout;
        [SerializeField] private Transform _modalWindow;
        [Space]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Button _accept;
        [SerializeField] private TMP_Text _acceptLabel;
        [SerializeField] private Button _cancel;
        [SerializeField] private TMP_Text _cancelLabel;

        private ConfirmationWindowModel _confirmationWindowModel;

        public void Initialize(ConfirmationWindowModel confirmationWindowModel)
        {
            _confirmationWindowModel = confirmationWindowModel;
            
            _title.text = confirmationWindowModel.Title;
            _message.text = confirmationWindowModel.Message;
            
            _accept.onClick.RemoveAllListeners();
            _accept.onClick.AddListener(Accept);
            _acceptLabel.text = confirmationWindowModel.AcceptLabel;
            
            _cancel.onClick.RemoveAllListeners();
            _cancel.onClick.AddListener(Cancel);
            _cancelLabel.text = confirmationWindowModel.CancelLabel;

            Show();
        }

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

        private void Accept() => _confirmationWindowModel.Accept();

        private void Cancel() => _confirmationWindowModel.Cancel();
    }
}