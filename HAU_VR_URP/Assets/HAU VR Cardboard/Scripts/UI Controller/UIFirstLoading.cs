using System.Collections;
using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Player_Controller;
using HAU_VR_Cardboard.Scripts.Scriptables;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class UIFirstLoading : SunBaseUI
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public PlayerSmoothFade SmoothFade { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public Image LoadingProcess { get; set; }

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
            DontDestroyOnLoad(gameObject);
            
            //
            LoadingProcess.fillAmount = 0f;
            _loadingScene = false;
            
            //
            SmoothFade.OnFinishAction += () =>
            {
                SettingManager.Instance.StartCoroutine(UnloadLoadingScene(LoadingConfigValue.Instance.DelayAfterFadedIn));
            };
            
            //
            if (UsingTweenShow)
            {
                TweenShow.EventFinishWhen = EventWhen.Forward;
                TweenShow.OnFinishEvent.AddListener(LoadMenuScene);
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
            _loadingAsync = SceneManager.LoadSceneAsync(LoadingConfigValue.Instance.MainSceneName, LoadSceneMode.Additive);
            _loadingAsync.allowSceneActivation = false;
            _loadingScene = true;
        }

        //
        private void Update()
        {
            if (_loadingScene)
            {
                _targetProcess = _loadingAsync.progress / .9f;
                _currentProcess = Mathf.MoveTowards(_currentProcess, _targetProcess, Time.deltaTime * LoadingConfigValue.Instance.ProcessAnimationMultiplier);
                LoadingProcess.fillAmount = _currentProcess;

                if (Mathf.Approximately(_currentProcess , 1f))
                {
                    SmoothFade.FadeOut();
                    _loadingScene = false;
                }
            }
        }
        
        //
        private IEnumerator UnloadLoadingScene(float delayTime)
        {
            _loadingAsync.allowSceneActivation = true;

            yield return new WaitForSeconds(.1f);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadingConfigValue.Instance.MainSceneName));
            _unloadingAsync = SceneManager.UnloadSceneAsync(LoadingConfigValue.Instance.LoadingSceneName);

            while (!_unloadingAsync.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            
            yield return new WaitForSeconds(delayTime);
            SunEventManager.EmitEvent(EventID.Screen_FadedIn);
            Hide();
        }

        #endregion
    }
}