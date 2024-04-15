namespace ModalWindows
{
    public delegate void AcceptDelegate();
    public delegate void CancelDelegate();
    
    public class ConfirmationWindowModel
    {
        public string Title;
        public string Message;
        public string AcceptLabel;
        public string CancelLabel;
        
        public AcceptDelegate AcceptDelegate;
        public CancelDelegate CancelDelegate;

        public ConfirmationWindowModel()
        {
            Title = string.Empty;
            Message = string.Empty;
            AcceptLabel = string.Empty;
            CancelLabel = string.Empty;
        }

        public void Accept() => AcceptDelegate?.Invoke();
        
        public void Cancel() => CancelDelegate?.Invoke();
    }
}