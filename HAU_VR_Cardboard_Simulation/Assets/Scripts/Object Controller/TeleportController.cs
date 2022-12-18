using System.Collections.Generic;
using Sun_Package;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Object_Controller
{
    public class TeleportController : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Teleport objects")]
        public List<Teleport> listTeleport;
        
        //
        private int _currentTeleportID = -1;

        #endregion

        #region Functions

        //
        private void Start()
        {
            SunEventManager.StartListening(EventID.OnTeleportPointerClick, OnTeleportPointerClick);
        }

        //
        private void OnTeleportPointerClick()
        {
            if (_currentTeleportID != -1)
            {
                listTeleport.Find(o => o.teleportID == _currentTeleportID).EnableTeleport();
            }

            var newTeleportID = (int) SunEventManager.GetSender(EventID.OnTeleportPointerClick);
            listTeleport.Find(o => o.teleportID == newTeleportID).DisableTeleport();
            _currentTeleportID = newTeleportID;
        }

        #endregion
    }
}