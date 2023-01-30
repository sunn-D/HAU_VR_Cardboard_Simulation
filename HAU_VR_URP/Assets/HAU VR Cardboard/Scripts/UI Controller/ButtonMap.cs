using HAU_VR_Cardboard.Scripts.UI_Controller.UI;
using Sun_Package;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class ButtonMap : BaseButtonController
    {
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            SunBaseUIController.PushScreen<MapUI>();
        }
    }
}