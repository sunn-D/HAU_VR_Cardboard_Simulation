using Sun_Package;
using UI_Controller.UI;

namespace UI_Controller.Elements
{
    public class ButtonMap : UIBaseButton
    {
        #region Functions
        
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            SunBaseUIController.PushScreen<UIMap>();
        }

        #endregion
    }
}