using DunnGSunn;
using UnityEngine;

namespace UI_Controller.UI
{
    public class UIMap : SunBaseUI
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