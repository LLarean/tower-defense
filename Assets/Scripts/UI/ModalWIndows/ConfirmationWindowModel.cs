using Utilities;

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

        public void Accept()
        {
            if (AcceptDelegate == null)
            {
                CustomLogger.LogWarning("AcceptDelegate == null");
                return;
            }
            
            AcceptDelegate.Invoke();
        }

        public void Cancel()
        {
            if (CancelDelegate == null)
            {
                CustomLogger.LogWarning("CancelDelegate == null");
                return;
            }
            
            CancelDelegate.Invoke();
        }
    }
}