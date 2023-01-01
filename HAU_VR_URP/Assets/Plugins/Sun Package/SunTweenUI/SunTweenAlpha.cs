using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class SunTweenAlpha : SunTween
    {
        #region Variables

        //
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private float fromValue;
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private float toValue;
        //
        [FoldoutGroup("Tween/Type Of Target")]
        [SerializeField] private bool isCanvasGroup;
        [FoldoutGroup("Tween/Type Of Target"), HideIf("isCanvasGroup")]
        [SerializeField] private Graphic graphic;
        [FoldoutGroup("Tween/Type Of Target"), ShowIf("isCanvasGroup")]
        [SerializeField] private CanvasGroup canvasGroup;
        
        //
        public float FromValue
        {
            get => fromValue;
            set => fromValue = value;
        }
        public float ToValue
        {
            get => toValue;
            set => toValue = value;
        }
        public bool IsCanvasGroup
        {
            get => isCanvasGroup;
            set => isCanvasGroup = value;
        }
        public Graphic Graphic
        {
            get => graphic;
            set => graphic = value;
        }
        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
            set => canvasGroup = value;
        }

        #endregion

        #region Functions

        //
        public override void LoadInReset()
        {
            CanvasGroup = MainTarget.GetComponent<CanvasGroup>();
            Graphic = MainTarget.GetComponent<Graphic>();

            if (CanvasGroup != null) IsCanvasGroup = true;
            else if (Graphic != null) IsCanvasGroup = false;
            else
            {
                CanvasGroup = MainTarget.gameObject.GetOrAddComponent<CanvasGroup>();
                IsCanvasGroup = true;
            }
        }

        //
        public override void LoadInAwake()
        {
            if (IsCanvasGroup)
            {
                if (CanvasGroup == null) CanvasGroup = MainTarget.gameObject.GetOrAddComponent<CanvasGroup>();
            }
            else
            {
                if (Graphic == null) Graphic = MainTarget.GetComponent<Graphic>();
            }
        }

        #endregion

        #region Tween

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenForward(float f, float t)
        {
            //
            if (MainTween != null && MainTween.IsActive())
            {
                if (!MainTween.IsComplete()) MainTween.Kill();
            }
            else
            {
                if (IsCanvasGroup) CanvasGroup.alpha = f;
                else Graphic.color = new Color(Graphic.color.r, Graphic.color.g, Graphic.color.b, @f);
            }

            //
            if (EnableBeforeForward) MainTarget.gameObject.SetActive(true);

            //
            if (IsCanvasGroup)
            {
                switch (StyleForward)
                {
                    case Style.Once:
                        MainTween = CanvasGroup.DOFade(t, Duration)
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
                        MainTween = CanvasGroup.DOFade(t, Duration)
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
                        MainTween = CanvasGroup.DOFade(t, Duration)
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
            else
            {
                switch (StyleForward)
                {
                    case Style.Once:
                        MainTween = Graphic.DOFade(t, Duration)
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
                        MainTween = Graphic.DOFade(t, Duration)
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
                        MainTween = Graphic.DOFade(t, Duration)
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
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTweenReverse(float f, float t)
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
                if (IsCanvasGroup) CanvasGroup.alpha = f;
                else Graphic.color = new Color(Graphic.color.r, Graphic.color.g, Graphic.color.b, @f);
            }

            if (IsCanvasGroup)
            {
                switch (SameInReverse ? StyleForward : StyleReverse)
                {
                    case Style.Once:
                        MainTween = CanvasGroup.DOFade(t, Duration)
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
                        MainTween = CanvasGroup.DOFade(t, Duration)
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
                        MainTween = CanvasGroup.DOFade(t, Duration)
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
            else
            {
                switch (SameInReverse ? StyleForward : StyleReverse)
                {
                    case Style.Once:
                        MainTween = Graphic.DOFade(t, Duration)
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
                        MainTween = Graphic.DOFade(t, Duration)
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
                        MainTween = Graphic.DOFade(t, Duration)
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
            if (IsCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                FromValue = target.alpha;
            }
            else
            {
                var target = GetComponent<Graphic>();
                FromValue = target.color.a;
            }
        }

        //
        public override void SetCurrentValueToEnd()
        {
            if (IsCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                ToValue = target.alpha;
            }
            else
            {
                var target = GetComponent<Graphic>();
                ToValue = target.color.a;
            }
        }

        //
        public override void SetStartToCurrentValue()
        {
            if (IsCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                target.alpha = FromValue;
            }
            else
            {
                var target = GetComponent<Graphic>();
                target.color = new Color(target.color.r, target.color.g, target.color.b, FromValue);
            }
        }

        //
        public override void SetEndToCurrentValue()
        {
            if (IsCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                target.alpha = ToValue;
            }
            else
            {
                var target = GetComponent<Graphic>();
                target.color = new Color(target.color.r, target.color.g, target.color.b, ToValue);
            }
        }

        #endregion
    }
}