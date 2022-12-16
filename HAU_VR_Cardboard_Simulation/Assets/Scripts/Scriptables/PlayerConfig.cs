using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Player/Player Config", order = 0)]
    public class PlayerConfig : SunScriptableSingleton<PlayerConfig>
    {
        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Camera Pointer")]
        [SerializeField] private float maxDistanceRay = 10f;
        [FoldoutGroup("Variables/Camera Pointer")]
        [SerializeField] private float maxGazerTime = 1f;
        [FoldoutGroup("Variables/Camera Pointer")]
        [SerializeField] private LayerMask layerMaskRay;
        [FoldoutGroup("Variables/Camera Pointer")] 
        [SerializeField] private float delayGazeTime = .5f;
        
        //
        public float MaxDistanceRay => maxDistanceRay;
        public float MaxGazerTime => maxGazerTime;
        public LayerMask LayerMaskRay => layerMaskRay;
        public float DelayGazeTime => delayGazeTime;
    }
}