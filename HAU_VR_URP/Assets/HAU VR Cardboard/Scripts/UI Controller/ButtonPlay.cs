using System.Collections;
using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Teleport_Controller;
using HAU_VR_Cardboard.Scripts.UI_Controller.UI;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
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
            SunEventManager.EmitEvent(EventID.Screen_FadedOut);
            
            yield return new WaitForSeconds(.5f);
            TeleportController.Instance.EnableAllTeleport();
            TeleportController.Instance.TeleportToFirstTeleportPoint();

            yield return new WaitForSeconds(.5f);
            SunEventManager.EmitEvent(EventID.Screen_FadedIn);
            
            yield return new WaitForSeconds(.5f);
            AudioManager.Instance.PlayActorSound("Congtruong");

            SunBaseUIController.ShowOutCanvas();
            SunBaseUIController.PushScreen<QuitUI>();
        }
    }
}