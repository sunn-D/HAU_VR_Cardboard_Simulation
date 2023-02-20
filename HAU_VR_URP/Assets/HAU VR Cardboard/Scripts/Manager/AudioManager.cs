using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    //
    public enum AudioGroupType { BgSound, ActorSound }
    
    //
    [Serializable] 
    public class Sound
    {
        #region Variables

        [field: FoldoutGroup("Base fields"), SerializeField] public AudioGroupType AudioGroup { get; private set; }
        [field: FoldoutGroup("Base fields"), SerializeField] public string ClipName { get; private set; }
        [field: FoldoutGroup("Base fields"), SerializeField] public AudioClip Clip { get; private set; }
        [field: FoldoutGroup("Clip fields"), Range(0f, 1f), SerializeField] public float Volume { get; private set; } = 1f;
        [field: FoldoutGroup("Clip fields"), Range(0f, 3f), SerializeField] public float Pitch { get; private set; } = 1f;
        [field: FoldoutGroup("Clip fields"), SerializeField] public bool Loop { get; private set; }
        [field: FoldoutGroup("Clip fields"), SerializeField] public bool PlayOnAwake { get; private set; }
        
        //
        public AudioSource AudioSource { get; private set; }

        #endregion

        #region Functions

        //
        public void InitializeSource(AudioSource audioSource)
        {
            AudioSource = audioSource;
            AudioSource.clip = Clip;
            AudioSource.pitch = Pitch;
            AudioSource.volume = Volume;
            AudioSource.loop = Loop;
            AudioSource.playOnAwake = PlayOnAwake;
        }
        
        //
        public void SetAudioClip(AudioClip clip)
        {
            AudioSource.clip = clip;
        }
        
        //
        public void Play(bool checkPlaying = true)
        {
            if (checkPlaying)
            {
                if (AudioSource.isPlaying) return;
                AudioSource.Play();
            }
            else
            {
                if (AudioSource.isPlaying) AudioSource.Stop();
                AudioSource.Play();
            }
        }

        //
        public void Stop()
        {
            if (!AudioSource.isPlaying) return;
            AudioSource.Stop();
        }
        
        //
        public void SetVolume(bool volumeValue)
        {
            AudioSource.volume = volumeValue ? Volume : 0f;
        }
        
        //
        public void SetVolume(float volumeValue)
        {
            AudioSource.volume = volumeValue <= Volume ? volumeValue : Volume;
        }

        #endregion
    }
    
    //
    public class AudioManager : SunMonoSingleton<AudioManager>
    {
        #region Variables
        
        //
        public static SunBoolPref BgSoundValue { get; private set; }
        public static SunBoolPref ActorSoundValue { get; private set; }
        
        //
        [field: FoldoutGroup("Sounds"), SerializeField] public List<Sound> Sounds { get; set; }
        
        //
        private Tweener _tweenPlayBackgroundSound;

        #endregion

        #region Functions

        //
        protected override void LoadInAwake()
        {
            BgSoundValue = new SunBoolPref("Audio_BGSoundValue", true);
            ActorSoundValue = new SunBoolPref("Audio_UXSoundValue", true);
            BgSoundValue.FirstCheck();
            ActorSoundValue.FirstCheck();
            BgSoundValue.SetupSetEvent(true, SunEventID.Audio_BGSound);
            ActorSoundValue.SetupSetEvent(true, SunEventID.Audio_ActorSound);
            
            //
            foreach (var sound in Sounds)
            {
                var go = new GameObject(sound.ClipName);
                go.transform.SetParent(transform);
                sound.InitializeSource(go.AddComponent<AudioSource>());
                if (sound.PlayOnAwake && (sound.AudioGroup == AudioGroupType.BgSound ? BgSoundValue.GetValue() : ActorSoundValue.GetValue()))
                {
                    sound.Play();
                }
            }
            
            //
            SunEventManager.StartListening(SunEventID.Audio_BGSound, OnBGSoundValueChange);
        }

        //
        public void PlayActorSound(string nameClip)
        {
            if (!ActorSoundValue.GetValue()) return;
            foreach (var sound in Sounds)
            {
                if (sound.AudioGroup == AudioGroupType.ActorSound)
                {
                    if (sound.ClipName.Contains(nameClip))
                        sound.Play();
                    else
                        sound.Stop();
                }
            }
        }
        
        //
        public void StopActorSound(string nameClip)
        {
            if (!ActorSoundValue.GetValue()) return;
            foreach (var sound in Sounds)
            {
                if (sound.AudioGroup == AudioGroupType.ActorSound)
                {
                    if (sound.ClipName.Contains(nameClip))
                        sound.Stop();
                }
            }
        }
        
        //
        public void PlaySound(string nameClip)
        {
            if (!BgSoundValue.GetValue()) return;
            foreach (var sound in Sounds)
            {
                if (sound.ClipName.Contains(nameClip))
                {
                    sound.Play();
                    return;
                }
            }
        }
        
        //
        public void StopSound(string nameClip)
        {
            if (!BgSoundValue.GetValue()) return;
            foreach (var sound in Sounds)
            {
                if (sound.ClipName.Contains(nameClip))
                {
                    sound.Stop();
                    return;
                }
            }
        }
        
        //
        public float DurationSound(string nameClip)
        {
            foreach (var sound in Sounds)
            {
                if (sound.ClipName.Contains(nameClip))
                {
                    return sound.Clip.length;
                }
            }

            return -1f;
        }

        #endregion

        #region Event callbacks

        //
        private void OnBGSoundValueChange(object sender)
        {
            var bgSound = Sounds.Find(o => o.ClipName.Contains("Background Sound"));
            if (BgSoundValue.GetValue())
            {
                _tweenPlayBackgroundSound?.Kill();
                var currentValue = 0f;
                var endValue = 1f;
                _tweenPlayBackgroundSound = DOTween.To(() => currentValue, x => currentValue = x, endValue, .25f)
                    .OnStart(() => bgSound.Play())
                    .OnUpdate(() => bgSound.SetVolume(currentValue));
            }
            else
            {
                _tweenPlayBackgroundSound?.Kill();
                var currentValue = 1f;
                var endValue = 0f;
                _tweenPlayBackgroundSound = DOTween.To(() => currentValue, x => currentValue = x, endValue, .25f)
                    .OnUpdate(() => bgSound.SetVolume(currentValue))
                    .OnComplete(() => bgSound.Stop());
            }
        }

        #endregion
    }
}