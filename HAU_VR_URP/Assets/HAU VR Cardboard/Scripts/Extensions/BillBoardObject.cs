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
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public BillboardTypeEnum BillboardType { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool LockX { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool LockY { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool LockZ { get; set; }

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
            //
            switch (BillboardType) {
                case BillboardTypeEnum.LookAtCamera:
                    transform.LookAt(_cameraMain.transform.position, Vector3.up);
                    break;
                case BillboardTypeEnum.CameraForward:
                    transform.forward = _cameraMain.transform.forward;
                    break;
            }

            //
            var rotation = transform.rotation.eulerAngles;
            if (LockX) { rotation.x = _originalRotation.x; }
            if (LockY) { rotation.y = _originalRotation.y; }
            if (LockZ) { rotation.z = _originalRotation.z; }
            transform.rotation = Quaternion.Euler(rotation);
        }

        #endregion
    }
}