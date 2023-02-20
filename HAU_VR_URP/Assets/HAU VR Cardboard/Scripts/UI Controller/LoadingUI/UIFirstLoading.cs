using HAU_VR_Cardboard.Scripts.Manager;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;
using UnityEngine.UI;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.LoadingUI
{
    public class UIFirstLoading : SunBaseUI
    {
        #region Variables
        
        [field: FoldoutGroup("Variables"), SerializeField] public Image LoadingProcess { get; set; }

        #endregion

        #region Functions

        //        
        public override void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            LoadingProcess.fillAmount = 0f;
            if (UsingTweenShow)
            {
                TweenShow.EventFinishWhen = EventWhen.Forward;
                TweenShow.OnFinishEvent.AddListener(() =>
                {
                    SceneLoader.LoadMainScene(null, 
                        process =>
                        {
                            LoadingProcess.fillAmount = process;
                        },
                        () =>
                        {
                            SceneLoader.UnloadLoadingScene();
                            Hide();
                        });
                });
            }
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

        #endregion
    }
}