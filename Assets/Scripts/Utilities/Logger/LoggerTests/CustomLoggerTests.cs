using NUnit.Framework;

namespace Utilities.Logger.LoggerTests
{
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
            CustomLogger.LogError("Test message", 0); 
        }
    }
}
