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
        public enum NPCStyle
        {
            Graduate, Girl, Cook 
        }

        //
        [field: FoldoutGroup("Style"), SerializeField] public NPCStyle Style { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public Animator NPCAnimator { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public BoxCollider NPCCollider { get; set; }
        
        
        //
        public NPCState State { get; set; }

        #endregion

        //
        private void Reset()
        {
            NPCCollider = GetComponent<BoxCollider>();
        }
        
        //
        private void OnValidate()
        {
            var renders = transform.GetChild(0).transform;
            switch (Style)
            {
                case NPCStyle.Graduate:
                    renders.GetChild(0).gameObject.SetActive(true);
                    renders.GetChild(1).gameObject.SetActive(false);
                    renders.GetChild(2).gameObject.SetActive(false);
                    NPCAnimator = renders.GetChild(0).GetComponent<Animator>();
                    break;
                case NPCStyle.Girl:
                    renders.GetChild(0).gameObject.SetActive(false);
                    renders.GetChild(1).gameObject.SetActive(true);
                    renders.GetChild(2).gameObject.SetActive(false);
                    NPCAnimator = renders.GetChild(1).GetComponent<Animator>();
                    break;
                case NPCStyle.Cook:
                    renders.GetChild(0).gameObject.SetActive(false);
                    renders.GetChild(1).gameObject.SetActive(false);
                    renders.GetChild(2).gameObject.SetActive(true);
                    NPCAnimator = renders.GetChild(2).GetComponent<Animator>();
                    break;
            }
        }
    }
}