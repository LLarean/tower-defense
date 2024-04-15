namespace ModalWindows
{
    public delegate void ConfirmDelegate();

    public class NotificationWindowModel
    {
        public string Title;
        public string Message;
        public string ConfirmLabel;
    
        public ConfirmDelegate ConfirmDelegate;

        public NotificationWindowModel()
        {
            Title = string.Empty;
            Message = string.Empty;
            ConfirmLabel = string.Empty;
        }
        
        public void Confirm() => ConfirmDelegate?.Invoke();

    }
}