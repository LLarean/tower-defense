using ModalWindows;
using NUnit.Framework;

namespace Tests
{
    public class NotificationWindowModelTests
    {
        [Test]
        public void Ctor_SetTitle_TitleIsCorrect()
        {
            var title = "TestTitle";
            
            NotificationWindowModel confirmationWindowModel = new NotificationWindowModel()
            {
                Title = title
            };

            Assert.IsTrue(confirmationWindowModel.Title == title);
        }
        
        [Test]
        public void Ctor_SetMessage_MessageIsCorrect()
        {
            var message = "TestMessage";
            
            NotificationWindowModel confirmationWindowModel = new NotificationWindowModel()
            {
                Message = message
            };

            Assert.IsTrue(confirmationWindowModel.Message == message);
        }
        
        [Test]
        public void Ctor_SetConfirmLabel_LabelIsCorrect()
        {
            var confirmLabel = "AcceptLabel";
            
            NotificationWindowModel confirmationWindowModel = new NotificationWindowModel()
            {
                ConfirmLabel = confirmLabel
            };

            Assert.IsTrue(confirmationWindowModel.ConfirmLabel == confirmLabel);
        }

        [Test]
        public void Ctor_SetConfirmDelegate_DelegateIsCorrect()
        {
            var wasCalled = false;
            ConfirmDelegate confirmDelegate = () => { wasCalled = true; };
            
            NotificationWindowModel confirmationWindowModel = new NotificationWindowModel()
            {
                ConfirmDelegate = confirmDelegate
            };
            
            confirmationWindowModel.Confirm();

            Assert.IsTrue(wasCalled == true);
        }

        [Test]
        public void Ctor_SetNullAcceptDelegate_CallDoesntResultError()
        {
            NotificationWindowModel confirmationWindowModel = new NotificationWindowModel();
            Assert.DoesNotThrow(() => confirmationWindowModel.Confirm());
        }
    }
}
