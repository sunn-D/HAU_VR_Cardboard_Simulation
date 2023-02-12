using System.Collections.Generic;
using HAU_VR_Cardboard.Scripts.Player_Controller;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.NPC_Controller
{
    public abstract class NPCBase : MonoBehaviour
    {
        #region Variables

        //
        public enum NPCState
        {
            Idle, Voice, Moving
        }
        
        //
        [field: FoldoutGroup("Variables"), SerializeField] public Animator NPCAnimator { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public bool NPCVoice { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField, ShowIf(nameof(NPCVoice))] public string NPCVoiceName { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public List<int> IdleAnimationIndex { get; set; }
        // [field: FoldoutGroup("Variables"), SerializeField] 
        // [field: FoldoutGroup("Variables"), SerializeField] 
        
        //
        public int IdleHash = Animator.StringToHash("Animation_int");
        
        

        #endregion
    }
}