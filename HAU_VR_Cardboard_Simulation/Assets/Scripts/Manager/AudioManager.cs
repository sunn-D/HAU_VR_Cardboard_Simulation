using System;
using System.Collections.Generic;
using DG.Tweening;
using DunnGSunn;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    [Serializable] public enum AudioGroupType
    {
        BGSound, UXSound
    }
    
    [Serializable] public class Sound
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Audio group")]
        [SerializeField] private AudioGroupType audioGroup = AudioGroupType.UXSound;
        //
        [FoldoutGroup("Variables/Clip")]
        [SerializeField] private string clipName;
        [FoldoutGroup("Variables/Clip")]
        [SerializeField] private AudioClip clip;
        //
        [FoldoutGroup("Variables/Clip fields")]
        [Range(0f, 1f), SerializeField] private float volume = 1f;
        [FoldoutGroup("Variables/Clip fields")]
        [Range(0f, 3f), SerializeField] private float pitch = 1f;
        [FoldoutGroup("Variables/Clip fields")]
        [SerializeField] private bool loop;
        [FoldoutGroup("Variables/Clip fields")]
        [SerializeField] private bool playOnAwake;
        //
        private AudioSource _audioSource;
        //
        public string ClipName
        {
            get => clipName;
            set => clipName = value;
        }
        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }
        public AudioClip AudioClip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }
        public bool IsPlaying
        {
            get => _audioSource.isPlaying;
        }
        public AudioGroupType AudioGroup
        {
            get => audioGroup;
            set => audioGroup = value;
        }

        #endregion

        #region Functions

        //
        public void InitializeSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
            _audioSource.clip = clip;
            _audioSource.pitch = pitch;
            _audioSource.volume = volume;
            _audioSource.playOnAwake = playOnAwake;
            _audioSource.loop = loop;
        }

        //
        public void Play()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }

        //
        public void Stop()
        {
            if (!_audioSource.isPlaying) return;
            _audioSource.Stop();
        }

        #endregion
    }
    
    public class AudioManager : SunMonoSingleton<AudioManager>
    {
        #region Variables

        [FoldoutGroup("Variables")] 
        //
        [FoldoutGroup("Variables/Audio")]
        [SerializeField] private AudioListener audioListener;
        //
        [FoldoutGroup("Variables/Audio")] 
        [SerializeField] private string bgSoundName = "bgsound";
        //
        [FoldoutGroup("Variables/Sounds")]
        [SerializeField] private List<Sound> sounds;
        //
        private Tweener _tweenPlayBGSong;
        private Tweener _tweenStopBGSong;
        //
        public static bool BGSoundValue
        {
            get => PlayerPrefs.GetInt("Audio bgsoundvalue") == 1;
            set
            {
                PlayerPrefs.SetInt("Audio bgsoundvalue", value ? 1 : 0);
            }
        }
        public static bool UXSoundValue
        {
            get => PlayerPrefs.GetInt("Audio uxsoundvalue") == 1;
            set
            {
                PlayerPrefs.SetInt("Audio uxsoundvalue", value ? 1 : 0);
            }
        }
        public static bool VibrationValue
        {
            get => PlayerPrefs.GetInt("Audio vibrationvalue") == 1;
            set
            {
                PlayerPrefs.SetInt("Audio vibrationvalue", value ? 1 : 0);
            }
        }

        #endregion

        #region Functions

        //
        protected override void LoadInAwake()
        {
            //
            if (!PlayerPrefs.HasKey("Audio bgsoundvalue")) BGSoundValue = true;
            if (!PlayerPrefs.HasKey("Audio uxsoundvalue")) UXSoundValue = true;
            if (!PlayerPrefs.HasKey("Audio vibrationvalue")) VibrationValue = true;
            
            //
            foreach (var sound in sounds)
            {
                var go = new GameObject($"Audio_{sound.ClipName}");
                go.transform.SetParent(transform);
                sound.InitializeSource(go.AddComponent<AudioSource>());
                sound.Volume = sound.AudioGroup == AudioGroupType.BGSound ? BGSoundValue ? 1 : 0 : UXSoundValue ? 1 : 0;
            } 
        }
        
        //
        public void PlaySound(string nameClip)
        {
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.Play();
                    return;
                }
            }
        }
        
        //
        public void StopSound(string nameClip)
        {
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.Stop();
                    return;
                }
            }
        }

        #endregion

        #region Event callbacks

        //
        private void OnBGSoundValueChange()
        {
            var bgSound = sounds.Find(s => s.ClipName == bgSoundName);
            if (bgSound.IsPlaying)
            {
                if (BGSoundValue)
                {
                    _tweenPlayBGSong?.Kill();
                    _tweenPlayBGSong = DOTween.To(() => bgSound.Volume, x => bgSound.Volume = x, 1f, .25f)
                        .OnStart(() =>
                        {
                            bgSound.Stop();
                            audioListener.enabled = true;
                            bgSound.Volume = 0f;
                            bgSound.Play();
                        });
                }
                else
                {
                    _tweenStopBGSong?.Kill();
                    _tweenStopBGSong = DOTween.To(() => bgSound.Volume, x => bgSound.Volume = x, 0f, .25f);
                }
            }
            else
            {
                bgSound.Volume = BGSoundValue ? 1 : 0;
            }
        }
        
        //
        private void OnVibrationValueChange()
        {
            
        }
        
        //
        private void OnUXSoundValueChange()
        {
            
        }

        #endregion
    }
}