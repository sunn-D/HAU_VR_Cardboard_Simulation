using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Loading Config", menuName = "HAU_VR/Loading Config", order = 0)]
    public class LoadingConfigValue : SunScriptableSingleton<LoadingConfigValue>
    {
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public float DelayAfterFadedIn { get; set; } = 2f;
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public float ProcessAnimationMultiplier { get; set; } = .25f;
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public string MainSceneName { get; set; } = "Main Scene";
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public string LoadingSceneName { get; set; } = "Loading Scene";
    }
}