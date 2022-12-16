using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Loading Config", menuName = "Loading/Loading Config", order = 0)]
    public class LoadingConfig : SunScriptableSingleton<LoadingConfig>
    {
        [FoldoutGroup("Variables")] 
        [SerializeField] private float delayAfterFadedIn = 2f;
        [FoldoutGroup("Variables")] 
        [SerializeField] private float processAnimationMultiplier = .25f;
        [FoldoutGroup("Variables")] 
        [SerializeField] private string mainSceneName = "Main Scene";
        [FoldoutGroup("Variables")] 
        [SerializeField] private string loadingSceneName = "Loading Scene";

        //
        public float DelayAfterFadedIn => delayAfterFadedIn;
        public float ProcessAnimationMultiplier => processAnimationMultiplier;
        public string MainSceneName => mainSceneName;

        public string LoadingSceneName => loadingSceneName;
    }
}