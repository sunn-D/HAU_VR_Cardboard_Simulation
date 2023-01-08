using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Teleport_Controller
{
    public class TeleportController : SunMonoSingleton<TeleportController>
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public List<TeleportObject> ListTeleObjects { get; set; }
        
        //
        public int StartTeleportID { get; set; }
        public int CurrentTeleportID { get; set; }
        public TeleportObject CurrentTeleportObject { get; set; }

        #endregion

        #region Functions
        
        //
        [Button(ButtonSizes.Large)] 
        public void GetAllTeleportObject()
        {
            var allTeleportObject = GameObject.FindObjectsOfType<TeleportObject>().ToList();
            allTeleportObject.Sort((t1, t2) =>
            {
                if (t1.TeleportID < t2.TeleportID) return -1;
                if (t1.TeleportID > t2.TeleportID) return 1;
                return 0;
            });
            ListTeleObjects = allTeleportObject;
        }

        //
        protected override void LoadInStart()
        {
            if (ListTeleObjects != null)
            {
                foreach (var teleportObject in ListTeleObjects)
                {
                    teleportObject.Initialize();
                    if (teleportObject.IsFirstTeleport)
                    {
                        StartTeleportID = teleportObject.TeleportID;
                        CurrentTeleportID = teleportObject.TeleportID;
                        CurrentTeleportObject = teleportObject;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No teleport object in this scene.");
                return;
            }
            
            SunEventManager.StartListening(EventID.Teleport_OnTeleportClicked, OnTeleportClicked);
            
            //
            // TeleportToFirstTeleportPoint();
        }

        //
        public void TeleportToFirstTeleportPoint()
        {
            CurrentTeleportObject.DisableTeleport();
            SunEventManager.EmitEvent(EventID.Teleport_OnTeleportPlayer, sender: CurrentTeleportObject.TelePoint);
        }
        
        //
        private void OnTeleportClicked()
        {
            CurrentTeleportObject.EnableTeleport();
            var newTeleportID = (int) SunEventManager.GetSender(EventID.Teleport_OnTeleportClicked);
            CurrentTeleportObject = ListTeleObjects.Find(o => o.TeleportID == newTeleportID);
            CurrentTeleportID = newTeleportID;
            CurrentTeleportObject.DisableTeleport();
        }

        #endregion
    }
}