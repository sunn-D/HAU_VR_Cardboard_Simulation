using System.Collections;
using Manager;
using Player;
using Scriptables;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI_Controller.UI
{
    public class UIFirstLoading : SunBaseUI
    {
        #region Variables
        
        //
        [FoldoutGroup("Variables/Faded")] 
        public SmoothFaded smoothFaded;
        //
        [FoldoutGroup("Variables/Loading bar")]
        public Image loadingProcessBar;
        
        //
        private float _currentProcess;
        private float _targetProcess;
        private bool _loadingScene;
        private AsyncOperation _loadingAsync;
        private AsyncOperation _unloadingAsync;

        #endregion
        
        #region Functions

        //
        public override void Initialize()
        {
            //
            loadingProcessBar.fillAmount = 0f;
            _loadingScene = false;
            
            //
            smoothFaded.OnFinishAction += () =>
            {
                MainThread.Instance.StartCoroutine(UnloadLoadingScene(LoadingConfig.Instance.DelayAfterFadedIn));
            };
            
            //
            if (UsingTweenShow)
            {
                TweenShow.FinishedEventWhen = EventWhen.Forward;
                TweenShow.OnFinishedEvent.AddListener(LoadMenuScene);
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
            Destroy(gameObject);
        }
        
        //
        private void LoadMenuScene()
        {
            _loadingAsync = SceneManager.LoadSceneAsync(LoadingConfig.Instance.MainSceneName, LoadSceneMode.Additive);
            _loadingAsync.allowSceneActivation = false;
            _loadingScene = true;
        }

        private void Update()
        {
            if (_loadingScene)
            {
                _targetProcess = _loadingAsync.progress / .9f;
                _currentProcess = Mathf.MoveTowards(_currentProcess, _targetProcess, Time.deltaTime * LoadingConfig.Instance.ProcessAnimationMultiplier);
                loadingProcessBar.fillAmount = _currentProcess;

                if (Mathf.Approximately(_currentProcess , 1f))
                {
                    smoothFaded.FadeOut();
                    _loadingScene = false;
                }
            }
        }

        //
        private IEnumerator UnloadLoadingScene(float delayTime)
        {
            _loadingAsync.allowSceneActivation = true;

            yield return new WaitForSeconds(.1f);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadingConfig.Instance.MainSceneName));
            _unloadingAsync = SceneManager.UnloadSceneAsync(LoadingConfig.Instance.LoadingSceneName);

            while (!_unloadingAsync.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            
            yield return new WaitForSeconds(delayTime);
            SunEventManager.EmitEvent(EventID.ScreenFadedIn);
            Hide();
        }

        #endregion
    }
}