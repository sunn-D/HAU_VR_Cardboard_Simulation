using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public class SunAssetBundleDownloader : MonoBehaviour
    {
        #region Variables

        //
        public bool IsDownloading { get; set; }
        public UnityWebRequest WWW { get; set; }
        public Action<float> OnDownloading { get; set; }
        public Action<bool> OnSaved { get; set; }

        //
        private string _assetID;
        private string _url;
        private float _lagTimeOut;
        private IEnumerator _downloadCoroutine;

        #endregion
        
        //
        public void StartDownload()
        {
            if (_downloadCoroutine != null) StopCoroutine(_downloadCoroutine);
            IsDownloading = true;
            _downloadCoroutine = SunFuncAssetBundle.Download(_url,
                webRequest => WWW = webRequest,
                process => OnDownloading?.Invoke(process),
                success =>
                {
                    if (WWW == null || WWW.error != null)
                    {
                        OnSaved.Invoke(false);
                        CancelDownload();
                    }
                    else
                    {
                        var saveSuccess = SunFuncAssetBundle.SaveAssetBundle(WWW, _assetID);
                        if (saveSuccess)
                        {
                            System.IO.Path.ChangeExtension(SunFuncAssetBundle.AssetBundleDataPath + _assetID + ".txt", null);
                            OnSaved?.Invoke(true);
                        }
                        else
                        {
                            SunFuncAssetBundle.Delete(_assetID);
                            OnSaved?.Invoke(false);
                        }
                        CancelDownload();
                    }
                }, _lagTimeOut);
            StartCoroutine(_downloadCoroutine);
        }
        
        //
        public void CancelDownload()
        {
            IsDownloading = false;
            WWW?.Dispose();
            StopAllCoroutines();
            Destroy(gameObject);
        }
        
        //
        public static SunAssetBundleDownloader DownloadAsset(string url, string assetID, float lagTimeout = 3f)
        {
            var controller = new GameObject(assetID + " downloading...").AddComponent<SunAssetBundleDownloader>();
            controller._assetID = assetID;
            controller._url = url;
            controller._lagTimeOut = lagTimeout;
            DontDestroyOnLoad(controller.gameObject);
            controller.StartDownload();
            return controller;
        }
    }
}