using HAU_VR_Cardboard.Scripts.Manager;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller.SettingUI
{
    public class ButtonActorSound : BaseButtonController
    {
        public Sprite offImage;
        public Sprite onImage;

        private void Start()
        {
            ImageButton.sprite = AudioManager.ActorSoundValue.GetValue() ? onImage : offImage;
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();
            AudioManager.ActorSoundValue.SetValue(!AudioManager.ActorSoundValue.GetValue());
            ImageButton.sprite = AudioManager.ActorSoundValue.GetValue() ? onImage : offImage;
        }
    }
}