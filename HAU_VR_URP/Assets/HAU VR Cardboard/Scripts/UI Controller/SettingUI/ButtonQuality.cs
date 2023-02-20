using System;
using HAU_VR_Cardboard.Scripts.Manager;
using TMPro;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.SettingUI
{
    public class ButtonQuality : BaseButtonController
    {
        public TextMeshProUGUI text;

        public string lowQuality = "Thấp";
        public string mediumQuality = "Vừa";
        public string highQuality = "Cao";

        private void OnEnable()
        {
            SetText();
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();
            SettingManager.Instance.SetValueQualityField();
            SetText();
        }
        
        //
        public void SetText()
        {
            switch (SettingManager.Instance.QualitySettingField.GetValue())
            {
                case 1:
                    text.text = lowQuality;
                    break;
                case 2: 
                    text.text = mediumQuality;
                    break;
                case 3: 
                    text.text = highQuality;
                    break;
            }
        }
    }
}