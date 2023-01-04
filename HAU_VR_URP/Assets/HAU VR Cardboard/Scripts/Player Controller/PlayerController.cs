using HAU_VR_Cardboard.Scripts.Scriptables;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public PlayerPointerController PointerController { get; set; }

        #endregion

        #region Functions

        //
        private void Reset()
        {
            if (PointerController == null) PointerController = GetComponentInChildren<PlayerPointerController>();
        }

        //
        private void Start()
        {
            //
            if (!PlayerConfigValue.Instance.UsingTextLogging) PointerController.TextLogging.gameObject.SetActive(false);
            if (!PlayerConfigValue.Instance.UsingDebugWindow) PointerController.DebugWindow.SetActive(false);
            
            //
            PointerController.Initialize();
            
            //
            SunEventManager.StartListening(EventID.Teleport_OnTeleportPlayer, OnTeleportPlayer);  
            SunEventManager.StartListening(EventID.Screen_FadedIn, OnScreenFadeIn);
            SunEventManager.StartListening(EventID.Screen_FadedOut, OnScreenFadeOut);
        }

        #endregion

        #region Events

        //
        private void OnTeleportPlayer()
        {
            var newPosition = (Vector3)SunEventManager.GetSender(EventID.Teleport_OnTeleportPlayer);
            transform.position = newPosition;  
        }

        //
        private void OnScreenFadeIn()
        {
            PointerController.SmoothFade.FadeIn();
        }

        //
        private void OnScreenFadeOut()
        {
            PointerController.SmoothFade.FadeOut();
        }

        #endregion
    }
}