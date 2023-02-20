using HAU_VR_Cardboard.Scripts.Manager;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.SettingUI
{
    public class ButtonBackgroundSound : BaseButtonController
    {
        public Sprite offImage;
        public Sprite onImage;

        private void Start()
        {
            ImageButton.sprite = AudioManager.BgSoundValue.GetValue() ? onImage : offImage;
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();
            AudioManager.BgSoundValue.SetValue(!AudioManager.BgSoundValue.GetValue());
            ImageButton.sprite = AudioManager.BgSoundValue.GetValue() ? onImage : offImage;
        }
    }
}