using Sun_Package;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Controller.Elements
{
    public abstract class UIBaseButton : MonoBehaviour, IPointerAction
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Tween")]
        public SunTweenUI tweenButton;
        //
        [FoldoutGroup("Variables/Renderer")] 
        public Image imageButton;
        [FoldoutGroup("Variables/Renderer")] 
        public Color baseColor;
        [FoldoutGroup("Variables/Renderer")] 
        public Color touchColor;
        //
        [FoldoutGroup("Variables/Collider")] 
        public BoxCollider colliderButton;
        //
        [FoldoutGroup("Variables/Touch")]
        public float lastTimeClick;
        [FoldoutGroup("Variables/Touch")] 
        public float delayTimeClick = .5f;
        
        //
        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > lastTimeClick + delayTimeClick;
                if (result) lastTimeClick = Time.unscaledTime;
                return result;
            }
        }

        #endregion

        #region Functions

        private void Reset()
        {
            //
            tweenButton = GetComponent<SunTweenUI>();
            imageButton = GetComponent<Image>();
            colliderButton = GetComponent<BoxCollider>();
            
            //
            baseColor = new Color(1f, 1f, 1f, 1f);
            touchColor = new Color(0.97f, 0.56f, 0.34f, 1f);
        }

        private void Update()
        {
            if ((Time.unscaledTime > lastTimeClick + delayTimeClick) && !colliderButton.enabled)
            {
                colliderButton.enabled = true;
            }
            else if (!(Time.unscaledTime > lastTimeClick + delayTimeClick) && colliderButton.enabled)
            {
                colliderButton.enabled = false;
            }
        }

        #endregion

        #region Interfaces

        //
        public virtual void OnPointerEnter()
        {
            imageButton.color = touchColor;
        }

        //
        public virtual void OnPointerClick()
        {
            if (!CanClick) return;
            tweenButton.PlayForward();
        }
        
        //
        public virtual void OnPointerExit()
        {
            imageButton.color = baseColor;
        }

        #endregion
    }
}