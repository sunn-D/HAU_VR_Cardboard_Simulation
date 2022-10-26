using DunnGSunn;

namespace UI_Controller.UI
{
    public class UIHome : SunBaseUI
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