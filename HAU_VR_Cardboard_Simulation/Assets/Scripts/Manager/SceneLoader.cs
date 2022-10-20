using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public static class SceneLoader
    {
        // Scene name
        public static string MenuScene = "SampleScene";
        
        //
        public static IEnumerator LoadMainScene(Action onStartLoading = null, Action<float> onLoading = null, Action onFinishLoading = null)
        {
            yield return new WaitForSeconds(15f);
            
            onStartLoading?.Invoke();
            var async = LoadSceneAsyncSingle(MenuScene);

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