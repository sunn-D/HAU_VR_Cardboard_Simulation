using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerOutsidePointer : MonoBehaviour
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")]
        [field: SerializeField, Range(0, 360)] public int FillAmount { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public SpriteRenderer TargetRenderer { get; set; }

        //
        private Material _targetMaterial;
        private static readonly int Arc2 = Shader.PropertyToID("_Arc2");

        #endregion

        #region Functions
        
        //
        private void OnValidate()
        {
            if (TargetRenderer == null) TargetRenderer = transform.Find("Outside Pointer").GetComponent<SpriteRenderer>();
            if (_targetMaterial == null) _targetMaterial = TargetRenderer.sharedMaterial;
            _targetMaterial.SetFloat(Arc2, 360 - FillAmount);
        }
        
        //
        private void Start()
        {
            if (TargetRenderer == null) TargetRenderer = transform.Find("Outside Pointer").GetComponent<SpriteRenderer>();
            if (_targetMaterial == null) _targetMaterial = TargetRenderer.sharedMaterial;
        }

        //
        public void SetFillAmount(float percent)
        {
            FillAmount = Mathf.RoundToInt(360 * percent);
            _targetMaterial.SetFloat(Arc2, 360 - FillAmount);
        }
        
        //
        public void SetActiveRenderer(bool value)
        {
            TargetRenderer.gameObject.SetActive(value);
        }

        #endregion
    }
}