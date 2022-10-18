using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DunnGSunn
{
    // ================== //
    public enum Style { Once, Loop, LoopWithCount }

    // ================== //
    public enum Direction { Forward, Reverse }

    // ================== //
    public enum EventWhen { None, Forward, Reverse, Both }

    // ================== //
    public enum DelayWhen { None, Forward, Reverse, Both }

    // ================== //
    public enum GroupTween { Show, Hide, Other }

    public abstract class SunTween : MonoBehaviour
    {
        #region Fields
        
        [FoldoutGroup("Tween")]
        [FoldoutGroup("Tween/Tween Base Info")]
        public GroupTween groupTween;
        [FoldoutGroup("Tween/Tween Base Info")]
        public int tweenIndex;
        [FoldoutGroup("Tween/Tween Base Info")]
        public bool isActive = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        public float duration = .5f;
        [FoldoutGroup("Tween/Tween Base Info")]
        public RectTransform mainTarget;

        [FoldoutGroup("Tween/Autoplay Tween")]
        public bool isAutoPlay;
        [FoldoutGroup("Tween/Autoplay Tween"), ShowIf("isAutoPlay")] 
        public Direction direction = Direction.Forward;

        [FoldoutGroup("Tween/Enable - Disable")]
        public bool enableBeforeForward;
        [FoldoutGroup("Tween/Enable - Disable")]
        public bool disableAfterReverse;

        [FoldoutGroup("Tween/Tween style")]
        public bool sameStyleInReverse = true;
        [FoldoutGroup("Tween/Tween style")]
        public Ease easeForward = Ease.OutBack;
        [FoldoutGroup("Tween/Tween style")]
        public Style styleForward = Style.Once;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameStyleInReverse")]
        public Ease easeReverse = Ease.InBack;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameStyleInReverse")]
        public Style styleReverse = Style.Once;

        [FoldoutGroup("Tween/Loop style")]
        public LoopType loopStyle = LoopType.Yoyo;
        [FoldoutGroup("Tween/Loop style")]
        public int loopCount = -1;

        [FoldoutGroup("Tween/Delay")]
        public DelayWhen delayWhen = DelayWhen.None;
        [FoldoutGroup("Tween/Delay"), HideIf("delayWhen", DelayWhen.None)]
        public float delay;

        [FoldoutGroup("Tween/Event trigger")]
        public EventWhen startEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger")]
        public EventWhen finishedEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger"), HideIf("startEventWhen", EventWhen.None)]
        public UnityEvent onStart;
        [FoldoutGroup("Tween/Event trigger"), HideIf("finishedEventWhen", EventWhen.None)]
        public UnityEvent onFinished;

        public bool Animating { get; set; }
        public Tween MainTween { get; set; }

        #endregion

        #region Unity callback functions

        private void Reset()
        {
            mainTarget = GetComponent<RectTransform>();
            LoadInReset();
        }

        private void Awake()
        {
            LoadInAwake();
        }

        private void OnEnable()
        {
            if (isActive && isAutoPlay)
            {
                switch (direction)
                {
                    case Direction.Forward:
                        PlayForward();
                        break;
                    case Direction.Reverse:
                        PlayReverse();
                        break;
                }
            }
        }

        #endregion

        #region Tween functions

        public virtual void LoadInReset() { }
        public virtual void LoadInAwake() { }

        public abstract void PlayForward();
        public abstract void PlayReverse();
        public abstract void Play(bool forward = true);
        public abstract void Stop(bool complete = false);
        
        [FoldoutGroup("Button")]
        [Button("Set Current Value To Start Value")]
        public abstract void SetCurrentValueToStart();
        [FoldoutGroup("Button")]
        [Button("Set Current Value To End Value")]
        public abstract void SetCurrentValueToEnd();

        [FoldoutGroup("Button")]
        [PropertySpace, Button("Set Start Value To Current Value")]
        public abstract void SetStartToCurrentValue();
        [FoldoutGroup("Button")]
        [Button("Set End Value To Current Value")]
        public abstract void SetEndToCurrentValue();

        [FoldoutGroup("Button")]
        [PropertySpace, Button("Set Main Target")]
        public void SetMainTarget()
        {
            mainTarget = GetComponent<RectTransform>();
        }

        public void AddListenerToStart(UnityAction listener) => onStart.AddListener(listener);
        public void AddListenerToEnd(UnityAction listener) => onFinished.AddListener(listener);

        #endregion
    }
}