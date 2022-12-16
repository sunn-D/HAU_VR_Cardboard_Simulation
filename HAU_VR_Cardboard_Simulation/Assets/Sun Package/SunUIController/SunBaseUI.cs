using Sirenix.OdinInspector;
using UnityEngine;

namespace Sun_Package
{
    public abstract class SunBaseUI : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Touch")]
        [SerializeField] private float lastTimeClick;
        [FoldoutGroup("Variables/Touch")] 
        [SerializeField] private float delayTimeClick = .5f;
        //
        [FoldoutGroup("Variables/UI config")] 
        [SerializeField] private bool isDebug;
        [FoldoutGroup("Variables/UI config")] 
        [SerializeField] private bool initOnAwake;
        //
        [FoldoutGroup("Variables/Show - Hide")]
        [SerializeField] private bool usingTweenShow;
        [FoldoutGroup("Variables/Show - Hide")]
        [SerializeField] private bool usingTweenHide;
        [FoldoutGroup("Variables/Show - Hide"), ShowIf("usingTweenShow")]
        [SerializeField] private SunTweenControl tweenShow;
        [FoldoutGroup("Variables/Show - Hide"), ShowIf("usingTweenHide")]
        [SerializeField] private SunTweenControl tweenHide;
        
        //
        public bool UsingTweenShow => usingTweenShow;
        public bool UsingTweenHide => usingTweenHide;
        public SunTweenControl TweenShow => tweenShow;
        public SunTweenControl TweenHide => tweenHide;

        //
        public bool IsShow { get; set; }
        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > lastTimeClick + delayTimeClick;
                if (result) lastTimeClick = Time.unscaledTime;
                return result;
            }
        }

        #endregion

        #region Functions

        // Unity callback function: Awake
        private void Awake()
        {
            if (initOnAwake) Initialize();
            IsShow = false;
        }
        
        // Override functions
        public abstract void Initialize();
        public abstract void Show();
        public abstract void Hide();
        
        //
        public void CheckShowTemplate()
        {
            if (IsShow) return;
            if (isDebug) Debug.Log("Show " + gameObject.name);
        }
        
        // 
        public void ShowTemplate()
        {
            lastTimeClick = Time.unscaledTime;
            if (!usingTweenShow)
            {
                gameObject.SetActive(true);
            }
            else
            {
                tweenShow.PlayForward();
            }

            IsShow = true;
        }
        
        //
        public void CheckHideTemplate()
        {
            if (!IsShow) return;
            if (isDebug) Debug.Log("Hide " + gameObject.name);
        }
        
        //
        public void HideTemplate()
        {
            lastTimeClick = Time.unscaledTime;
            if (!usingTweenHide)
            {
                gameObject.SetActive(false);
            }
            else
            {
                tweenHide.PlayReverse();
            }

            IsShow = false;
        }

        #endregion
    }
}