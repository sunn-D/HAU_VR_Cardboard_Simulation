using DG.Tweening;
using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Player_Controller;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public abstract class BaseButtonController : MonoBehaviour, IPointerAction
    {
        #region Variables

        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public Image ImageButton { get; set; }
        
        //
        private Tweener _tweenColorButton;

        //
        private readonly Color NormalColor = new Color(1f, 1f, 1f, 1f);
        private readonly Color PointEnterColor = new Color(.9f, .7f, .3f, 1f);

        #endregion

        #region Functions

        //
        private void Reset()
        {
            ImageButton = GetComponent<Image>();
        }

        #endregion

        #region Implement Interface

        //
        public virtual void OnPointerEnter()
        {
            _tweenColorButton?.Kill();
            _tweenColorButton = ImageButton.DOColor(PointEnterColor, .25f);
            Debug.Log("Enter");
        }

        //
        public virtual void OnPointerClick()
        {
            AudioManager.Instance.PlaySound("Splash");
        }
        
        //
        public virtual void OnPointerExit()
        {
            _tweenColorButton?.Kill();
            _tweenColorButton = ImageButton.DOColor(NormalColor, .25f);
            Debug.Log("Exit");
        }

        #endregion
    }
}