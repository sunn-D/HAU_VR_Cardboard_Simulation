using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Scriptables;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class ButtonFXSound : BaseButtonController
    {
        public Sprite offImage;
        public Sprite onImage;

        private void Start()
        {
            ImageButton.sprite = AudioManager.FXSoundValue ? onImage : offImage;
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();
            AudioManager.FXSoundValue = !AudioManager.FXSoundValue;
            AudioManager.Instance.SetAudioVolume(AudioGroupType.FXSound);
            ImageButton.sprite = AudioManager.FXSoundValue ? onImage : offImage;
        }
    }
}