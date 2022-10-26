using DunnGSunn;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Manager
{
    public class AppPreferences : SunMonoSingleton<AppPreferences>
    {
        #region Variables
        
        //
        [FoldoutGroup("Default Variables")]
        public MovementStyle movementStyle = Player.MovementStyle.Teleport;

        // 
        public static int MovementStyle
        {
            get => PlayerPrefs.GetInt("GamePref movementstyle");
            set
            {
                PlayerPrefs.SetInt("GamePref movementstyle", value);
                SunEventManager.EmitEvent(EventID.OnMovementStyleChange, sender: value);
            }
        }

        #endregion
        
        #region Functions

        protected override void LoadInAwake()
        {
            //
            if (!PlayerPrefs.HasKey("GamePref movementstyle")) MovementStyle = (int) movementStyle;

        }

        #endregion
    }
}