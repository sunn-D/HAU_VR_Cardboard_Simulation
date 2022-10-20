using DunnGSunn;
using Manager;
using UnityEngine;

namespace UI_Controller
{
    public class UIFirstLoading : SunBaseUI
    {
        #region Functions

        //
        public override void Initialize()
        {
            Show();
        }

        //
        public override void Show()
        {
            ShowTemplate();
            
        }

        //
        public override void Hide()
        {
            HideTemplate();
        }

        #endregion
        
    }
}