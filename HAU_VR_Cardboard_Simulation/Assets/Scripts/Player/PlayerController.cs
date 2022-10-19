using DunnGSunn;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Pointer")]
        public Image pointProcessImage;
        //
        [FoldoutGroup("Variables/Movement configs")]
        public float toggleAngle = 30f;
        [FoldoutGroup("Variables/Movement configs")]
        public float speed = 3f;
        [FoldoutGroup("Variables/Movement configs")]
        public Transform cameraPlayer;

        //
        private CharacterController _characterController;
        private bool moveForward;
        
        #endregion

        #region Functions

        //
        private void Start()
        {
            Initialize();
            ListeningEvents();
        }
        
        //
        private void Initialize()
        {
            //
            pointProcessImage.fillAmount = 0f;
            pointProcessImage.gameObject.SetActive(false);
            
            //
            _characterController = GetComponent<CharacterController>();
        }
        
        //
        private void ListeningEvents()
        {
            //
            SunEventManager.StartListening(EventID.OnGazerTimerStart, OnGazerTimerStart);
            SunEventManager.StartListening(EventID.OnGazerTimerProcess, OnGazerTimerProcess);
            SunEventManager.StartListening(EventID.OnGazerTimerFinish, OnGazerTimerFinish);
            
            //
            SunEventManager.StartListening(EventID.OnTeleport, OnTeleport);
        }
        
        //
        private void Update()
        {
            if (cameraPlayer.eulerAngles.x >= toggleAngle && cameraPlayer.eulerAngles.x < 90f)
            {
                moveForward = true;
            }
            else
            {
                moveForward = false;
            }
            
            if (moveForward)
            {
                var forward = cameraPlayer.TransformDirection(Vector3.forward);
                _characterController.SimpleMove(forward * speed);
            }
        }

        #endregion

        #region Events

        //
        private void OnGazerTimerStart()
        {
            pointProcessImage.fillAmount = 0f;
            pointProcessImage.gameObject.SetActive(true);
        }

        //
        private void OnGazerTimerProcess()
        {
            var process = (float) SunEventManager.GetSender(EventID.OnGazerTimerProcess);
            pointProcessImage.fillAmount = process;
        }

        //
        private void OnGazerTimerFinish()
        { 
            pointProcessImage.gameObject.SetActive(false);
        }

        //
        private void OnTeleport()
        {
            var newPosition = (Vector3) SunEventManager.GetSender(EventID.OnTeleport);
            transform.position = newPosition;
        }

        #endregion
    }
}