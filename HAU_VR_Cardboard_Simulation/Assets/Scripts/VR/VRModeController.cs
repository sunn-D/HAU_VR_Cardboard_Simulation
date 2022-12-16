using System.Collections;
using Google.XR.Cardboard;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR.Management;

namespace VR
{
    public class VRModeController : MonoBehaviour
    {
        #region Variables
        
        //
        [FoldoutGroup("Variables")]
        public float defaultFieldOfView = 60f;
        
        //
        private Camera _mainCamera;
        private Coroutine _startXR;
        
        //
        public static bool IsScreenTouched
        {
            get
            {
                return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            }
        }

        //
        public static bool IsVRModeEnabled
        {
            get
            {
                return XRGeneralSettings.Instance.Manager.isInitializationComplete;
            }
        }

        #endregion

        #region Functions

        //
        private void Start()
        {
            //
            _mainCamera = Camera.main;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        
        //
        private void Update()
        {
            if (IsVRModeEnabled)
            {
                //
                if (Api.IsCloseButtonPressed) ExitVRMode();

                //
                Api.UpdateScreenParams();
            }
            else
            {
                //
                if (IsScreenTouched) EnterVRMode();
            }
        }
        
        //
        private void EnterVRMode()
        {
            StartXR();
            if (Api.HasNewDeviceParams()) Api.ReloadDeviceParams();
        }
        
        //
        private void ExitVRMode()
        {
            StopXR();
        }
        
        //
        private void StartXR()
        {
            if (_startXR != null) StopCoroutine(_startXR);
            _startXR = StartCoroutine(nameof(StartXRCoroutine));
        }

        //
        private IEnumerator StartXRCoroutine()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }
        
        //
        private void StopXR()
        {
            //
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            
            //
            _mainCamera.ResetAspect();
            _mainCamera.fieldOfView = defaultFieldOfView;
        }

        #endregion
    }
}