using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;
using UnityEngine.Rendering;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    public class SettingManager : SunMonoSingleton<SettingManager>
    {
        //
        public enum QualitySetting
        {
            Low, Medium, High
        }
        
        //
        [field: FoldoutGroup("Render Setting"), SerializeField] public RenderPipelineAsset LowSetting { get; set; }
        [field: FoldoutGroup("Render Setting"), SerializeField] public RenderPipelineAsset MediumSetting { get; set; }
        [field: FoldoutGroup("Render Setting"), SerializeField] public RenderPipelineAsset HighSetting { get; set; }
        
        // 1 - low, 2 - medium, 3 - high
        public SunIntPref QualitySettingField { get; set; }

        //
        protected override void LoadInStart()
        {
            QualitySettingField = new SunIntPref("Quality Setting", 2);
            QualitySettingField.FirstCheck();
            ChangeQualitySetting(QualitySettingField.GetValue());
            
        }
        
        //
        public void SetValueQualityField()
        {
            var qualityField = QualitySettingField.GetValue() + 1;
            if (qualityField == 4)
            {
                qualityField = 1;
            }

            QualitySettingField.SetValue(qualityField);
            ChangeQualitySetting(qualityField);
        }
        
        //
        public void ChangeQualitySetting(int qualityField)
        {
            switch (qualityField)
            {
                case 1:
                    QualitySettings.renderPipeline = LowSetting;
                    break;
                case 2:
                    QualitySettings.renderPipeline = MediumSetting;
                    break;
                case 3:
                    QualitySettings.renderPipeline = HighSetting;
                    break;
            }
        }
    }
}