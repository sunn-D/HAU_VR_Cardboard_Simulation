using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    public class AppInitialize : SunMonoSingleton<AppInitialize>
    {
        #region Functions

        //
        protected override void LoadInAwake()
        {
            DefaultVariables();
        }
        
        //
        protected override void LoadInStart()
        {
            DestroyImmediate(gameObject);
        }

        //
        private void DefaultVariables()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }
        
        #endregion
    }
}