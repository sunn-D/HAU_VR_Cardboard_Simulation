using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Sun_Package
{
    public class SunTweenUIAlpha : SunTweenUI
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

        #endregion

        #region Functions

        //
        public override void LoadInReset()
        {
            canvasGroup = mainTarget.GetComponent<CanvasGroup>();
            graphic = mainTarget.GetComponent<Graphic>();

            if (canvasGroup != null)
            {
                isCanvasGroup = true;
            }
            else if (graphic != null)
            {
                isCanvasGroup = false;
            }
            else
            {
                canvasGroup = mainTarget.gameObject.AddComponent<CanvasGroup>();
                isCanvasGroup = true;
            }
        }
        
        //
        public override void LoadInAwake()
        {
            if (isCanvasGroup)
            {
                if (canvasGroup == null) canvasGroup = mainTarget.GetComponent<CanvasGroup>();
                if (canvasGroup == null) canvasGroup = mainTarget.gameObject.AddComponent<CanvasGroup>();
            }
            else
            {
                if (graphic == null) graphic = mainTarget.GetComponent<Graphic>();
            }
        }

        #endregion

        #region Tween

        //
        private void SetTweenForward(float f, float t)
        {
            if (MainTween != null && MainTween.IsActive())
            {
                if (!MainTween.IsComplete())
                    MainTween.Kill();
            }
            else
            {
                if (isCanvasGroup)
                    canvasGroup.alpha = f;
                else
                    graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, @f);
            }

            if (enableBeforeForward)
                mainTarget.gameObject.SetActive(true);

            if (isCanvasGroup)
            {
                switch (styleForward)
                {
                    case Style.Once:
                        MainTween = canvasGroup.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                            .SetEase(easeForward)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                            });
                        break;
                    case Style.Loop:
                        MainTween = canvasGroup.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                            .SetLoops(-1, loopStyle)
                            .SetEase(easeForward)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            });
                        break;
                    case Style.LoopWithCount:
                        MainTween = canvasGroup.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                            .SetLoops(loopCount * 2, loopStyle)
                            .SetEase(easeForward)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                            });
                        break;
                }
            }
            else
            {
                switch (styleForward)
                {
                    case Style.Once:
                        MainTween = graphic.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                            .SetEase(easeForward)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                            });
                        break;
                    case Style.Loop:
                        MainTween = graphic.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                            .SetLoops(-1, loopStyle)
                            .SetEase(easeForward)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            });
                        break;
                    case Style.LoopWithCount:
                        MainTween = graphic.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                            .SetLoops(loopCount * 2, loopStyle)
                            .SetEase(easeForward)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                            });
                        break;
                }
            }
        }

        //
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
                if (isCanvasGroup) canvasGroup.alpha = f;
                else graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, @f);
            }

            if (isCanvasGroup)
            {
                switch (sameStyleInReverse ? styleForward : styleReverse)
                {
                    case Style.Once:
                        MainTween = canvasGroup.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                            .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                                if (disableAfterReverse) mainTarget.gameObject.SetActive(false);
                            });
                        break;
                    case Style.Loop:
                        MainTween = canvasGroup.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                            .SetLoops(-1, loopStyle)
                            .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            });
                        break;
                    case Style.LoopWithCount:
                        MainTween = canvasGroup.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                            .SetLoops(loopCount * 2, loopStyle)
                            .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                                if (disableAfterReverse) mainTarget.gameObject.SetActive(false);
                            });
                        break;
                }
            }
            else
            {
                switch (sameStyleInReverse ? styleForward : styleReverse)
                {
                    case Style.Once:
                        MainTween = graphic.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                            .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                                if (disableAfterReverse) mainTarget.gameObject.SetActive(false);
                            });
                        break;
                    case Style.Loop:
                        MainTween = graphic.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                            .SetLoops(-1, loopStyle)
                            .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            });
                        break;
                    case Style.LoopWithCount:
                        MainTween = graphic.DOFade(t, duration)
                            .SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                            .SetLoops(loopCount * 2, loopStyle)
                            .SetEase(sameStyleInReverse ? easeForward : easeReverse)
                            .OnStart(() =>
                            {
                                if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                                    onStartEvent?.Invoke();
                                Animating = true;
                            })
                            .OnComplete(() =>
                            {
                                if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both)
                                    onFinishedEvent?.Invoke();
                                Animating = false;
                                if (disableAfterReverse) mainTarget.gameObject.SetActive(false);
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
            if (!isActive) return;
            SetTweenForward(fromValue, toValue);
        }

        //
        public override void PlayReverse()
        {
            if (!isActive) return;
            SetTweenReverse(toValue, fromValue);
        }

        //
        public override void Play(bool forward = true)
        {
            if (forward)
                PlayForward();
            else
                PlayReverse();
        }

        //
        public override void Stop(bool complete = false)
        {
            MainTween.Kill(complete);
        }

        //
        public override void SetCurrentValueToStart()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                fromValue = target.alpha;
            }
            else
            {
                var target = GetComponent<Graphic>();
                fromValue = target.color.a;
            }
        }

        //
        public override void SetCurrentValueToEnd()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                toValue = target.alpha;
            }
            else
            {
                var target = GetComponent<Graphic>();
                toValue = target.color.a;
            }
        }

        //
        public override void SetStartToCurrentValue()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                target.alpha = fromValue;
            }
            else
            {
                var target = GetComponent<Graphic>();
                target.color = new Color(target.color.r, target.color.g, target.color.b, fromValue);
            }
        }

        //
        public override void SetEndToCurrentValue()
        {
            if (isCanvasGroup)
            {
                var target = GetComponent<CanvasGroup>();
                target.alpha = toValue;
            }
            else
            {
                var target = GetComponent<Graphic>();
                target.color = new Color(target.color.r, target.color.g, target.color.b, toValue);
            }
        }

        #endregion
    }
}