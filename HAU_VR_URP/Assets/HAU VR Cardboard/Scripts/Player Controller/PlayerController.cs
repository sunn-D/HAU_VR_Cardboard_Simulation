using System;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        

        #endregion

        #region Functions

        //
        private void Start()
        {
            SunEventManager.StartListening(EventID.Teleport_OnTeleportPlayer, OnTeleportPlayer);  
        }

        //
        private void OnTeleportPlayer()
        {
            var newPosition = (Vector3)SunEventManager.GetSender(EventID.Teleport_OnTeleportPlayer);
            transform.position = newPosition;  
        }

        #endregion
    }
}