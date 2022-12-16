using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Sun_Package
{
    public class SunTweenUIColor : SunTweenUI
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
            _graphic = mainTarget.GetComponent<Graphic>();
        }

        //
        public override void LoadInAwake()
        {
            if (_graphic == null) _graphic = mainTarget.GetComponent<Graphic>();
        }

        #endregion

        #region Tween

        //
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

            if (enableBeforeForward) mainTarget.gameObject.SetActive(true);
            switch (styleForward)
            {
                case Style.Once:
                    MainTween = _graphic.DOColor(t, duration)
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
                    MainTween = _graphic.DOColor(t, duration)
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
                    MainTween = _graphic.DOColor(t, duration)
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

        //
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

            switch (sameStyleInReverse ? styleForward : styleReverse)
            {
                case Style.Once:
                    MainTween = _graphic.DOColor(t, duration)
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
                    MainTween = _graphic.DOColor(t, duration)
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
                    MainTween = _graphic.DOColor(t, duration)
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
            var target = GetComponent<Graphic>();
            fromValue = target.color;
        }

        //
        public override void SetCurrentValueToEnd()
        {
            var target = GetComponent<Graphic>();
            toValue = target.color;
        }

        //
        public override void SetStartToCurrentValue()
        {
            var target = GetComponent<Graphic>();
            target.color = fromValue;
        }

        //
        public override void SetEndToCurrentValue()
        {
            var target = GetComponent<Graphic>();
            target.color = toValue;
        }

        #endregion
    }
}