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
            PointerController.Initialize();
            
            //
            SunEventManager.StartListening(SunEventID.Teleport_TeleportPlayer, OnTeleportPlayer);  
            SunEventManager.StartListening(SunEventID.Screen_FadedIn, OnScreenFadeIn);
            SunEventManager.StartListening(SunEventID.Screen_FadedOut, OnScreenFadeOut);
        }

        #endregion

        #region Events

        //
        private void OnTeleportPlayer(object sender)
        {
            var newPosition = (Vector3)sender;
            transform.position = newPosition;  
        }

        //
        private void OnScreenFadeIn(object sender)
        {
            PointerController.SmoothFade.FadeIn();
        }

        //
        private void OnScreenFadeOut(object sender)
        {
            PointerController.SmoothFade.FadeOut();
        }

        #endregion
    }
}