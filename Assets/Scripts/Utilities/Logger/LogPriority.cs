namespace Utilities
{
    public enum LogPriority
    {
        /// <summary>
        /// It will always be displayed if logs are enabled
        /// </summary>
        High,
        /// <summary>
        /// It will be displayed if logs are enabled and the display priority is not lower than average
        /// </summary>
        Medium,
        /// <summary>
        /// It will be displayed if logs are enabled and the display priority is minimal
        /// </summary>
        Low,
    }
}