using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerOutsidePointer : MonoBehaviour
    {
        #region Variables

        //
        [FoldoutGroup("Variables")]
        [SerializeField, Range(0, 360)] private int fillAmount;
        [FoldoutGroup("Variables")] 
        [SerializeField] private SpriteRenderer targetRenderer;
        
        //
        public int FillAmount
        {
            get => fillAmount;
            set => fillAmount = value;
        }
        public SpriteRenderer TargetRenderer
        {
            get => targetRenderer;
            set => targetRenderer = value;
        }

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
        public void SetFillAmount(float percent)
        {
            if (_targetMaterial == null) _targetMaterial = TargetRenderer.sharedMaterial;
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