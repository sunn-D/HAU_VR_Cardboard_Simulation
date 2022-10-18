using Sirenix.OdinInspector;
using UnityEngine;

namespace DunnGSunn
{
    public abstract class SunBaseUI : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Touch")]
        public float lastTimeClick;
        [FoldoutGroup("Variables/Touch")] 
        public float delayTimeClick = .5f;
        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > lastTimeClick + delayTimeClick;
                if (result) lastTimeClick = Time.unscaledTime;
                return result;
            }
        }
        //
        [FoldoutGroup("Variables/UI config")] 
        public bool isDebug;
        [FoldoutGroup("Variables/UI config")] 
        public bool initOnAwake;
        //
        [FoldoutGroup("Variables/Show - Hide")]
        public bool usingTweenShow;
        [FoldoutGroup("Variables/Show - Hide")]
        public bool usingTweenHide;
        [FoldoutGroup("Variables/Show - Hide"), ShowIf("usingTweenShow")]
        public SunTweenControl tweenShow;
        [FoldoutGroup("Variables/Show - Hide"), ShowIf("usingTweenHide")]
        public SunTweenControl tweenHide;
        //
        public bool IsShow { get; protected set; }

        #endregion

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
    }
}