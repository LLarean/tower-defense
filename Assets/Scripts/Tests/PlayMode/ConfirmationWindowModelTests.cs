using ModalWindows;
using NUnit.Framework;

namespace Tests
{
    public class ConfirmationWindowModelTests
    {
        [Test]
        public void Ctor_SetTitle_TitleIsCorrect()
        {
            var title = "TestTitle";
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel()
            {
                Title = title
            };

            Assert.IsTrue(confirmationWindowModel.Title == title);
        }
        
        [Test]
        public void Ctor_SetMessage_MessageIsCorrect()
        {
            var message = "TestMessage";
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel()
            {
                Message = message
            };

            Assert.IsTrue(confirmationWindowModel.Message == message);
        }
        
        [Test]
        public void Ctor_SetAcceptLabel_LabelIsCorrect()
        {
            var acceptLabel = "AcceptLabel";
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel()
            {
                AcceptLabel = acceptLabel
            };

            Assert.IsTrue(confirmationWindowModel.AcceptLabel == acceptLabel);
        }
        
        [Test]
        public void Ctor_SetCancelLabel_LabelIsCorrect()
        {
            var cancelLabel = "CancelLabel";
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel()
            {
                CancelLabel = cancelLabel
            };

            Assert.IsTrue(confirmationWindowModel.CancelLabel == cancelLabel);
        }

        [Test]
        public void Ctor_SetAcceptDelegate_DelegateIsCorrect()
        {
            var wasCalled = false;
            AcceptDelegate acceptDelegate = () => { wasCalled = true; };
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel()
            {
                AcceptDelegate = acceptDelegate
            };
            
            confirmationWindowModel.Accept();

            Assert.IsTrue(wasCalled == true);
        }
        
        [Test]
        public void Ctor_SetCancelDelegate_DelegateIsCorrect()
        {
            var wasCalled = false;
            CancelDelegate cancelDelegate = () => { wasCalled = true; };
            
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel()
            {
                CancelDelegate = cancelDelegate
            };
            
            confirmationWindowModel.Cancel();

            Assert.IsTrue(wasCalled == true);
        }
        
        [Test]
        public void Ctor_SetNullAcceptDelegate_CallDoesntResultError()
        {
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel();
            Assert.DoesNotThrow(() => confirmationWindowModel.Accept());
        }
        
        [Test]
        public void Ctor_SetNullCancelDelegate_CallDoesntResultError()
        {
            ConfirmationWindowModel confirmationWindowModel = new ConfirmationWindowModel();
            Assert.DoesNotThrow(() => confirmationWindowModel.Cancel());
        }
    }
}
