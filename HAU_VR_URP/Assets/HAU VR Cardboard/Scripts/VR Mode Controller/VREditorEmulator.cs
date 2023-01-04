using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.VR_Mode_Controller
{
    public class VREditorEmulator : SunMonoSingleton<VREditorEmulator>
    {
        #region Variables

        //
        private const string AxisMouseX = "Mouse X";
        private const string AxisMouseY = "Mouse Y";

        //
        private static readonly Vector3 NeckOffset = new Vector3(0, 0.075f, 0.08f);

        //
        private static Camera[] _allCameras = new Camera[32];

        //
        private float _mouseX;
        private float _mouseY;
        private float _mouseZ;

        //
        public Vector3 HeadPosition { get; set; }
        public Quaternion HeadRotation { get; set; }

        #endregion

        #region Functions

        //
        protected override void LoadInAwake()
        {
            if (!Application.isEditor)
            {
                DestroyImmediate(gameObject);
            }
        }
        
        //
        protected override void LoadInStart()
        {
            UpdateAllCameras();
        }

        //
        private void Update()
        {
            UpdateEditorEmulation();
        }
        
        //
        public void Recenter()
        {
            _mouseX = _mouseZ = 0;
            UpdateHeadPositionAndRotation();
            ApplyHeadOrientationToVRCameras();
        }

        //
        public void UpdateEditorEmulation()
        {
            var rolled = false;
            if (CanChangeYawPitch())
            {
                _mouseX += Input.GetAxis(AxisMouseX) * 5;
                if (_mouseX <= -180)
                {
                    _mouseX += 360;
                }
                else if (_mouseX > 180)
                {
                    _mouseX -= 360;
                }

                _mouseY -= Input.GetAxis(AxisMouseY) * 2.4f;
                _mouseY = Mathf.Clamp(_mouseY, -85, 85);
            }
            else if (CanChangeRoll())
            {
                rolled = true;
                _mouseZ += Input.GetAxis(AxisMouseX) * 5;
                _mouseZ = Mathf.Clamp(_mouseZ, -85, 85);
            }

            if (!rolled)
            {
                _mouseZ = Mathf.Lerp(_mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
            }

            UpdateHeadPositionAndRotation();
            ApplyHeadOrientationToVRCameras();

            if (CanResetRotation())
            {
                Recenter();
            }

            if (CanControlCursorLockMode())
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        
        // Input
        private bool CanChangeYawPitch()
        {
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        }
        
        // Input
        private bool CanChangeRoll()
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }
        
        // Input
        private bool CanResetRotation()
        {
            return Input.GetMouseButtonDown(0);
        }
        
        // Input
        private bool CanControlCursorLockMode()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        //
        private void UpdateHeadPositionAndRotation()
        {
            HeadRotation = Quaternion.Euler(_mouseY, _mouseX, _mouseZ);
            HeadPosition = (HeadRotation * NeckOffset) - (NeckOffset.y * Vector3.up);
        }

        //
        private void ApplyHeadOrientationToVRCameras()
        {
            UpdateAllCameras();

            for (var i = 0; i < Camera.allCamerasCount; ++i)
            {
                var cam = _allCameras[i];
                var camTransform = cam.transform;
                
                if (cam && cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    camTransform.localPosition = HeadPosition * camTransform.lossyScale.y;
                    camTransform.localRotation = HeadRotation;
                }
            }
        }

        //
        private void UpdateAllCameras()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            if (Camera.allCamerasCount > _allCameras.Length)
            {
                var newAllCamerasSize = Camera.allCamerasCount;
                while (Camera.allCamerasCount > newAllCamerasSize)
                {
                    newAllCamerasSize *= 2;
                }

                _allCameras = new Camera[newAllCamerasSize];
            }

            Camera.GetAllCameras(_allCameras);
        }
        
        #endregion   
    }
}