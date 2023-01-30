using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.UI
{
    public class SettingUI : SunBaseUI
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