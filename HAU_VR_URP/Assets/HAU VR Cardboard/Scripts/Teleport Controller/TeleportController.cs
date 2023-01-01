using System;
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
        [FoldoutGroup("Variables")] 
        [SerializeField] private List<TeleportObject> listTeleObjects;
        
        //
        public List<TeleportObject> ListTeleObjects
        {
            get => listTeleObjects;
            set => listTeleObjects = value;
        }
        
        //
        public int StartTeleportID { get; set; }
        public int CurrentTeleportID { get; set; }
        public TeleportObject CurrentTeleportObject { get; set; }

        #endregion

        #region Functions
        
        //
        private void Reset()
        {
            listTeleObjects = GameObject.FindObjectsOfType<TeleportObject>().ToList();
        }

        //
        protected override void LoadInStart()
        {
            if (ListTeleObjects == null || ListTeleObjects.Count == 0)
            {
                listTeleObjects = GameObject.FindObjectsOfType<TeleportObject>().ToList();
            }

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
        }

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