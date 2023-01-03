using System.Collections;
using Google.XR.Cardboard;
using Sun_Package;
using UnityEngine;
using UnityEngine.XR.Management;

namespace HAU_VR_Cardboard.Scripts.VR_Mode_Controller
{
    public class VRStartup : SunMonoSingleton<VRStartup>
    {
        #region Variables

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
        
        //
        protected override void LoadInAwake()
        {
            DefaultVariables();
            _mainCamera = Camera.main;
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
        }
        
        //
        private void DefaultVariables()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }
    }
}