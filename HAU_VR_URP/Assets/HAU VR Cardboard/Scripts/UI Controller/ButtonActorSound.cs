using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Scriptables;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.UI_Controller
{
    public class ButtonActorSound : BaseButtonController
    {
        public Sprite offImage;
        public Sprite onImage;

        private void Start()
        {
            ImageButton.sprite = AudioManager.ActorSoundValue ? onImage : offImage;
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();
            AudioManager.ActorSoundValue = !AudioManager.ActorSoundValue;
            AudioManager.Instance.SetAudioVolume(AudioGroupType.ActorSound);
            ImageButton.sprite = AudioManager.ActorSoundValue ? onImage : offImage;
        }
    }
}