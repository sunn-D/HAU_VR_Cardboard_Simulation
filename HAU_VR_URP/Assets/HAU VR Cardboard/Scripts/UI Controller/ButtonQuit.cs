using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class ButtonQuit : BaseButtonController
    {
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            Application.Quit();
        }
    }
}