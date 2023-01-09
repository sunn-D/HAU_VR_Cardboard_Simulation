using System;
using System.Collections.Generic;
using HAU_VR_Cardboard.Scripts.Scriptables;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    //
    [Serializable]
    public class AudioControl
    {
        #region Variables

        //
        public AudioConfig AudioConfig { get; set; }
        public AudioSource AudioSource { get; set; }

        #endregion

        #region Functions

        //
        public void Initialize(AudioConfig config, AudioSource source)
        {
            AudioConfig = config;
            AudioSource = source;
            AudioSource.clip = AudioConfig.AudioClip;
            AudioSource.volume = AudioConfig.VolumeClip;
            AudioSource.pitch = AudioConfig.PitchClip;
            AudioSource.loop = AudioConfig.Looping;
            AudioSource.playOnAwake = AudioConfig.PlayOnAwake;
        }
        
        //
        public void Play()
        {
            if (AudioSource.isPlaying) return;
            AudioSource.Play();
        }
        
        //
        public void Stop()
        {
            if (!AudioSource.isPlaying) return;
            AudioSource.Stop();
        }

        #endregion
    }
    
    //
    public class AudioManager : SunMonoSingleton<AudioManager>
    {
        #region Variables

        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] private List<AudioControl> ListAudioControl { get; set; }
        
        //
        public static bool BackgroundSoundValue
        {
            get => PlayerPrefs.GetInt("Audio_backgroundsoundvalue") == 1;
            set => PlayerPrefs.SetInt("Audio_backgroundsoundvalue", value ? 1 : 0);
        }
        public static bool FXSoundValue
        {
            get => PlayerPrefs.GetInt("Audio_fxsoundvalue") == 1;
            set => PlayerPrefs.SetInt("Audio_fxsoundvalue", value ? 1 : 0);
        }
        public static bool ActorSoundValue
        {
            get => PlayerPrefs.GetInt("Audio_actorsoundvalue") == 1;
            set => PlayerPrefs.SetInt("Audio_actorsoundvalue", value ? 1 : 0);
        }

        #endregion

        #region Functions

        //
        protected override void LoadInAwake()
        {
            //
            if (!PlayerPrefs.HasKey("Audio_backgroundsoundvalue")) BackgroundSoundValue = true;
            if (!PlayerPrefs.HasKey("Audio_fxsoundvalue")) FXSoundValue = true;
            if (!PlayerPrefs.HasKey("Audio_actorsoundvalue")) ActorSoundValue = true;
            
            //
            ListAudioControl = new List<AudioControl>();
            
            //
            foreach (var audioConfig in AudioConfigValue.Instance.ListAudioConfig)
            {
                var newAudioSource = new GameObject($"Audio {audioConfig.ClipName}");
                newAudioSource.transform.SetParent(transform);
                var audioSource = newAudioSource.AddComponent<AudioSource>();

                var newAudioControl = new AudioControl();
                newAudioControl.Initialize(audioConfig, audioSource);

                ListAudioControl.Add(newAudioControl);
            }
            
            //
            SetAudioVolume(AudioGroupType.BackgroundSound);
            SetAudioVolume(AudioGroupType.FXSound);
            SetAudioVolume(AudioGroupType.ActorSound);
            
            //
            foreach (var audioControl in ListAudioControl)
            {
                if (audioControl.AudioConfig.PlayOnAwake)
                {
                    audioControl.Play();
                }
            }
        }
        
        //
        public void SetAudioVolume(AudioGroupType type)
        {
            foreach (var audioControl in ListAudioControl)
            {
                if (audioControl.AudioConfig.AudioGroupType == type)
                {
                    var volumeValue = 0f;
                    switch (type)
                    {
                        case AudioGroupType.BackgroundSound:
                            volumeValue = BackgroundSoundValue ? audioControl.AudioConfig.VolumeClip : 0;
                            break;
                        case AudioGroupType.FXSound:
                            volumeValue = FXSoundValue ? audioControl.AudioConfig.VolumeClip : 0;
                            break;
                        case AudioGroupType.ActorSound:
                            volumeValue = ActorSoundValue ? audioControl.AudioConfig.VolumeClip : 0;
                            break;
                    }
                    audioControl.AudioSource.volume = volumeValue;
                }
            }
        }
        
        //
        public void PlaySound(string nameClip)
        {
            foreach (var audioControl in ListAudioControl)
            {
                if (audioControl.AudioConfig.ClipName == nameClip)
                {
                    audioControl.Play();
                    return;
                }
            }
        }
        
        //
        public void StopSound(string nameClip)
        {
            foreach (var audioControl in ListAudioControl)
            {
                if (audioControl.AudioConfig.ClipName == nameClip)
                {
                    audioControl.Stop();
                    return;
                }
            }
        }

        #endregion
    }
}