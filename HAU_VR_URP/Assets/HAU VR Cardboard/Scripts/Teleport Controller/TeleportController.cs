using System.Collections.Generic;
using System.Linq;
using HAU_VR_Cardboard.Scripts.Manager;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Teleport_Controller
{
    public class TeleportController : SunMonoSingleton<TeleportController>
    {
        #region Variables

        //
        [field: FoldoutGroup("Teleport"), SerializeField] public int StartTeleportID { get; set; }
        [field: FoldoutGroup("Teleport"), SerializeField] public List<Teleport> ListTeleports { get; set; }
        
        //
        public int CurrentTeleportID { get; set; }
        public Teleport CurrentTeleport { get; set; }

        #endregion

        #region Functions
        
        //
        [Button(ButtonSizes.Large)] 
        public void GetAllTeleportObject()
        {
            var allTeleportObject = GameObject.FindObjectsOfType<Teleport>().ToList();
            allTeleportObject.Sort((t1, t2) =>
            {
                if (t1.TeleportID < t2.TeleportID) return -1;
                if (t1.TeleportID > t2.TeleportID) return 1;
                return 0;
            });
            ListTeleports = allTeleportObject;
        }

        //
        protected override void LoadInStart()
        {
            SunEventManager.StartListening(SunEventID.Teleport_TeleportClicked, OnTeleportClicked);
            DisableAllTeleport();
        }

        //
        public void EnableAllTeleport()
        {
            foreach (var teleport in ListTeleports)
            {
                teleport.gameObject.SetActive(true);
                teleport.EnableTeleport();
            }
        }
        
        //
        public void DisableAllTeleport()
        {
            foreach (var teleport in ListTeleports)
            {
                teleport.DisableTeleport();
                teleport.gameObject.SetActive(false);
            }
        }

        //
        public void TeleportToFirstTeleportPoint()
        {
            var firstTeleport = ListTeleports.Find(o => o.TeleportID == StartTeleportID);
            firstTeleport.DisableTeleport();
            CurrentTeleport = firstTeleport;
            CurrentTeleportID = StartTeleportID;
            SunEventManager.EmitEvent(SunEventID.Teleport_TeleportPlayer, sender: firstTeleport.TeleportPosition);
        }
        
        //
        private void OnTeleportClicked(object sender)
        {
            EnableAllTeleport();
            var newTeleportID = (int) sender;
            CurrentTeleport = ListTeleports.Find(o => o.TeleportID == newTeleportID);
            CurrentTeleport.DisableTeleport();
            if (CurrentTeleport.ChangeOutside)
            {
                BuildingController.Instance.ShowBuilding(CurrentTeleport.ShowBuilding);
                EnableAllTeleport();
            }
            if (CurrentTeleport.TeleportToOtherPoint)
            {
                var disableTeleport = ListTeleports.Find(o => o.TeleportID == CurrentTeleport.TeleportToID);
                disableTeleport.DisableTeleport();
            }
            CurrentTeleportID = newTeleportID;
        }

        #endregion
    }
}