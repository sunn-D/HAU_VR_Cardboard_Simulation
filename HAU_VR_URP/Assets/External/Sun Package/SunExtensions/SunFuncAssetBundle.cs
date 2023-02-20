using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunFuncAssetBundle
    {
        //
        private static string _assetBundleDataPath;
        private static string _streamingDataPath;

        //
        public static string AssetBundleDataPath
        {
            get
            {
                if (_assetBundleDataPath == null)
                {
                    //TODO: Change path of asset bundle
                    _assetBundleDataPath = @"" + Application.persistentDataPath + "/Musics/";
                    if (!Directory.Exists(_assetBundleDataPath)) Directory.CreateDirectory(_assetBundleDataPath);
                }

                return _assetBundleDataPath;
            }
        }
        public static string StreamingDataPath
        {
            get
            {
                if (_streamingDataPath == null)
                {
                    _streamingDataPath = @"" + Application.streamingAssetsPath + "/Musics/";
                }

                return _streamingDataPath;
            }
        }
        
        //
        public static IEnumerator LoadAssetBundleAsync(string path, Action<AssetBundleCreateRequest> onStart, Action<float> onProcess, Action<AssetBundle> onFinish)
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(path);
            onStart?.Invoke(bundleLoadRequest);
            while (!bundleLoadRequest.isDone)
            {
                onProcess?.Invoke(bundleLoadRequest.progress);
                yield return null;
            }
            onFinish?.Invoke(bundleLoadRequest.assetBundle);
        }
        
        //
        public static IEnumerator LoadAssetBundleAllAsync(AssetBundle assetBundle, Action<AssetBundleRequest> onStart, Action<float> onProcess, Action<UnityEngine.Object[]> onFinish)
        {
            var assetLoadRequest = assetBundle.LoadAllAssetsAsync();
            onStart?.Invoke(assetLoadRequest);
            while (!assetLoadRequest.isDone)
            {
                onProcess?.Invoke(assetLoadRequest.progress);
                yield return null;
            }
            onFinish?.Invoke(assetLoadRequest.allAssets);
        }

        //
        public static IEnumerator Download(string url, Action<UnityWebRequest> onStart, Action<float> onDownloading, Action<bool> onFinishDownload, float lagTimeout = 0)
        {
            using (var webRequest = UnityWebRequest.Get(url))
            {
                onStart?.Invoke(webRequest);
                var lastProgress = 0f;
                var lastRequestTime = Time.realtimeSinceStartup;
                webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    if (lagTimeout > 0)
                    {
                        var isProgressRunning = Math.Abs(lastProgress - webRequest.downloadProgress) > 0f;
                        if (isProgressRunning)
                        {
                            lastProgress = webRequest.downloadProgress;
                            lastRequestTime = Time.realtimeSinceStartup;
                        }
                        
                        if (Time.realtimeSinceStartup - lastRequestTime >= lagTimeout)
                        {
                            webRequest.Abort();
                            onFinishDownload.Invoke(false);
                            yield break;
                        }

                        onDownloading?.Invoke(webRequest.downloadProgress);
                        yield return null;
                    }
                    else
                    {
                        onDownloading?.Invoke(webRequest.downloadProgress);
                        yield return null;
                    }
                }

                onFinishDownload?.Invoke(webRequest.result == UnityWebRequest.Result.Success);
            }
        }
        
        //
        public static T LoadObject<T>(UnityEngine.Object[] objs) where T : UnityEngine.Object
        {
            foreach (var obj in objs) 
            {
                if (obj as T != null)
                {
                    return obj as T;
                }
            }
            return null;
        }

        //
        public static bool SaveAssetBundle(UnityWebRequest www, string name)
        {
            if (www == null) return false;
            
            bool success;
            try
            {
                var localPath = AssetBundleDataPath + name;
                File.WriteAllBytes(localPath, www.downloadHandler.data);
                success = true;
            }
            catch (Exception) { success = false; }
            return success;
        }
        
        //
        public static bool IsExist(string name)
        {
            var localPath = AssetBundleDataPath + name;
            return File.Exists(localPath);
        }
        
        //
        public static void Delete(string name)
        {
            var localPath = AssetBundleDataPath + name;
            File.Delete(localPath);
        }
    }
}