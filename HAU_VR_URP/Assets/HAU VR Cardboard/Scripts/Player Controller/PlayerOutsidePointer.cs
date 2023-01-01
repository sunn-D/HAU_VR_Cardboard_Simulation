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
        
        //
        public int FillAmount
        {
            get => fillAmount;
            set => fillAmount = value;
        }

        //
        private SpriteRenderer _targetRenderer;
        private Material _targetMaterial;
        private static readonly int Arc2 = Shader.PropertyToID("_Arc2");

        #endregion

        #region Functions

        //
        private void Reset()
        {
            _targetRenderer = GetComponent<SpriteRenderer>();
            _targetMaterial = _targetRenderer.sharedMaterial;
        }
        
        //
        private void OnValidate()
        {
            if (_targetRenderer == null) _targetRenderer = GetComponent<SpriteRenderer>();
            if (_targetMaterial == null) _targetMaterial = _targetRenderer.sharedMaterial;
            _targetMaterial.SetFloat(Arc2, 360 - FillAmount);
        }
        
        //
        public void SetFillAmount(float percent)
        {
            FillAmount = Mathf.RoundToInt(360 * percent);
            _targetMaterial.SetFloat(Arc2, 360 - FillAmount);
        }

        #endregion
    }
}