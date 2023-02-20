using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class SunTweenControl : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Tween")]
        //
        [FoldoutGroup("Tween/Tween Group")]
        [SerializeField] private GroupTween groupTween;
        //
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool isActive = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool enableBeforeForward = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool disableAfterReverse = true;
        //
        [FoldoutGroup("Tween/Delay")]
        [SerializeField] private DelayWhen delayWhen = DelayWhen.None;
        [FoldoutGroup("Tween/Delay"), HideIf("delayWhen", DelayWhen.None)]
        [SerializeField] private float delayTime;
        //
        [FoldoutGroup("Tween/Event trigger")]
        [SerializeField] private EventWhen eventStartWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger")]
        [SerializeField] private EventWhen eventFinishWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger"), HideIf("eventStartWhen", EventWhen.None)]
        [SerializeField] private UnityEvent onStartEvent;
        [FoldoutGroup("Tween/Event trigger"), HideIf("eventFinishWhen", EventWhen.None)]
        [SerializeField] private UnityEvent onFinishEvent;
        //
        [FoldoutGroup("Tween/List Tween")]
        [SerializeField] private List<SunTween> listTween;
        
        //
        public GroupTween GroupTween
        {
            get => groupTween;
            set => groupTween = value;
        }
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        public bool EnableBeforeForward
        {
            get => enableBeforeForward;
            set => enableBeforeForward = value;
        }
        public bool DisableAfterReverse
        {
            get => disableAfterReverse;
            set => disableAfterReverse = value;
        }
        public DelayWhen DelayWhen
        {
            get => delayWhen;
            set => delayWhen = value;
        }
        public float DelayTime
        {
            get => delayTime;
            set => delayTime = value;
        }
        public EventWhen EventStartWhen
        {
            get => eventStartWhen;
            set => eventStartWhen = value;
        }
        public EventWhen EventFinishWhen
        {
            get => eventFinishWhen;
            set => eventFinishWhen = value;
        }
        public UnityEvent OnStartEvent
        {
            get => onStartEvent;
            set => onStartEvent = value;
        }
        public UnityEvent OnFinishEvent
        {
            get => onFinishEvent;
            set => onFinishEvent = value;
        }
        public List<SunTween> ListTween
        {
            get => listTween;
            set => listTween = value;
        }

        //
        public bool Animating { get; private set; }
        public float Duration { get; private set; }
        public GameObject MainTarget { get; set; }

        private Sequence _allTweenSequence;

        #endregion

        #region Functions

        //
        private void Reset()
        {
            if (MainTarget == null) MainTarget = transform.gameObject;
        }
        
        //
        private void Awake()
        {
            if (ListTween == null)
            {
                ListTween = new List<SunTween>();
                Debug.Log("Không có tween nào trong danh sách.");
            }

            if (MainTarget == null) MainTarget = transform.gameObject;
        }

        #endregion

        #region Tween functions

        //
        public void PlayForward()
        {
            if (!IsActive) return;
            if (ListTween == null || ListTween.Count <= 0) return;
            if (EnableBeforeForward) MainTarget.SetActive(true);

            _allTweenSequence = DOTween.Sequence();
            foreach (var tween in ListTween)
            {
                tween.PlayForward();
                if (tween.MainTween != null)
                {
                    _allTweenSequence.Join(tween.MainTween);
                }
            }

            Duration = _allTweenSequence.Duration();
            _allTweenSequence.SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Forward ? DelayTime : 0)
                .OnStart(() =>
                {
                    if (EventStartWhen == EventWhen.Forward || EventStartWhen == EventWhen.Both)
                    {
                        OnStartEvent?.Invoke();
                    }
                    Animating = true;
                })
                .OnComplete(() =>
                {
                    if (EventFinishWhen == EventWhen.Forward || EventFinishWhen == EventWhen.Both)
                    {
                        OnFinishEvent?.Invoke();
                    }
                    Animating = false;
                }).Play();
        }

        //
        public void PlayReverse()
        {
            if (!IsActive) return;
            if (ListTween == null || ListTween.Count <= 0) return;
            _allTweenSequence = DOTween.Sequence();
            foreach (var tween in ListTween)
            {
                tween.PlayReverse();
                if (tween.MainTween != null)
                {
                    _allTweenSequence.Join(tween.MainTween);
                }
            }

            Duration = _allTweenSequence.Duration();
            _allTweenSequence.SetDelay(DelayWhen == DelayWhen.Both || DelayWhen == DelayWhen.Reverse ? DelayTime : 0)
                .OnStart(() =>
                {
                    if (EventStartWhen == EventWhen.Reverse || EventStartWhen == EventWhen.Both)
                    {
                        OnStartEvent?.Invoke();
                    }
                    Animating = true;
                })
                .OnComplete(() =>
                {
                    if (EventFinishWhen == EventWhen.Reverse || EventFinishWhen == EventWhen.Both)
                    {
                        OnFinishEvent?.Invoke();
                    }
                    Animating = false;
                    if (DisableAfterReverse) MainTarget.SetActive(false);
                }).Play();
        }

        //
        public void Stop(bool complete = false)
        {
            _allTweenSequence?.Kill(complete);
        }
        
        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void AddTweenFromChildren()
        {
            if (ListTween == null) ListTween = new List<SunTween>();
            ListTween.Clear();

            var allTweenInChild = transform.GetComponentsInChildren<SunTween>();
            foreach (var tweenHelper in allTweenInChild)
            {
                if (tweenHelper.GroupTween == GroupTween)
                {
                    ListTween.Add(tweenHelper);
                }
            }

            ListTween.Sort((tween1, tween2) =>
            {
                if (tween1.TweenIndex < tween2.TweenIndex) return -1;
                if (tween1.TweenIndex == tween2.TweenIndex) return 0;
                return 1;
            });
        }

        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void ResetListTween()
        {
            if (ListTween == null) ListTween = new List<SunTween>();
            ListTween.Clear();
        }

        #endregion
    }
}