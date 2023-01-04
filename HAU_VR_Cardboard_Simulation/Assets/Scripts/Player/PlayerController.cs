using Sun_Package;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Pointer")]
        public Image pointProcessImage;
        //
        [FoldoutGroup("Variables/Faded")] 
        public SmoothFaded smoothFaded;

        #endregion

        #region Functions

        //
        private void Start()
        {
            Initialize();
            ListeningEvents();
        }
        
        //
        private void Initialize()
        {
            //
            pointProcessImage.fillAmount = 0f;
            pointProcessImage.gameObject.SetActive(false);
        }
        
        //
        private void ListeningEvents()
        {
            //
            SunEventManager.StartListening(EventID.OnGazerTimerStart, OnGazerTimerStart);
            SunEventManager.StartListening(EventID.OnGazerTimerProcess, OnGazerTimerProcess);
            SunEventManager.StartListening(EventID.OnGazerTimerFinish, OnGazerTimerFinish);
            
            //
            SunEventManager.StartListening(EventID.OnTeleport, OnTeleport);
            
            //
            SunEventManager.StartListening(EventID.ScreenFadedIn, ScreenFadedIn);
            SunEventManager.StartListening(EventID.ScreenFadedOut, ScreenFadedOut);
        }

        #endregion

        #region Events

        //
        private void OnGazerTimerStart()
        {
            pointProcessImage.fillAmount = 0f;
            pointProcessImage.gameObject.SetActive(true);
        }

        //
        private void OnGazerTimerProcess()
        {
            var process = (float) SunEventManager.GetSender(EventID.OnGazerTimerProcess);
            pointProcessImage.fillAmount = process;
        }

        //
        private void OnGazerTimerFinish()
        { 
            pointProcessImage.gameObject.SetActive(false);
        }

        //
        private void OnTeleport()
        {
            var newPosition = (Vector3) SunEventManager.GetSender(EventID.OnTeleport);
            transform.position = newPosition;
        }
        
        //
        private void ScreenFadedOut()
        {
            smoothFaded.FadeOut();
        }

        //
        private void ScreenFadedIn()
        {
            smoothFaded.FadeIn();   
        }

        #endregion
    }
}