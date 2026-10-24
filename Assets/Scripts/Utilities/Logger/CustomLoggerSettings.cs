using UnityEngine;

namespace Utilities.Logger
{
    [CreateAssetMenu(fileName = "CustomLoggerSettings", menuName = "ScriptableObjects/CustomLoggerSettings", order = 1)]
    public class CustomLoggerSettings : ScriptableObject
    {
        /// <summary>
        /// Indicates whether logs should be displayed at all
        /// </summary>
        [SerializeField] private bool _isDevelop = true;
        /// <summary>
        /// The minimum level of importance of the logs displayed, 0 all logs, 1 only important and critical, 2 only critical
        /// </summary>
        [SerializeField] private int _minimumDisplayLevel = 0;

        public bool IsDevelop => _isDevelop;
        public int MinimumDisplayLevel => _minimumDisplayLevel;
    }
}