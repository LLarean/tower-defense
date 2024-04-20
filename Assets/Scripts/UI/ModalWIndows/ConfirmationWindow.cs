using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ModalWindows
{
    public class ConfirmationWindow : ModalWindow
    {
        [Space]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Button _accept;
        [SerializeField] private TMP_Text _acceptLabel;
        [SerializeField] private Button _cancel;
        [SerializeField] private TMP_Text _cancelLabel;

        private ConfirmationWindowModel _confirmationWindowModel;

        [Inject]
        public void Construction(ConfirmationWindowModel confirmationWindowModel)
        {
            _confirmationWindowModel = confirmationWindowModel;
        }

        public override void Show()
        {
            UpdateWindow();
            base.Show();
        }

        private void UpdateWindow()
        {
            _title.text = _confirmationWindowModel.Title;
            _message.text = _confirmationWindowModel.Message;
            
            _accept.onClick.RemoveAllListeners();
            _accept.onClick.AddListener(Accept);
            _acceptLabel.text = _confirmationWindowModel.AcceptLabel;
            
            _cancel.onClick.RemoveAllListeners();
            _cancel.onClick.AddListener(Cancel);
            _cancelLabel.text = _confirmationWindowModel.CancelLabel;
        }

        private void Accept()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _confirmationWindowModel.Accept();
        }

        private void Cancel()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _confirmationWindowModel.Cancel();
        }
    }
}