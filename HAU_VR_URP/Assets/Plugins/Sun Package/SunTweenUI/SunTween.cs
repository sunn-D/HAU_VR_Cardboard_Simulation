using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable CheckNamespace

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

    public abstract class SunTween : MonoBehaviour
    {
        #region Variables
        
        [FoldoutGroup("Tween")]
        //
        [FoldoutGroup("Tween/Tween Group")]
        [SerializeField] private GroupTween groupTween;
        [FoldoutGroup("Tween/Tween Group")]
        [SerializeField] private int tweenIndex;
        //
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool isActive = true;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool isAutoPlay;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool enableBeforeForward;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private bool disableAfterReverse;
        [FoldoutGroup("Tween/Tween Base Info"), ShowIf("isAutoPlay")] 
        [SerializeField] private Direction directionAutoPlay = Direction.Forward;
        [FoldoutGroup("Tween/Tween Base Info")]
        [SerializeField] private float duration = .5f;
        //
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] private bool sameInReverse = true;
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] private Ease easeForward = Ease.OutBack;
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] private Style styleForward = Style.Once;
        [FoldoutGroup("Tween/Tween style")] 
        [SerializeField] private LoopType loopStyleForward = LoopType.Yoyo;
        [FoldoutGroup("Tween/Tween style")]
        [SerializeField] private int loopCountForward = -1;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameInReverse")]
        [SerializeField] private Ease easeReverse = Ease.InBack;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameInReverse")]
        [SerializeField] private Style styleReverse = Style.Once;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameInReverse")] 
        [SerializeField] private LoopType loopStyleReverse = LoopType.Yoyo;
        [FoldoutGroup("Tween/Tween style"), HideIf("sameInReverse")]
        [SerializeField] private int loopCountReverse = -1;
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
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        public float Duration
        {
            get => duration;
            set => duration = value;
        }
        public bool IsAutoPlay
        {
            get => isAutoPlay;
            set => isAutoPlay = value;
        }
        public Direction DirectionAutoPlay
        {
            get => directionAutoPlay;
            set => directionAutoPlay = value;
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
        public bool SameInReverse
        {
            get => sameInReverse;
            set => sameInReverse = value;
        }
        public Ease EaseForward
        {
            get => easeForward;
            set => easeForward = value;
        }
        public Style StyleForward
        {
            get => styleForward;
            set => styleForward = value;
        }
        public Ease EaseReverse
        {
            get => easeReverse;
            set => easeReverse = value;
        }
        public Style StyleReverse
        {
            get => styleReverse;
            set => styleReverse = value;
        }
        public LoopType LoopStyleForward
        {
            get => loopStyleForward;
            set => loopStyleForward = value;
        }
        public int LoopCountForward
        {
            get => loopCountForward;
            set => loopCountForward = value;
        }
        public LoopType LoopStyleReverse
        {
            get => loopStyleReverse;
            set => loopStyleReverse = value;
        }
        public int LoopCountReverse
        {
            get => loopCountReverse;
            set => loopCountReverse = value;
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

        //
        public RectTransform MainTarget { get; set; }
        public bool Animating { get; set; }
        public Tween MainTween { get; set; }

        #endregion

        #region Functions

        //
        private void OnValidate()
        {
            MainTarget = GetComponent<RectTransform>();
            LoadInValidate();
        }

        //
        private void Reset()
        {
            MainTarget = GetComponent<RectTransform>();
            LoadInReset();
        }

        //
        private void Awake()
        {
            if (MainTarget == null) MainTarget = GetComponent<RectTransform>();
            LoadInAwake();
        }

        //
        private void OnEnable()
        {
            if (IsActive && IsAutoPlay)
            {
                switch (DirectionAutoPlay)
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
        public virtual void LoadInValidate() { }
        public virtual void LoadInReset() { }
        public virtual void LoadInAwake() { }

        //
        public abstract void PlayForward();
        public abstract void PlayReverse();
        public abstract void Play(bool forward = true);
        public abstract void Stop(bool complete = false);
        
        //
        [FoldoutGroup("Button")]
        
        [FoldoutGroup("Button/Rect To Value Tween")]
        [HorizontalGroup("Button/Rect To Value Tween/Value To")]
        [Button("Rect To Start", ButtonSizes.Large)]
        public abstract void SetCurrentValueToStart();
        [FoldoutGroup("Button")]
        [HorizontalGroup("Button/Rect To Value Tween/Value To")]
        [Button("Rect To End", ButtonSizes.Large)]
        public abstract void SetCurrentValueToEnd();
        
        [FoldoutGroup("Button/Value Tween To Rect")]
        [HorizontalGroup("Button/Value Tween To Rect/To Value")]
        [Button("Start To Rect", ButtonSizes.Large)]
        public abstract void SetStartToCurrentValue();
        [FoldoutGroup("Button/Value Tween To Rect")]
        [HorizontalGroup("Button/Value Tween To Rect/To Value")]
        [Button("End To Rect", ButtonSizes.Large)]
        public abstract void SetEndToCurrentValue();
        
        [FoldoutGroup("Button")]
        [Button("Set Main Target", ButtonSizes.Large)]
        public void SetMainTarget()
        {
            MainTarget = GetComponent<RectTransform>();
        }

        #endregion
    }
}