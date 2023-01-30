using System;
using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Scriptables;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class ButtonBackgroundSound : BaseButtonController
    {
        public Sprite offImage;
        public Sprite onImage;

        private void Start()
        {
            ImageButton.sprite = AudioManager.BackgroundSoundValue ? onImage : offImage;
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();
            AudioManager.BackgroundSoundValue = !AudioManager.BackgroundSoundValue;
            AudioManager.Instance.SetAudioVolume(AudioGroupType.BackgroundSound);
            AudioManager.Instance.PlaySound("Background");
            ImageButton.sprite = AudioManager.BackgroundSoundValue ? onImage : offImage;
        }
    }
}