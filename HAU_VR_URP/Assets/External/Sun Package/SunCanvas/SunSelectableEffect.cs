using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public class SunSelectableEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Sub Classes

        //
        [Serializable] public class UIButtonEvent : UnityEvent<PointerEventData.InputButton> { }

        #endregion

        #region Variables

        //
        [field: FoldoutGroup("Press"), SerializeField] public bool UsingDefaultPressEvent { get; set; }
        [field: FoldoutGroup("Press"), SerializeField] public bool UsingPressEvent { get; set; }
        [field: FoldoutGroup("Press"), SerializeField, ShowIf(nameof(UsingPressEvent))] public UIButtonEvent OnButtonPressEvent { get; set; }
        
        //        
        [field: FoldoutGroup("Release"), SerializeField] public bool UsingDefaultReleaseEvent { get; set; }
        [field: FoldoutGroup("Release"), SerializeField] public bool UsingReleaseEvent { get; set; }
        [field: FoldoutGroup("Release"), SerializeField, ShowIf(nameof(UsingReleaseEvent))] public UIButtonEvent OnButtonReleaseEvent { get; set; }
        
        //
        [field: FoldoutGroup("Held"), SerializeField] public bool UsingHeldEvent { get; set; }
        [field: FoldoutGroup("Held"), SerializeField, ShowIf(nameof(UsingHeldEvent))] public UIButtonEvent OnButtonHeldEvent { get; set; }

        //
        private Tween _mainTween;
        private bool _pressed;
        private PointerEventData _heldEventData;
        
        #endregion

        //
        public void OnPointerDown(PointerEventData eventData)
        {
            if (UsingDefaultPressEvent) TweenPress();
            if (UsingPressEvent) OnButtonPressEvent?.Invoke(eventData.button);
            _pressed = true;
            _heldEventData = eventData;
        }

        //
        public void OnPointerUp(PointerEventData eventData)
        {
            if (UsingDefaultReleaseEvent) TweenRelease();
            if (UsingReleaseEvent) OnButtonReleaseEvent?.Invoke(eventData.button);
            _pressed = true;
            _heldEventData = eventData;
        }
        
        //
        private void Update()
        {
            if (!_pressed) return;
            if (UsingHeldEvent) OnButtonHeldEvent?.Invoke(_heldEventData.button);
        }
        
        //
        private void OnDisable()
        {
            _pressed = false;
            _heldEventData = null;
        }

        #region Default Tween

        // ReSharper disable Unity.PerformanceAnalysis
        private void TweenPress()
        {
            if (_mainTween != null && _mainTween.IsActive() && !_mainTween.IsComplete())
                _mainTween.Kill();
            else
                transform.localScale = Vector3.one;

            _mainTween = transform.DOScale(Vector3.one * 1.1f, .1f).SetEase(Ease.Linear);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void TweenRelease()
        {
            if (_mainTween != null && _mainTween.IsActive() && !_mainTween.IsComplete())
                _mainTween.Kill();
            else
                transform.localScale = Vector3.one * 1.1f;

            _mainTween = transform.DOScale(Vector3.one, .1f).SetEase(Ease.Linear);
        }

        #endregion
    }
}