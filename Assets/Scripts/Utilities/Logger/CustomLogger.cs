using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace Utilities
{
    public static class CustomLogger
    {
        // TODO Is it possible to display an object in the hierarchy of the scene?
        /// <summary>
        /// Indicates whether logs should be displayed at all
        /// </summary>
        private const bool IsDevelop = true;
        /// <summary>
        /// The minimum level of importance of the logs displayed, 0 all logs, 1 only important and critical, 2 only critical
        /// </summary>
        private const int MinimumDisplayLevel = 0;

        /// <summary>
        /// Output a regular message to the console
        /// </summary>
        /// <param name="message">Message to the console</param>
        /// <param name="importanceLevel">0 is the most unimportant, 1 is necessary to pay attention, 2 is critical</param>
        public static void LogMessage(string message, int  importanceLevel)
        {
            if (IsDevelop == true && MinimumDisplayLevel <= importanceLevel)
            {
                Debug.Log($"{GetCallerData()}'{message}'");
            }
        }

        /// <summary>
        /// Output a warning message to the console
        /// </summary>
        /// <param name="message">Message to the console</param>
        /// <param name="importanceLevel">0 is the most unimportant, 1 is necessary to pay attention, 2 is critical</param>
        public static void LogWarning(string message, int  importanceLevel)
        {
            if (IsDevelop == true && MinimumDisplayLevel <= importanceLevel)
            {
                Debug.LogWarning($"{GetCallerData()}'<color=yellow>{message}</color>'");
            }
        }

        /// <summary>
        /// Output a critical message to the console, Note that this pauses the editor when 'ErrorPause' is enabled.
        /// </summary>
        /// <param name="message">Message to the console</param>
        /// <param name="importanceLevel">0 is the most unimportant, 1 is necessary to pay attention, 2 is critical</param>
        public static void LogError(string message, int  importanceLevel)
        {
            if (IsDevelop == true && MinimumDisplayLevel <= importanceLevel)
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