using Sun_Package;
using UI_Controller.UI;
using UnityEngine;

namespace UI_Controller.Elements
{
    public class ButtonSetting : UIBaseButton
    {
        #region Functions
        
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            SunBaseUIController.PushScreen<UISetting>();
        }

        #endregion
    }
}