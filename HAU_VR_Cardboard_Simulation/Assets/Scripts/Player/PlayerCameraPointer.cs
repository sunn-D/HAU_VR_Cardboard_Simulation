using Scriptables;
using Sun_Package;
using UnityEngine;

namespace Player
{
    public class PlayerCameraPointer : MonoBehaviour
    {
        #region Variables
        
        //
        private RaycastHit _raycastHit;
        private GameObject _gazeAtObject;
        private float _currentGazerTimer;
        private float _currentDelayTimer;
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
            if (_currentDelayTimer > 0)
            {
                _currentDelayTimer -= Time.deltaTime;
                return;
            }
            
            if (Physics.Raycast(transform.position, transform.forward, out _raycastHit, PlayerConfig.Instance.MaxDistanceRay, PlayerConfig.Instance.LayerMaskRay))
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
                    var process = (PlayerConfig.Instance.MaxGazerTime - _currentGazerTimer) / PlayerConfig.Instance.MaxGazerTime;
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
                        _currentDelayTimer = PlayerConfig.Instance.DelayGazeTime;
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
                _currentGazerTimer = PlayerConfig.Instance.MaxGazerTime;
                
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
            _currentGazerTimer = PlayerConfig.Instance.MaxGazerTime;
            _gazerStatus = false;
        }
        
        // 
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * PlayerConfig.Instance.MaxDistanceRay);
        }

        #endregion
    }
}