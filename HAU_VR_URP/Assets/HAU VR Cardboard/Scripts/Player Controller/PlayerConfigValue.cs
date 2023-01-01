using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "HAU_VR/Player Config", order = 0)]
    public class PlayerConfigValue : SunScriptableSingleton<PlayerConfigValue>
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