using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public class SunAutoSaveConfig : ScriptableObject
    {
        [Tooltip("Enable auto save functionality")]
        public bool Enabled;
        
        [Tooltip("The frequency in minutes auto save will activate"), Min(1)]
        public int Frequency = 1;
        
        [Tooltip("Log a message every time the scene is auto saved")]
        public bool Logging;
    }
}