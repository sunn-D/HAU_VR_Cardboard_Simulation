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
        // private const string MainScene = "SampleScene";

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
        public static IEnumerator LoadMainScene(Action onStartLoading = null, Action<float> onLoading = null, Action onFinishLoading = null, float timeDelayActive = .5f)
        {
            onStartLoading?.Invoke();
            var async = LoadSceneAsyncAdditive(MainScene);
            async.allowSceneActivation = false;

            while (async.progress < .9f)
            {
                var progress = Mathf.Clamp01(async.progress / .9f);
                onLoading?.Invoke(progress);
                yield return null;
            }

            onFinishLoading?.Invoke();

            yield return new WaitForSeconds(timeDelayActive);
            async.allowSceneActivation = true;
        }
        
        //
        private static AsyncOperation LoadSceneAsyncAdditive(string scenePath)
        {
            return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
        }

        //
        private static AsyncOperation UnloadSceneAsync(string scenePath)
        {
            return SceneManager.UnloadSceneAsync(scenePath);
        }
    }
}