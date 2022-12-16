using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sun_Package
{
    public class SunTweenUIPosition : SunTweenUI
    {
        #region Variables

        //
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private Vector2 fromValue = Vector2.zero;
        [FoldoutGroup("Tween/From - To Value")]
        [SerializeField] private Vector2 toValue = Vector2.zero;
        //
        [FoldoutGroup("Tween/Other Option")]
        [SerializeField] private bool reverseIncremental;

        //
        public Vector2 FromValue
        {
            get => fromValue;
            set => fromValue = value;
        }
        public Vector2 ToValue
        {
            get => toValue;
            set => toValue = value;
        }

        #endregion

        #region Tween

        // 
        private void SetTweenForward(Vector2 f, Vector2 t)
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
                mainTarget.anchoredPosition = f;
            }

            if (enableBeforeForward) mainTarget.gameObject.SetActive(true);
            switch (styleForward)
            {
                case Style.Once:
                    MainTween = mainTarget.DOAnchorPos(t, duration)
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
                    MainTween = mainTarget.DOAnchorPos(t, duration)
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
                    MainTween = mainTarget.DOAnchorPos(t, duration)
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
        private void SetTweenReverse(Vector2 f, Vector2 t)
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
                mainTarget.anchoredPosition = f;
            }

            if (!reverseIncremental)
            {
                switch (sameStyleInReverse ? styleForward : styleReverse)
                {
                    case Style.Once:
                        MainTween = mainTarget.DOAnchorPos(t, duration)
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
                        MainTween = mainTarget.DOAnchorPos(t, duration)
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
                        MainTween = mainTarget.DOAnchorPos(t, duration)
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
                MainTween = mainTarget.DOAnchorPos(f + (f - t), duration)
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
            }
        }

        #endregion

        #region Functions

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
            var target = GetComponent<RectTransform>();
            fromValue = target.anchoredPosition;
        }

        //
        public override void SetCurrentValueToEnd()
        {
            var target = GetComponent<RectTransform>();
            toValue = target.anchoredPosition;
        }

        //
        public override void SetStartToCurrentValue()
        {
            var target = GetComponent<RectTransform>();
            target.anchoredPosition = fromValue;
        }

        //
        public override void SetEndToCurrentValue()
        {
            var target = GetComponent<RectTransform>();
            target.anchoredPosition = toValue;
        }

        #endregion
    }
}