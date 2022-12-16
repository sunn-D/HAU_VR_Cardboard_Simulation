using System.Collections;
using Sun_Package;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI_Controller.Elements
{
    public class ButtonPlay : UIBaseButton
    {
        #region Variables

        //
        [FoldoutGroup("Variables/Config")]
        public float delayPlay;

        #endregion
        
        #region Functions
        
        //
        public override void OnPointerClick()
        {
            base.OnPointerClick();
            Debug.Log("Play app");
        }

        private IEnumerator StartPlay()
        {
            SunEventManager.EmitEvent(EventID.ScreenFadedOut);

            yield return new WaitForSeconds(.5f);
            
            
        }

        #endregion
    }
}