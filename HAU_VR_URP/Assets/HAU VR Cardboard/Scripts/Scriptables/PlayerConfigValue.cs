using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "HAU_VR/Player Config", order = 0)]
    public class PlayerConfigValue : SunScriptableSingleton<PlayerConfigValue>
    {
        [field: FoldoutGroup("Variables")]
        //
        [field: FoldoutGroup("Variables/Configs")]
        [field: SerializeField] public bool UsingTextLogging { get; set; } 
        [field: FoldoutGroup("Variables/Configs")] 
        [field: SerializeField] public bool UsingDebugWindow { get; set; } 
        //
        [field: FoldoutGroup("Variables/Camera Pointer")]
        [field: SerializeField] public float MaxDistanceRay { get; set; }
        [field: FoldoutGroup("Variables/Camera Pointer")]
        [field: SerializeField] public float MaxGazerTime { get; set; }
        [field: FoldoutGroup("Variables/Camera Pointer")]
        [field: SerializeField] public LayerMask LayerMaskRay { get; set; } 
        [field: FoldoutGroup("Variables/Camera Pointer")] 
        [field: SerializeField] public float DelayGazeTime { get; set; }
    }
}