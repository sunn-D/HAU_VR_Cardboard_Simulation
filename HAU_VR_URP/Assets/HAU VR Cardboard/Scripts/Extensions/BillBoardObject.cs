using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Extensions
{
    public class BillBoardObject : MonoBehaviour
    {
        #region Variables

        //
        public enum BillboardTypeEnum { LookAtCamera, CameraForward };
        
        //
        [FoldoutGroup("Variables")]
        [SerializeField] private BillboardTypeEnum billboardType;
        [FoldoutGroup("Variables")]
        [SerializeField] private bool lockX;
        [FoldoutGroup("Variables")]
        [SerializeField] private bool lockY;
        [FoldoutGroup("Variables")]
        [SerializeField] private bool lockZ;
        
        //
        public BillboardTypeEnum BillboardType
        {
            get => billboardType;
            set => billboardType = value;
        }
        public bool LockX
        {
            get => lockX;
            set => lockX = value;
        }
        public bool LockY
        {
            get => lockY;
            set => lockY = value;
        }
        public bool LockZ
        {
            get => lockZ;
            set => lockZ = value;
        }

        //
        private Vector3 _originalRotation;
        private Camera _cameraMain;

        #endregion

        #region Functions

        //
        private void Awake() 
        {
            _originalRotation = transform.rotation.eulerAngles;
            _cameraMain = Camera.main;
        }
        
        //
        private void LateUpdate()
        {
            switch (BillboardType) {
                case BillboardTypeEnum.LookAtCamera:
                    transform.LookAt(_cameraMain.transform.position, Vector3.up);
                    break;
                case BillboardTypeEnum.CameraForward:
                    transform.forward = _cameraMain.transform.forward;
                    break;
            }

            var rotation = transform.rotation.eulerAngles;
            if (lockX) { rotation.x = _originalRotation.x; }
            if (lockY) { rotation.y = _originalRotation.y; }
            if (lockZ) { rotation.z = _originalRotation.z; }
            transform.rotation = Quaternion.Euler(rotation);
        }

        #endregion
    }
}