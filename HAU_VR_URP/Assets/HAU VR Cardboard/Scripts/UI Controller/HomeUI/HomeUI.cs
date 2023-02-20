using Sun_Package;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.HomeUI
{
    public class HomeUI : SunBaseUI
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