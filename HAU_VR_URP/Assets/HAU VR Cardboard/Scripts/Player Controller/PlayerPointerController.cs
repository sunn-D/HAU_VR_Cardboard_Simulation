using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerPointerController : MonoBehaviour
    {
        #region Variables
        
        //
        [FoldoutGroup("Variables")] 
        [SerializeField] private PlayerOutsidePointer outPointer;
        [FoldoutGroup("Variables")]
        [SerializeField] private PlayerSmoothFade smoothFade;
        [FoldoutGroup("Variables")] 
        [SerializeField] private PlayerTextLogging textLogging;
        
        //
        public PlayerOutsidePointer OutPointer
        {
            get => outPointer;
            set => outPointer = value;
        }
        public PlayerSmoothFade SmoothFade
        {
            get => smoothFade;
            set => smoothFade = value;
        }
        public PlayerTextLogging TextLogging
        {
            get => textLogging;
            set => textLogging = value;
        }

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
            //
            if (OutPointer == null) OutPointer = GetComponentInChildren<PlayerOutsidePointer>();
            if (SmoothFade == null) SmoothFade = GetComponentInChildren<PlayerSmoothFade>();
            if (TextLogging == null) TextLogging = GetComponentInChildren<PlayerTextLogging>();
            
            //
            ResetGazer();
            
            //
            _currentDelayTimer = PlayerConfigValue.Instance.DelayGazeTime;
            _currentGazerTimer = PlayerConfigValue.Instance.MaxGazerTime;
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
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * PlayerConfigValue.Instance.MaxDistanceRay);
        }

        #endregion
    }
}