using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public abstract class SunBaseUI : MonoBehaviour
    {
        #region Variables

        [field: FoldoutGroup("Variables")]
        //
        [field: FoldoutGroup("Variables/Touch config"), SerializeField] public float DelayTimeClick { get; set; } = .5f;
        //
        [field: FoldoutGroup("Variables/UI config"), SerializeField] public bool IsDebug { get; set; }
        [field: FoldoutGroup("Variables/UI config"), SerializeField] public bool InitOnAwake { get; set; }
        [field: FoldoutGroup("Variables/UI config"), SerializeField] public bool UsingSafeArea { get; set; }
        [field: FoldoutGroup("Variables/UI config"), SerializeField, ShowIf(nameof(UsingSafeArea))] public RectTransform Panel { get; set; }
        //
        [field: FoldoutGroup("Variables/Show - Hide"), SerializeField] public bool UsingTweenShow { get; set; }
        [field: FoldoutGroup("Variables/Show - Hide"), SerializeField] public bool UsingTweenHide { get; set; }
        [field: FoldoutGroup("Variables/Show - Hide"), SerializeField, ShowIf("UsingTweenShow")] public SunTweenControl TweenShow { get; set; }
        [field: FoldoutGroup("Variables/Show - Hide"), SerializeField, ShowIf("UsingTweenHide")] public SunTweenControl TweenHide { get; set; }
        
        //
        public bool IsShow { get; protected set; }
        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > _lastTimeClick + DelayTimeClick;
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
            if (InitOnAwake) Initialize();
            IsShow = false;
            if (Panel == null) Panel = GetComponent<RectTransform>();
            if (UsingSafeArea) Refresh();
        }
        
        //
        private void Update()
        {
            if (UsingSafeArea) Refresh();
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
                    Panel.anchorMin = anchorMin;
                    Panel.anchorMax = anchorMax;
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
            if (IsDebug) Debug.Log("Show " + gameObject.name);
        }
        
        // 
        public virtual void ShowTemplate()
        {
            _lastTimeClick = Time.unscaledTime;
            
            if (!UsingTweenShow) gameObject.SetActive(true);
            else TweenShow.PlayForward();

            IsShow = true;
        }
        
        //
        public virtual void CheckHideTemplate()
        {
            if (!IsShow) return;
            if (IsDebug) Debug.Log("Hide " + gameObject.name);
        }
        
        //
        public virtual void HideTemplate()
        {
            _lastTimeClick = Time.unscaledTime;
            
            if (!UsingTweenHide) gameObject.SetActive(false);
            else TweenHide.PlayReverse();

            IsShow = false;
        }

        #endregion
    }
}