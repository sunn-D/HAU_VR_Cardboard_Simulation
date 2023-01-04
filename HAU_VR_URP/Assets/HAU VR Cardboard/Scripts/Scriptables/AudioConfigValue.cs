using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Scriptables
{ 
    //
    public enum AudioGroupType { BackgroundSound, FXSound, ActorSound }
    
    //
    [Serializable]
    public class AudioConfig
    {
        //
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public AudioGroupType AudioGroupType { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public string ClipName { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public AudioClip AudioClip { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField, Range(0f, 1f)] public float VolumeClip { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField, Range(0f, 1f)] public float PitchClip { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool Looping { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool PlayOnAwake { get; set; }
    }
    
    //
    [CreateAssetMenu(fileName = "Audio Config", menuName = "HAU_VR/Audio Config", order = 0)]
    public class AudioConfigValue : SunScriptableSingleton<AudioConfigValue>
    {
        //
        [FoldoutGroup("Variables")] 
        [field: SerializeField] public List<AudioConfig> ListAudioConfig { get; set; } = new List<AudioConfig>();

        //
        [FoldoutGroup("Buttons")] 
        [Button(ButtonSizes.Large)]
        public void AddAudioBackground()
        {
            if (ListAudioConfig == null) ListAudioConfig = new List<AudioConfig>();
            var newAudio = new AudioConfig();
            newAudio.AudioGroupType = AudioGroupType.BackgroundSound;
            ListAudioConfig.Add(newAudio);
        }
        
        //
        [FoldoutGroup("Buttons")] 
        [Button(ButtonSizes.Large)]
        public void AddAudioFX()
        {
            if (ListAudioConfig == null) ListAudioConfig = new List<AudioConfig>();
            var newAudio = new AudioConfig();
            newAudio.AudioGroupType = AudioGroupType.FXSound;
            ListAudioConfig.Add(newAudio);
        }
        
        //
        [FoldoutGroup("Buttons")] 
        [Button(ButtonSizes.Large)]
        public void AddAudioActor()
        {
            if (ListAudioConfig == null) ListAudioConfig = new List<AudioConfig>();
            var newAudio = new AudioConfig();
            newAudio.AudioGroupType = AudioGroupType.ActorSound;
            ListAudioConfig.Add(newAudio);
        }
    }
}