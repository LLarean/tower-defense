using System;
using NUnit.Framework;

namespace Utilities.Logger.LoggerTests
{
    //TODO Test all options
    public class CustomLoggerTests
    {
        [Test]
        public void LogMessage_DisplayMessage_MessageIsShowed()
        {
            CustomLogger.LogMessage("Test message", 0); 
        }
        
        [Test]
        public void LogMessage_DisplayWarningTestMessage_MessageIsShowed()
        {
            CustomLogger.LogWarning("Test message", 0); 
        }
        
        [Test]
        public void LogMessage_DisplayCriticalMessage_MessageIsShowed()
        {
            // TODO finish testing for correct operation with error output to the console
            // Assert.Throws<Exception>(() => {
            //     
            //     CustomLogger.LogError("Test message", 0);
            // });
        }
    }
}