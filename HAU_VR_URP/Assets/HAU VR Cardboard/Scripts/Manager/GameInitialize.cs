using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    public class GameInitialize : SunMonoSingleton<GameInitialize>
    {
        //
        protected override void LoadInAwake()
        {
            SetupApplication();
        }
        
        //
        private void SetupApplication()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }
    }
}