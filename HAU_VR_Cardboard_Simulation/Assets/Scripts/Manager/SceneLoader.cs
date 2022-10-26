using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public static class SceneLoader
    {
        // Scene name
        private const string LoadingScene = "Loading Scene";
        private const string MainScene = "Main Scene";

        //
        public static IEnumerator UnloadFirstLoadingScene(Action onStartUnloading = null, Action<float> onUnloading = null, Action onFinishUnloading = null)
        {
            onStartUnloading?.Invoke();
            var async = UnloadSceneAsync(LoadingScene);

            while (!async.isDone)
            {
                var progress = Mathf.Clamp01(async.progress / .9f);
                onUnloading?.Invoke(progress);
                yield return null;
            }

            onFinishUnloading?.Invoke();
        }
        
        //
        public static IEnumerator LoadMainScene(Action onStartLoading = null, Action<float> onLoading = null, Action onFinishLoading = null)
        {
            onStartLoading?.Invoke();
            var async = LoadSceneAsyncAdditive(MainScene);

            while (!async.isDone)
            {
                var progress = Mathf.Clamp01(async.progress / .9f);
                onLoading?.Invoke(progress);
                yield return null;
            }

            onFinishLoading?.Invoke();
        }
        
        //
        private static AsyncOperation LoadSceneAsyncAdditive(string scenePath)
        {
            return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
        }
        
        //
        private static AsyncOperation LoadSceneAsyncSingle(string scenePath)
        {
            return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        }

        //
        private static AsyncOperation UnloadSceneAsync(string scenePath)
        {
            return SceneManager.UnloadSceneAsync(scenePath);
        }
    }
}