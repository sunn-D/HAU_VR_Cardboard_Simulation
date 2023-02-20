using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class SunTweenScale : SunTween
    {
        #region Fields

        //
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private Vector3 fromValue = Vector3.zero;
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private Vector3 toValue = Vector3.zero;
        
        //
        public Vector3 FromValue
        {
            get => fromValue;
            set => fromValue = value;
        }
        public Vector3 ToValue
        {
            get => toValue;
            set => toValue = value;
        }

        #endregion

        #region Tween

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenForward(Vector3 f, Vector3 t)
        {
            if (MainTween != null && MainTween.IsActive())
            {
                if (!MainTween.IsComplete())
                {
                    MainTween.Kill();
                }
            }
            else
            {
                MainTarget.localScale = f;
            }

            if (EnableBeforeForward) MainTarget.gameObject.SetActive(true);
            switch (StyleForward)
            {
                case Style.Once:
                    MainTween = MainTarget.DOScale(t, Duration)
                        .SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Forward ? DelayTime : 0)
                        .SetEase(EaseForward)
                        .OnStart(() =>
                        {
                            if (EventStartWhen == EventWhen.Forward || EventStartWhen == EventWhen.Both)
                                OnStartEvent?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (EventFinishWhen == EventWhen.Forward || EventFinishWhen == EventWhen.Both)
                                OnFinishEvent?.Invoke();
                            Animating = false;
                        });
                    break;
                case Style.Loop:
                    MainTween = MainTarget.DOScale(t, Duration)
                        .SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Forward ? DelayTime : 0)
                        .SetLoops(-1, LoopStyleForward)
                        .SetEase(EaseForward)
                        .OnStart(() =>
                        {
                            if (EventStartWhen == EventWhen.Forward || EventStartWhen == EventWhen.Both)
                                OnStartEvent?.Invoke();
                            Animating = true;
                        });
                    break;
                case Style.LoopWithCount:
                    MainTween = MainTarget.DOScale(t, Duration)
                        .SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Forward ? DelayTime : 0)
                        .SetLoops(LoopCountForward * 2, LoopStyleForward)
                        .SetEase(EaseForward)
                        .OnStart(() =>
                        {
                            if (EventStartWhen == EventWhen.Forward || EventStartWhen == EventWhen.Both)
                                OnStartEvent?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (EventFinishWhen == EventWhen.Forward || EventFinishWhen == EventWhen.Both)
                                OnFinishEvent?.Invoke();
                            Animating = false;
                        });
                    break;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenReverse(Vector3 f, Vector3 t)
        {
            if (MainTween != null && MainTween.IsActive())
            {
                if (!MainTween.IsComplete())
                {
                    MainTween.Kill();
                }
            }
            else
            {
                MainTarget.localScale = f;
            }

            switch (SameInReverse ? StyleForward : StyleReverse)
            {
                case Style.Once:
                    MainTween = MainTarget.DOScale(t, Duration)
                        .SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Reverse ? DelayTime : 0)
                        .SetEase(SameInReverse ? EaseForward : EaseReverse)
                        .OnStart(() =>
                        {
                            if (EventStartWhen == EventWhen.Reverse || EventStartWhen == EventWhen.Both)
                                OnStartEvent?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (EventFinishWhen == EventWhen.Reverse || EventFinishWhen == EventWhen.Both)
                                OnFinishEvent?.Invoke();
                            Animating = false;
                            if (DisableAfterReverse) MainTarget.gameObject.SetActive(false);
                        });
                    break;
                case Style.Loop:
                    MainTween = MainTarget.DOScale(t, Duration)
                        .SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Reverse ? DelayTime : 0)
                        .SetLoops(-1, SameInReverse ? LoopStyleForward : LoopStyleReverse)
                        .SetEase(SameInReverse ? EaseForward : EaseReverse)
                        .OnStart(() =>
                        {
                            if (EventStartWhen == EventWhen.Reverse || EventStartWhen == EventWhen.Both)
                                OnStartEvent?.Invoke();
                            Animating = true;
                        });
                    break;
                case Style.LoopWithCount:
                    MainTween = MainTarget.DOScale(t, Duration)
                        .SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Reverse ? DelayTime : 0)
                        .SetLoops((SameInReverse ? LoopCountForward : LoopCountReverse) * 2, SameInReverse ? LoopStyleForward : LoopStyleReverse)
                        .SetEase(SameInReverse ? EaseForward : EaseReverse)
                        .OnStart(() =>
                        {
                            if (EventStartWhen == EventWhen.Reverse || EventStartWhen == EventWhen.Both)
                                OnStartEvent?.Invoke();
                            Animating = true;
                        })
                        .OnComplete(() =>
                        {
                            if (EventFinishWhen == EventWhen.Reverse || EventFinishWhen == EventWhen.Both)
                                OnFinishEvent?.Invoke();
                            Animating = false;
                            if (DisableAfterReverse) MainTarget.gameObject.SetActive(false);
                        });
                    break;
            }
        }

        #endregion

        #region Implement tween functions

        //
        public override void PlayForward()
        {
            if (!IsActive) return;
            SetTweenForward(FromValue, ToValue);
        }

        //
        public override void PlayReverse()
        {
            if (!IsActive) return;
            SetTweenReverse(ToValue, FromValue);
        }

        //
        public override void Play(bool forward = true)
        {
            if (forward) PlayForward();
            else PlayReverse();
        }

        //
        public override void Stop(bool complete = false)
        {
            MainTween.Kill(complete);
        }

        //
        public override void SetCurrentValueToStart()
        {
            var target = GetComponent<RectTransform>();
            FromValue = target.localScale;
        }

        //
        public override void SetCurrentValueToEnd()
        {
            var target = GetComponent<RectTransform>();
            ToValue = target.localScale;
        }

        //
        public override void SetStartToCurrentValue()
        {
            var target = GetComponent<RectTransform>();
            target.localScale = FromValue;
        }

        //
        public override void SetEndToCurrentValue()
        {
            var target = GetComponent<RectTransform>();
            target.localScale = ToValue;
        }

        #endregion
    }
}