using Utilities;

namespace ModalWindows
{
    public delegate void ConfirmDelegate();

    public class NotificationWindowModel
    {
        public string Title;
        public string Message;
        public string ConfirmLabel;
    
        public ConfirmDelegate ConfirmDelegate;

        public void Confirm()
        {
            if (ConfirmDelegate == null)
            {
                CustomLogger.LogWarning("ConfirmDelegate == null");
                return;
            }
            
            ConfirmDelegate?.Invoke();
        }
    }
}