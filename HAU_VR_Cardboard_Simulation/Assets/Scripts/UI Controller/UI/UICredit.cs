using Sun_Package;
using UnityEngine;

namespace UI_Controller.UI
{
    public class UICredit : SunBaseUI
    {
        #region Functions

        public override void Initialize()
        {
            
        }

        public override void Show()
        {
            CheckShowTemplate();
            ShowTemplate();
        }

        public override void Hide()
        {
            CheckHideTemplate();
            HideTemplate();
        }

        #endregion
    }
}