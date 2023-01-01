using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public abstract class SunBaseUI : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Touch config")]
        [SerializeField] private float delayTimeClick = .5f;
        //
        [FoldoutGroup("Variables/UI config")] 
        [SerializeField] private bool isDebug;
        [FoldoutGroup("Variables/UI config")] 
        [SerializeField] private bool initOnAwake;
        [FoldoutGroup("Variables/UI config")] 
        [SerializeField] private bool usingSafeArea;
        [FoldoutGroup("Variables/UI config"), ShowIf(nameof(usingSafeArea))]
        [SerializeField] private RectTransform panel;
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
        public bool IsShow { get; protected set; }
        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > _lastTimeClick + delayTimeClick;
                if (result) _lastTimeClick = Time.unscaledTime;
                return result;
            }
        }
        
        //
        private float _lastTimeClick;
        private Rect _lastSafeArea = new Rect(0f, 0f, 0f, 0f);
        private Vector2Int _lastScreenSize = new Vector2Int(0, 0);

        #endregion

        #region Functions

        // Unity callback function: Awake
        private void Awake()
        {
            if (initOnAwake) Initialize();
            IsShow = false;
            if (panel == null) panel = GetComponent<RectTransform>();
            if (usingSafeArea) Refresh();
        }
        
        //
        private void Update()
        {
            if (usingSafeArea) Refresh();
        }
        
        //
        private void Refresh()
        {
            var safeArea = Screen.safeArea;
            if (safeArea != _lastSafeArea || Screen.width != _lastScreenSize.x || Screen.height != _lastScreenSize.y)
            {
                _lastScreenSize.x = Screen.width;
                _lastScreenSize.y = Screen.height;
                ApplySafeArea(safeArea);
            }
        }
        
        //
        private void ApplySafeArea(Rect newRect)
        {
            _lastSafeArea = newRect;

            if (Screen.width > 0 && Screen.height > 0)
            {
                var anchorMin = newRect.position;
                var anchorMax = newRect.position + newRect.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    panel.anchorMin = anchorMin;
                    panel.anchorMax = anchorMax;
                }
            }
        }

        #endregion

        #region Override functions

        // Abstract functions
        public abstract void Initialize();
        public abstract void Show();
        public abstract void Hide();
        
        //
        public virtual void CheckShowTemplate()
        {
            if (IsShow) return;
            if (isDebug) Debug.Log("Show " + gameObject.name);
        }
        
        // 
        public virtual void ShowTemplate()
        {
            _lastTimeClick = Time.unscaledTime;
            
            if (!usingTweenShow) gameObject.SetActive(true);
            else tweenShow.PlayForward();

            IsShow = true;
        }
        
        //
        public virtual void CheckHideTemplate()
        {
            if (!IsShow) return;
            if (isDebug) Debug.Log("Hide " + gameObject.name);
        }
        
        //
        public virtual void HideTemplate()
        {
            _lastTimeClick = Time.unscaledTime;
            
            if (!usingTweenHide) gameObject.SetActive(false);
            else tweenHide.PlayReverse();

            IsShow = false;
        }

        #endregion
    }
}