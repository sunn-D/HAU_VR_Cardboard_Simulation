using System.Collections;
using DunnGSunn;
using Manager;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Controller
{
    public class UIFirstLoading : SunBaseUI
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Configs")] 
        public float delayFadedIn = 1f;
        //
        [FoldoutGroup("Variables/Faded")] 
        public SmoothFaded smoothFaded;
        //
        [FoldoutGroup("Variables/Loading bar")]
        public Image loadingProcessBar;

        #endregion
        
        #region Functions

        //
        public override void Initialize()
        {
            //
            loadingProcessBar.fillAmount = 0f;
            smoothFaded.OnFinishAction += UnloadFirstLoadingScene;
            
            //
            if (usingTweenShow)
            {
                tweenShow.finishedEventWhen = EventWhen.Forward;
                tweenShow.onFinished.AddListener(LoadMenuScene);
            }
            
            //
            Show();
        }

        //
        public override void Show()
        {
            ShowTemplate();
        }

        //
        public override void Hide()
        {
            HideTemplate();
        }
        
        //
        private void LoadMenuScene()
        {
            MainCoroutineThread.Instance.StartCoroutine(SceneLoader.LoadMainScene(
                null,
                value =>
                {
                    loadingProcessBar.fillAmount = value;
                }, () =>
                {
                    smoothFaded.FadeOut();
                }));
        }
        
        //
        private void UnloadFirstLoadingScene()
        {
            MainCoroutineThread.Instance.StartCoroutine(SceneLoader.UnloadFirstLoadingScene(
                null, null,
                () =>
                {
                    MainCoroutineThread.Instance.StartCoroutine(DelayEventOnFinishUnloadingCoroutine(delayFadedIn));
                }));
        }
        
        //
        private IEnumerator DelayEventOnFinishUnloadingCoroutine(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SunEventManager.EmitEvent(EventID.ScreenFadedIn);
        }

        #endregion
        
    }
}