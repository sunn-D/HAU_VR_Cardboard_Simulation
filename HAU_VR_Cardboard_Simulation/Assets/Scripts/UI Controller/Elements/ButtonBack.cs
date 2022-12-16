using Sun_Package;
using UI_Controller.UI;
using UnityEngine;

namespace UI_Controller.Elements
{
    public class ButtonBack : UIBaseButton
    {
        #region Functions
        
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            SunBaseUIController.PopAllScreen();
        }

        #endregion
    }
}