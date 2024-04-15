using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void Initialize(NotificationWindowModel notificationWindowModel)
        {
            _notificationWindowModel = notificationWindowModel;
            
            _title.text = notificationWindowModel.Title;
            _message.text = notificationWindowModel.Message;
            
            _confirm.onClick.RemoveAllListeners();
            _confirm.onClick.AddListener(Confirm);
            _confirmLabel.text = notificationWindowModel.ConfirmLabel;
            
            Show();
        }
        
        private void Confirm() => _notificationWindowModel.ConfirmDelegate();
    }
}