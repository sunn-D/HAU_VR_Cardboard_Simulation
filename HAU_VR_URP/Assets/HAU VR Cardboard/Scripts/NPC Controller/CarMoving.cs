using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.NPC_Controller
{
    public class CarMoving : MonoBehaviour
    {
        //
        [field: FoldoutGroup("Path"), SerializeField] public Transform EndPoint { get; set; }
        [field: FoldoutGroup("Path"), SerializeField] public float Duration { get; set; }
        
        
        //
        private void Start()
        {
            transform.DOMove(EndPoint.position, Duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
    }
}