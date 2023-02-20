using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    public class SceneLoader : SunMonoSingleton<SceneLoader>
    {
        //
        [field: FoldoutGroup("Variables"), SerializeField] public string LoadingScene { get; } = "Loading Scene";
        [field: FoldoutGroup("Variables"), SerializeField] public string MainScene { get; } = "Main Scene";
        
        //
        public static void MoveObjectToScene(GameObject go, string nameScene)
        {
            SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName(nameScene));
        }

        //
        public static void LoadMainScene(Action onStartLoading = null, Action<float> onLoading = null, Action onFinishLoading = null)
        {
            Instance.StartCoroutine(CoroutineLoadScene(Instance.MainScene, onStartLoading, onLoading, onFinishLoading));
        }
        
        //
        public static void UnloadLoadingScene(Action onStartUnloading = null, Action<float> onUnloading = null, Action onFinishUnloading = null)
        {
            Instance.StartCoroutine(CoroutineUnloadScene(Instance.LoadingScene, onStartUnloading, onUnloading, onFinishUnloading));
        }

        //
        public static IEnumerator CoroutineLoadScene(string sceneName, Action onStartLoading = null, Action<float> onLoading = null, Action onFinishLoading = null)
        {
            onStartLoading?.Invoke();

            var asyncLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (asyncLoading.progress <= .9f)
            {
                var progress = asyncLoading.progress / .9f;
                onLoading?.Invoke(progress);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(.1f);
            onFinishLoading?.Invoke();
        }
        
        //
        public static IEnumerator CoroutineUnloadScene(string sceneName, Action onStartUnloading = null, Action<float> onUnloading = null, Action onFinishUnloading = null)
        {
            onStartUnloading?.Invoke();

            var asyncUnloading = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.None);

            while (asyncUnloading.progress <= .9f)
            {
                var progress = asyncUnloading.progress / .9f;
                onUnloading?.Invoke(progress);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(.1f);
            onFinishUnloading?.Invoke();
        }
    }
}