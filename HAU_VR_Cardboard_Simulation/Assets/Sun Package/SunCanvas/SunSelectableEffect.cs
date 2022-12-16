using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Sun_Package
{
    public class SunSelectableEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Sub-Classes
        
        [Serializable]
        public class UIButtonEvent : UnityEvent<PointerEventData.InputButton> { }
        
        #endregion
        
        #region Events
        
        [FoldoutGroup("Variables")] 
        //
        [FoldoutGroup("Variables/Condition show")]
        public bool usingPressEvent;
        [FoldoutGroup("Variables/Condition show")]
        public bool usingReleaseEvent;
        [FoldoutGroup("Variables/Condition show")]
        public bool usingHeldEvent;
        //
        [FoldoutGroup("Variables/Event"), ShowIf("usingPressEvent")]
        public UIButtonEvent OnButtonPress;
        [FoldoutGroup("Variables/Event"), ShowIf("usingReleaseEvent")]
        public UIButtonEvent OnButtonRelease;
        [FoldoutGroup("Variables/Event"), ShowIf("usingHeldEvent")]
        public UIButtonEvent OnButtonHeld;
        
        //
        private bool _pressed;
        private PointerEventData _heldEventData;
        
        #endregion

        #region Variables

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (usingPressEvent) OnButtonPress?.Invoke(eventData.button);
            _pressed = true;
            _heldEventData = eventData;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (usingReleaseEvent) OnButtonRelease?.Invoke(eventData.button);
            _pressed = false;
            _heldEventData = null;
        }

        private void Update()
        {
            if (!_pressed) return;

            if (usingHeldEvent) OnButtonHeld?.Invoke(_heldEventData.button);
        }

        private void OnDisable()
        {
            _pressed = false;
        }

        #endregion
    }
}