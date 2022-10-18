using UnityEngine;

namespace VR
{
    public class VREditorEmulator : MonoBehaviour
    {
#if UNITY_EDITOR

        #region Variables

        //
        private const string AXIS_MOUSE_X = "Mouse X";
        private const string AXIS_MOUSE_Y = "Mouse Y";

        //
        private static readonly Vector3 NECK_OFFSET = new Vector3(0, 0.075f, 0.08f);

        //
        private static Camera[] allCameras = new Camera[32];

        //
        private float _mouseX;
        private float _mouseY;
        private float _mouseZ;

        //
        public Vector3 HeadPosition { get; private set; }
        public Quaternion HeadRotation { get; private set; }

        #endregion

        #region Functions

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
                _mouseX += Input.GetAxis(AXIS_MOUSE_X) * 5;
                if (_mouseX <= -180)
                {
                    _mouseX += 360;
                }
                else if (_mouseX > 180)
                {
                    _mouseX -= 360;
                }

                _mouseY -= Input.GetAxis(AXIS_MOUSE_Y) * 2.4f;
                _mouseY = Mathf.Clamp(_mouseY, -85, 85);
            }
            else if (CanChangeRoll())
            {
                rolled = true;
                _mouseZ += Input.GetAxis(AXIS_MOUSE_X) * 5;
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
        
        //
        private void Start()
        {
            UpdateAllCameras();
        }

        //
        private void Update()
        {
            UpdateEditorEmulation();
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
            HeadPosition = (HeadRotation * NECK_OFFSET) - (NECK_OFFSET.y * Vector3.up);
        }

        //
        private void ApplyHeadOrientationToVRCameras()
        {
            UpdateAllCameras();

            for (var i = 0; i < Camera.allCamerasCount; ++i)
            {
                var cam = allCameras[i];

                if (cam && cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    cam.transform.localPosition = HeadPosition * cam.transform.lossyScale.y;
                    cam.transform.localRotation = HeadRotation;
                }
            }
        }

        //
        private void UpdateAllCameras()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            if (Camera.allCamerasCount > allCameras.Length)
            {
                var newAllCamerasSize = Camera.allCamerasCount;
                while (Camera.allCamerasCount > newAllCamerasSize)
                {
                    newAllCamerasSize *= 2;
                }

                allCameras = new Camera[newAllCamerasSize];
            }

            Camera.GetAllCameras(allCameras);
        }
        
        #endregion
        
#endif
    }
}