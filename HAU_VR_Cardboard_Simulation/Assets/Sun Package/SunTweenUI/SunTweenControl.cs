using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Sun_Package
{
    public class SunTweenControl : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Tween")]
        //
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private GroupTween groupTween;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool isActive = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private GameObject mainTarget;
        //
        [FoldoutGroup("Tween/Enable - Disable")]
        [SerializeField] private bool enableBeforeForward = true;
        [FoldoutGroup("Tween/Enable - Disable")]
        [SerializeField] private bool disableAfterReverse = true;
        //
        [FoldoutGroup("Tween/Delay")]
        [SerializeField] private DelayWhen delayWhen = DelayWhen.None;
        [FoldoutGroup("Tween/Delay"), HideIf("delayWhen", DelayWhen.None)]
        [SerializeField] private float delay;
        //
        [FoldoutGroup("Tween/List Tween")]
        [SerializeField] private List<SunTweenUI> listTween;
        //
        [FoldoutGroup("Tween/Event trigger")]
        [SerializeField] private EventWhen startEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger")]
        [SerializeField] private EventWhen finishedEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger"), HideIf("startEventWhen", EventWhen.None)]
        [SerializeField] private UnityEvent onStartEvent;
        [FoldoutGroup("Tween/Event trigger"), HideIf("finishedEventWhen", EventWhen.None)]
        [SerializeField] private UnityEvent onFinishedEvent;
        
        //
        public UnityEvent OnStartEvent
        {
            get => onStartEvent;
            set => onStartEvent = value;
        }
        public UnityEvent OnFinishedEvent
        {
            get => onFinishedEvent;
            set => onFinishedEvent = value;
        }
        public EventWhen StartEventWhen
        {
            get => startEventWhen;
            set => startEventWhen = value;
        }
        public EventWhen FinishedEventWhen
        {
            get => finishedEventWhen;
            set => finishedEventWhen = value;
        }

        //
        public bool Animating { get; private set; }
        public float Duration { get; private set; }

        //
        private Sequence _allTweenSequence;

        #endregion

        #region Functions

        //
        private void Reset()
        {
            if (mainTarget == null) mainTarget = transform.gameObject;
        }

        //
        private void Awake()
        {
            if (listTween == null)
            {
                listTween = new List<SunTweenUI>();
                Debug.Log("Không có tween nào trong danh sách.");
            }

            if (mainTarget == null) mainTarget = transform.gameObject;
        }

        #endregion

        #region Tween functions

        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void PlayForward()
        {
            if (!isActive) return;
            if (listTween == null || listTween.Count <= 0) return;
            if (enableBeforeForward) mainTarget.SetActive(true);

            _allTweenSequence = DOTween.Sequence();
            foreach (var tween in listTween)
            {
                tween.PlayForward();
                if (tween.MainTween != null)
                {
                    _allTweenSequence.Join(tween.MainTween);
                }
            }

            Duration = _allTweenSequence.Duration();
            _allTweenSequence.SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Forward ? delay : 0)
                .OnStart(() =>
                {
                    if (startEventWhen == EventWhen.Forward || startEventWhen == EventWhen.Both)
                    {
                        onStartEvent?.Invoke();
                    }
                    Animating = true;
                })
                .OnComplete(() =>
                {
                    if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both)
                    {
                        onFinishedEvent?.Invoke();
                    }
                    Animating = false;
                })
                .Play();
        }

        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void PlayReverse()
        {
            if (!isActive) return;
            if (listTween == null || listTween.Count <= 0) return;
            _allTweenSequence = DOTween.Sequence();
            foreach (var tween in listTween)
            {
                tween.PlayReverse();
                if (tween.MainTween != null)
                {
                    _allTweenSequence.Join(tween.MainTween);
                }
            }

            Duration = _allTweenSequence.Duration();
            _allTweenSequence.SetDelay(delayWhen == DelayWhen.Both || delayWhen == DelayWhen.Reverse ? delay : 0)
                .OnStart(() =>
                {
                    if (startEventWhen == EventWhen.Reverse || startEventWhen == EventWhen.Both)
                    {
                        onStartEvent?.Invoke();
                    }
                    Animating = true;
                })
                .OnComplete(() =>
                {
                    if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both)
                    {
                        onFinishedEvent?.Invoke();
                    }
                    Animating = false;
                    if (disableAfterReverse) mainTarget.SetActive(false);
                })
                .Play();
        }

        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void Stop(bool complete = false)
        {
            _allTweenSequence?.Kill(complete);
        }

        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void AddTweenFromChildren()
        {
            if (listTween == null) listTween = new List<SunTweenUI>();
            listTween.Clear();

            var allTweenInChild = transform.GetComponentsInChildren<SunTweenUI>();
            foreach (var tweenHelper in allTweenInChild)
            {
                if (tweenHelper.GroupTween == groupTween)
                {
                    listTween.Add(tweenHelper);
                }
            }

            listTween.Sort((tween1, tween2) =>
            {
                if (tween1.TweenIndex < tween2.TweenIndex) return -1;
                if (tween1.TweenIndex == tween2.TweenIndex) return 0;
                return 1;
            });
        }

        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void ResetListTween()
        {
            if (listTween == null) listTween = new List<SunTweenUI>();
            listTween.Clear();
        }

        #endregion
    }
}