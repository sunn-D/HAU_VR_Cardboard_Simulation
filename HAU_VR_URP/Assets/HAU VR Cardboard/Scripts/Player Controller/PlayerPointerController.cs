using TMPro;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerPointerController : MonoBehaviour
    {
        #region Variables

        //
        public TextMeshPro textMeshPro;
        
        //
        public PlayerConfigValue PlayerConfig { get; set; }
        
        //
        private PlayerOutsidePointer _playerOutsidePointer;
        private RaycastHit _raycastHit;
        private GameObject _gazeAtObject;
        private float _currentGazerTimer;
        private float _currentDelayTimer;
        private bool _gazerStatus;
        
        #endregion

        #region Functions
        
        //
        private void Reset()
        {
            if (_playerOutsidePointer == null) _playerOutsidePointer = GetComponentInChildren<PlayerOutsidePointer>();
        }

        //
        private void Start()
        {
            if (_playerOutsidePointer == null) _playerOutsidePointer = GetComponentInChildren<PlayerOutsidePointer>();
            PlayerConfig = PlayerConfigValue.Instance;
            ResetGazer();
        }

        //
        private void ResetGazer()
        {
            _playerOutsidePointer.gameObject.SetActive(false);
            _currentGazerTimer = PlayerConfig.MaxGazerTime;
            _gazerStatus = false;
        }

        //
        private void Update()
        {
            if (_currentDelayTimer > 0)
            {
                _currentDelayTimer -= Time.deltaTime;
                return;
            }
            
            if (Physics.Raycast(transform.position, transform.forward, out _raycastHit, PlayerConfig.MaxDistanceRay, PlayerConfig.LayerMaskRay))
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
                        textMeshPro.text = _raycastHit.transform.name;
                        _playerOutsidePointer.SetFillAmount(0f);
                        _playerOutsidePointer.gameObject.SetActive(true);
                    }
                    
                    //
                    _currentGazerTimer -= Time.deltaTime;
                    textMeshPro.text = _currentGazerTimer.ToString();
                    var percent = (PlayerConfig.MaxGazerTime - _currentGazerTimer) / PlayerConfig.MaxGazerTime;
                    _playerOutsidePointer.SetFillAmount(percent);
                    
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
                        _currentDelayTimer = PlayerConfig.DelayGazeTime;
                        ResetGazer();
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
                _currentGazerTimer = PlayerConfig.MaxGazerTime;
                
                //
                if (_gazerStatus)
                {
                    ResetGazer();
                }
            }
        }
        
        // 
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * PlayerConfigValue.Instance.MaxDistanceRay);
        }

        #endregion
    }
}