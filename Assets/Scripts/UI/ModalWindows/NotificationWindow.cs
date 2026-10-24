using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ModalWindows
{
    public class NotificationWindow : ModalWindow
    {
        [Space]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Button _confirm;
        [SerializeField] private TMP_Text _confirmLabel;
        
        private NotificationWindowModel _notificationWindowModel;

        [Inject]
        public void Construction(NotificationWindowModel notificationWindowModel)
        {
            _notificationWindowModel = notificationWindowModel;
        }
        
        public override void Show()
        {
            UpdateWindow();
            base.Show();
        }

        private void UpdateWindow()
        {
            _title.text = _notificationWindowModel.Title;
            _message.text = _notificationWindowModel.Message;
            
            _confirm.onClick.RemoveAllListeners();
            _confirm.onClick.AddListener(Confirm);
            _confirmLabel.text = _notificationWindowModel.ConfirmLabel;
        }

        private void Confirm()
        {
            EventBus.RaiseEvent<ISoundHandler>(soundHandler => soundHandler.HandleClick());
            _notificationWindowModel.Confirm();
        }
    }
}