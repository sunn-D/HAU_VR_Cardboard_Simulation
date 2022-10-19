using DunnGSunn;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerCameraPointer : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Default configs")]
        public float maxDistanceRay = 10f;
        [FoldoutGroup("Variables/Default configs")]
        public float maxGazerTime = 1f;
        [FoldoutGroup("Variables/Default configs")]
        public LayerMask layerMaskRay;

        //
        private RaycastHit _raycastHit;
        private GameObject _gazeAtObject;
        private float _currentGazerTimer;
        private bool _gazerStatus;
        
        #endregion

        #region Functions
        
        //
        private void Start()
        {
            ResetGazer();
        }

        //
        private void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out _raycastHit, maxDistanceRay, layerMaskRay))
            {
                if (_gazeAtObject != _raycastHit.transform.gameObject)
                {
                    _gazeAtObject = _raycastHit.transform.gameObject;
                }
                else
                {
                    if (!_gazerStatus)
                    {
                        _gazerStatus = true;
                        _raycastHit.transform.GetComponent<IPointerAction>()?.OnPointerEnter();
                        SunEventManager.EmitEvent(EventID.OnGazerTimerStart);
                    }
                    
                    //
                    _currentGazerTimer -= Time.deltaTime;
                    var process = (maxGazerTime - _currentGazerTimer) / maxGazerTime;
                    SunEventManager.EmitEvent(EventID.OnGazerTimerProcess, sender: process);
                    
                    //
                    if (_currentGazerTimer <= 0)
                    {
                        // Message pointer exit
                        if (_gazeAtObject != null)
                        {
                            var currentIPointer = _gazeAtObject.GetComponent<IPointerAction>();
                            currentIPointer?.OnPointerExit();
                        }

                        // Refer gazer object
                        _gazeAtObject = _raycastHit.transform.gameObject;
                    
                        // Message pointer enter
                        var newIPointer = _gazeAtObject.GetComponent<IPointerAction>();
                        newIPointer?.OnPointerClick();
                        
                        // Reset gazer
                        ResetGazer();
                        SunEventManager.EmitEvent(EventID.OnGazerTimerFinish);
                    }
                }
            }
            else
            {
                // Message pointer exit
                if (_gazeAtObject != null)
                {
                    var currentIPointer = _gazeAtObject.GetComponent<IPointerAction>();
                    currentIPointer?.OnPointerExit();
                }
                
                // Reset object and gazer timer
                _gazeAtObject = null;
                _currentGazerTimer = maxGazerTime;
                
                //
                if (_gazerStatus)
                {
                    _gazerStatus = false;
                    SunEventManager.EmitEvent(EventID.OnGazerTimerFinish);
                }
            }
        }

        //
        private void ResetGazer()
        {
            _currentGazerTimer = maxGazerTime;
            _gazerStatus = false;
        }
        
        // Draw line in editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxDistanceRay);
        }

        #endregion
    }
}