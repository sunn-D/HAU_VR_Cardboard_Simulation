using Sun_Package;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class ButtonBack : BaseButtonController
    {
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            SunBaseUIController.PopAllScreen();
        }
    }
}