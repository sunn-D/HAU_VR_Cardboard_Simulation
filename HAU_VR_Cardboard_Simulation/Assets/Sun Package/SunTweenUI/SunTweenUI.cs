using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Sun_Package
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

    public abstract class SunTweenUI : MonoBehaviour
    {
        #region Variables
        
        //
        [FoldoutGroup("Tween")]
        //
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] protected GroupTween groupTween;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] protected int tweenIndex;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] protected bool isActive = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] protected float duration = .5f;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] protected RectTransform mainTarget;
        //
        [FoldoutGroup("Tween/Autoplay Tween")]
        [SerializeField] protected bool isAutoPlay;
        [FoldoutGroup("Tween/Autoplay Tween"), ShowIf("isAutoPlay")] 
        [SerializeField] protected Direction direction = Direction.Forward;
        //
        [FoldoutGroup("Tween/Enable - Disable")]
        [SerializeField] protected bool enableBeforeForward;
        [FoldoutGroup("Tween/Enable - Disable")]
        [SerializeField] protected bool disableAfterReverse;
        //
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] protected bool sameStyleInReverse = true;
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] protected Ease easeForward = Ease.OutBack;
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] protected Style styleForward = Style.Once;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameStyleInReverse")]
        [SerializeField] protected Ease easeReverse = Ease.InBack;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameStyleInReverse")]
        [SerializeField] protected Style styleReverse = Style.Once;
        //
        [FoldoutGroup("Tween/Loop style")]
        [SerializeField] protected LoopType loopStyle = LoopType.Yoyo;
        [FoldoutGroup("Tween/Loop style")]
        [SerializeField] protected int loopCount = -1;
        //
        [FoldoutGroup("Tween/Delay")]
        [SerializeField] protected DelayWhen delayWhen = DelayWhen.None;
        [FoldoutGroup("Tween/Delay"), HideIf("delayWhen", DelayWhen.None)]
        [SerializeField] protected float delay;
        //
        [FoldoutGroup("Tween/Event trigger")]
        [SerializeField] protected EventWhen startEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger")]
        [SerializeField] protected EventWhen finishedEventWhen = EventWhen.None;
        [FoldoutGroup("Tween/Event trigger"), HideIf("startEventWhen", EventWhen.None)]
        [SerializeField] protected UnityEvent onStartEvent;
        [FoldoutGroup("Tween/Event trigger"), HideIf("finishedEventWhen", EventWhen.None)]
        [SerializeField] protected UnityEvent onFinishedEvent;

        //
        public GroupTween GroupTween
        {
            get => groupTween;
            set => groupTween = value;
        }
        public int TweenIndex
        {
            get => tweenIndex;
            set => tweenIndex = value;
        }
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

        //
        public bool Animating { get; set; }
        public Tween MainTween { get; set; }

        #endregion

        #region Functions

        //
        private void Reset()
        {
            mainTarget = GetComponent<RectTransform>();
            LoadInReset();
        }

        //
        private void Awake()
        {
            LoadInAwake();
        }

        //
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

        //
        public virtual void LoadInReset() { }
        public virtual void LoadInAwake() { }

        //
        public abstract void PlayForward();
        public abstract void PlayReverse();
        public abstract void Play(bool forward = true);
        public abstract void Stop(bool complete = false);
        
        //
        [FoldoutGroup("Button")]
        [Button(ButtonSizes.Large)]
        public abstract void SetCurrentValueToStart();
        [FoldoutGroup("Button")]
        [Button(ButtonSizes.Large)]
        public abstract void SetCurrentValueToEnd();
        [FoldoutGroup("Button")]
        [PropertySpace, Button(ButtonSizes.Large)]
        public abstract void SetStartToCurrentValue();
        [FoldoutGroup("Button")]
        [Button(ButtonSizes.Large)]
        public abstract void SetEndToCurrentValue();
        [FoldoutGroup("Button")]
        [PropertySpace, Button(ButtonSizes.Large)]
        public void SetMainTarget()
        {
            mainTarget = GetComponent<RectTransform>();
        }

        #endregion
    }
}