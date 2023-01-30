using HAU_VR_Cardboard.Scripts.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerPointerController : MonoBehaviour
    {
        #region Variables
        
        //
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public PlayerOutsidePointer OutPointer { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public PlayerSmoothFade SmoothFade { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public PlayerTextLogging TextLogging { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public GameObject DebugWindow { get; set; }

        //
        private RaycastHit _raycastHit;
        private GameObject _gazeAtObject;
        private float _currentGazerTimer;
        private float _currentDelayTimer;
        private bool _gazerStatus;
        private bool _isInitialized;
        
        #endregion

        #region Functions

        //
        private void Reset()
        {
            if (OutPointer == null) OutPointer = GetComponentInChildren<PlayerOutsidePointer>();
            if (SmoothFade == null) SmoothFade = GetComponentInChildren<PlayerSmoothFade>();
            if (TextLogging == null) TextLogging = GetComponentInChildren<PlayerTextLogging>();
            if (DebugWindow == null) DebugWindow = transform.Find("VR Debug Window").gameObject;
        }

        //
        public void Initialize()
        {
            //
            ResetGazer();
            
            //
            _currentDelayTimer = PlayerConfigValue.Instance.DelayGazeTime;
            _currentGazerTimer = PlayerConfigValue.Instance.MaxGazerTime;

            _isInitialized = true;
        }

        //
        private void ResetGazer()
        {
            OutPointer.SetActiveRenderer(false);
            TextLogging.SetActiveTextMesh(false);
            _currentGazerTimer = PlayerConfigValue.Instance.MaxGazerTime;
            _gazerStatus = false;
        }

        //
        private void Update()
        {
            if (!_isInitialized) return;
            
            if (_currentDelayTimer > 0)
            {
                _currentDelayTimer -= Time.deltaTime;
                return;
            }
            
            if (Physics.Raycast(transform.position, transform.forward, out _raycastHit, PlayerConfigValue.Instance.MaxDistanceRay, PlayerConfigValue.Instance.LayerMaskRay))
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
                        _gazeAtObject = _raycastHit.transform.gameObject;
                        _gazeAtObject.GetComponent<IPointerAction>()?.OnPointerEnter();
                        OutPointer.SetFillAmount(0f);
                        OutPointer.SetActiveRenderer(true);
                        TextLogging.SetTextLogging("");
                        TextLogging.SetActiveTextMesh(true);
                    }
                    
                    //
                    _currentGazerTimer -= Time.deltaTime;
                    TextLogging.SetTextLogging(_gazeAtObject.name + " " + _currentGazerTimer.ToString("0.00"));
                    var percent = (PlayerConfigValue.Instance.MaxGazerTime - _currentGazerTimer) / PlayerConfigValue.Instance.MaxGazerTime;
                    OutPointer.SetFillAmount(percent);
                    
                    //
                    if (_currentGazerTimer <= 0)
                    {
                        // Message pointer exit
                        if (_gazeAtObject != null) _gazeAtObject.GetComponent<IPointerAction>()?.OnPointerExit();
                        
                        TextLogging.SetActiveTextMesh(false);
            
                        // Refer gazer object
                        _gazeAtObject = _raycastHit.transform.gameObject;
                    
                        // Message pointer enter
                        _gazeAtObject.GetComponent<IPointerAction>()?.OnPointerClick();
                        
                        // Reset gazer
                        _currentDelayTimer = PlayerConfigValue.Instance.DelayGazeTime;
                        ResetGazer();
                    }
                }
            }
            else
            {
                // Message pointer exit
                if (_gazeAtObject != null)
                {
                    _gazeAtObject.GetComponent<IPointerAction>()?.OnPointerExit();
                    TextLogging.SetActiveTextMesh(false);
                    _gazeAtObject = null;
                }
                
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
            if (!_isInitialized) return;
            
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * PlayerConfigValue.Instance.MaxDistanceRay);
        }

        #endregion
    }
}