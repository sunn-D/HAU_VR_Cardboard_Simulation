using System.Collections;
using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Player_Controller;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.NPC_Controller
{
    public class NPCVoice : NPCBase, IPointerAction
    {
        //
        [field: FoldoutGroup("Voice"), SerializeField] public string NPCVoiceName { get; set; }
        [field: FoldoutGroup("Anim"), SerializeField] public int IdleEnterAnimIndex { get; set; }
        [field: FoldoutGroup("Anim"), SerializeField] public int IdleClickAnimIndex { get; set; }
        [field: FoldoutGroup("Anim"), SerializeField] public int IdleExitAnimIndex { get; set; }
        
        //
        private readonly int IdleHash = Animator.StringToHash("Animation_int");
        private float _clipVoiceLength;
        
        //
        private void Start()
        {
            _clipVoiceLength = AudioManager.Instance.DurationSound(NPCVoiceName);
            NPCAnimator.SetInteger(IdleHash, IdleExitAnimIndex);
        }

        //
        public void OnPointerEnter()
        {
            NPCAnimator.SetInteger(IdleHash, IdleEnterAnimIndex);
        }
        
        //
        public void OnPointerClick()
        {
            NPCAnimator.SetInteger(IdleHash, IdleClickAnimIndex);
            AudioManager.Instance.PlayActorSound(NPCVoiceName);
            State = NPCState.Voice;
            StartCoroutine(nameof(DisableCollider), _clipVoiceLength);
        }
        
        //
        public void OnPointerExit()
        {
            if (State != NPCState.Voice)
            {
                NPCAnimator.SetInteger(IdleHash, IdleExitAnimIndex);
            }
        }
        
        //
        private IEnumerator DisableCollider(float duration)
        {
            NPCCollider.enabled = false;
            yield return new WaitForSeconds(duration);
            NPCCollider.enabled = true;
            NPCAnimator.SetInteger(IdleHash, IdleExitAnimIndex);
            State = NPCState.Idle;
        }
    }
}