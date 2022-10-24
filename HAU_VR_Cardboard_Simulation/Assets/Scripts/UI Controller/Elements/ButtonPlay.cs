using UnityEngine;

namespace UI_Controller.Elements
{
    public class ButtonPlay : UIBaseButton
    {
        #region Functions
        
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            Debug.Log("Play app");
        }

        #endregion
    }
}