using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace Utilities
{
    public static class CustomLogger
    {
        // TODO Is it possible to display an object in the hierarchy of the scene?
        // TODO Is it possible to configure the logger?
        /// <summary>
        /// Indicates whether logs should be displayed at all
        /// </summary>
        private const bool IsDevelop = true;

        /// <summary>
        /// The minimum level of importance of the logs displayed, 0 only critical, 1 only important and critical, 2 all logs
        /// </summary>
        private const int LogDisplayLevel = 2;
        
        /// <summary>
        /// The minimum level of priority of the logs displayed, High - only critical, Medium - only important and critical, Low - all logs
        /// </summary>
        private const LogPriority LevelPriorityLogsDisplayed = LogPriority.Low;

        /// <summary>
        /// Output a regular message to the console
        /// </summary>
        /// <param name="message">Message to the console</param>
        /// <param name="logPriority">High - only critical, Medium - only important and critical, Low - all logs</param>
        public static void Log(string message, LogPriority logPriority)
        {
            if (IsDevelop == true && LevelPriorityLogsDisplayed >= logPriority)
            {
                Debug.Log($"{GetCallerData()}'{message}'");
            }
        }

        /// <summary>
        /// Output a warning message to the console
        /// </summary>
        /// <param name="message">Message to the console</param>
        public static void LogWarning(string message)
        {
            if (IsDevelop == true)
            {
                Debug.LogWarning($"{GetCallerData()}'<color=yellow>{message}</color>'");
            }
        }

        /// <summary>
        /// Output a critical message to the console, Note that this pauses the editor when 'ErrorPause' is enabled.
        /// </summary>
        /// <param name="message">Message to the console</param>
        public static void LogError(string message)
        {
            if (IsDevelop == true)
            {
                Debug.LogError($"{GetCallerData()}'<color=red>{message}</color>'");
            }
        }
        
        private static string GetCallerData()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame callingFrame = stackTrace.GetFrame(2);

            MethodBase callingMethod = callingFrame.GetMethod();
            string callingClassName = callingMethod.ReflectedType.Name;

            var classAndMethod = $"Class: '{callingClassName}', Method: '{callingMethod.Name}', Message: ";
            return classAndMethod;
        }
    }
}