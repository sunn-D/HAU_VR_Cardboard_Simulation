using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class SunTweenColor : SunTween
    {
        #region Variables

        //
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private Color fromValue = Color.black;
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private Color toValue = Color.white;
        
        //
        public Color FromValue
        {
            get => fromValue;
            set => fromValue = value;
        }
        public Color ToValue
        {
            get => toValue;
            set => toValue = value;
        }

        //
        private Graphic _graphic;

        #endregion

        #region Functions

        //
        public override void LoadInReset()
        {
            _graphic = MainTarget.GetComponent<Graphic>();
        }

        //s
        public override void LoadInAwake()
        {
            if (_graphic == null) _graphic = MainTarget.GetComponent<Graphic>();
        }

        #endregion

        #region Tween

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenForward(Color f, Color t)
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
                _graphic.color = f;
            }

            if (EnableBeforeForward) MainTarget.gameObject.SetActive(true);
            switch (StyleForward)
            {
                case Style.Once:
                    MainTween = _graphic.DOColor(t, Duration)
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
                    MainTween = _graphic.DOColor(t, Duration)
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
                    MainTween = _graphic.DOColor(t, Duration)
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
        private void SetTweenReverse(Color f, Color t)
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
                _graphic.color = f;
            }

            switch (SameInReverse ? StyleForward : StyleReverse)
            {
                case Style.Once:
                    MainTween = _graphic.DOColor(t, Duration)
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
                    MainTween = _graphic.DOColor(t, Duration)
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
                    MainTween = _graphic.DOColor(t, Duration)
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
            var target = GetComponent<Graphic>();
            FromValue = target.color;
        }

        //
        public override void SetCurrentValueToEnd()
        {
            var target = GetComponent<Graphic>();
            ToValue = target.color;
        }

        //
        public override void SetStartToCurrentValue()
        {
            var target = GetComponent<Graphic>();
            target.color = FromValue;
        }

        //
        public override void SetEndToCurrentValue()
        {
            var target = GetComponent<Graphic>();
            target.color = ToValue;
        }

        #endregion
    }
}