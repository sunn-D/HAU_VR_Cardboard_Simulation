using System.Collections;
using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Teleport_Controller;
using HAU_VR_Cardboard.Scripts.UI_Controller.UI;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.HomeUI
{
    public class ButtonPlay : BaseButtonController
    {
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            StartCoroutine(nameof(StartPlay));
        }

        private IEnumerator StartPlay()
        {
            SunEventManager.EmitEvent(SunEventID.Screen_FadedOut);
            
            yield return new WaitForSeconds(.5f);
            BuildingController.Instance.ShowNPCCampus();
            TeleportController.Instance.EnableAllTeleport();
            TeleportController.Instance.TeleportToFirstTeleportPoint();

            yield return new WaitForSeconds(.5f);
            SunEventManager.EmitEvent(SunEventID.Screen_FadedIn);
            
            yield return new WaitForSeconds(.5f);
            
            SunBaseUIController.ShowOutCanvas();
            SunBaseUIController.PushScreen<QuitUI>();
        }
    }
}