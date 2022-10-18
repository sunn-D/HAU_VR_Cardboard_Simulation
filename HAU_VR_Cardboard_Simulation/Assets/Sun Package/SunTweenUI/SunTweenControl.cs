using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DunnGSunn
{
    public class SunTweenControl : MonoBehaviour
    {
        #region Fields

        [FoldoutGroup("Tween")]
        [FoldoutGroup("Tween/Tween Base Info")]
        public GroupTween groupTween;
        [FoldoutGroup("Tween/Tween Base Info")]
        public bool isActive = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        public GameObject mainTarget;

        [FoldoutGroup("Tween/Enable - Disable")]
        public bool enableBeforeForward = true;
        [FoldoutGroup("Tween/Enable - Disable")]
        public bool disableAfterReverse = true;

        [FoldoutGroup("Tween/Delay")]
        public DelayWhen delayWhen = DelayWhen.None;
        [FoldoutGroup("Tween/Delay"), HideIf("delayWhen", DelayWhen.None)]
        public float delay;

        [FoldoutGroup("Tween/List Tween")]
        public List<SunTween> listTween;
        
        [FoldoutGroup("Tween/Event trigger")]
        public EventWhen startEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger")]
        public EventWhen finishedEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger"), HideIf("startEventWhen", EventWhen.None)]
        public UnityEvent onStart;
        [FoldoutGroup("Tween/Event trigger"), HideIf("finishedEventWhen", EventWhen.None)]
        public UnityEvent onFinished;

        public bool Animating { get; private set; }
        public float Duration { get; private set; }

        private Sequence _allTweenSequence;

        #endregion

        #region Unity callback functions

        private void Reset()
        {
            if (mainTarget == null) mainTarget = transform.gameObject;
        }

        private void Awake()
        {
            if (listTween == null)
            {
                listTween = new List<SunTween>();
                Debug.Log("Không có tween nào trong danh sách.");
            }

            if (mainTarget == null) mainTarget = transform.gameObject;
        }

        #endregion

        #region Tween functions

        [FoldoutGroup("Button"), Button]
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
                        onStart?.Invoke();
                    }
                    Animating = true;
                })
                .OnComplete(() =>
                {
                    if (finishedEventWhen == EventWhen.Forward || finishedEventWhen == EventWhen.Both)
                    {
                        onFinished?.Invoke();
                    }
                    Animating = false;
                })
                .Play();
        }

        [FoldoutGroup("Button"), Button]
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
                        onStart?.Invoke();
                    }
                    Animating = true;
                })
                .OnComplete(() =>
                {
                    if (finishedEventWhen == EventWhen.Reverse || finishedEventWhen == EventWhen.Both)
                    {
                        onFinished?.Invoke();
                    }
                    Animating = false;
                    if (disableAfterReverse) mainTarget.SetActive(false);
                })
                .Play();
        }

        public void Stop(bool complete = false)
        {
            _allTweenSequence?.Kill(complete);
        }

        [FoldoutGroup("Button"), Button]
        public void AddTweenFromChildren()
        {
            if (listTween == null) listTween = new List<SunTween>();
            listTween.Clear();

            var allTweenInChild = transform.GetComponentsInChildren<SunTween>();
            foreach (var tweenHelper in allTweenInChild)
            {
                if (tweenHelper.groupTween == groupTween)
                {
                    listTween.Add(tweenHelper);
                }
            }

            listTween.Sort((tween1, tween2) =>
            {
                if (tween1.tweenIndex < tween2.tweenIndex) return -1;
                if (tween1.tweenIndex == tween2.tweenIndex) return 0;
                return 1;
            });
        }

        [FoldoutGroup("Button"), Button]
        public void ResetListTween()
        {
            if (listTween == null) listTween = new List<SunTween>();
            listTween.Clear();
        }

        #endregion
    }
}