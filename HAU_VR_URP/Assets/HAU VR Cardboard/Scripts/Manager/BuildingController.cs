using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Manager
{
    //
    public enum BuildingEnum
    {
        Campus, Floor, Room
    }
    
    public class BuildingController : SunMonoSingleton<BuildingController>
    {
        //
        [field: FoldoutGroup("Building"), SerializeField] public GameObject HAUCampus { get; set; }
        [field: FoldoutGroup("Building"), SerializeField] public GameObject NPCCampus { get; set; }
        [field: FoldoutGroup("Building"), SerializeField] public GameObject SevenFloor { get; set; }
        [field: FoldoutGroup("Building"), SerializeField] public GameObject LabRoom { get; set; }
        
        
        //
        protected override void LoadInStart()
        {
            ShowBuilding(BuildingEnum.Campus);
        }

        //
        public void ShowBuilding(BuildingEnum value)
        {
            switch (value)
            {
                case BuildingEnum.Campus:
                    HAUCampus.SetActive(true);
                    SevenFloor.SetActive(false);
                    LabRoom.SetActive(false);
                    break;
                case BuildingEnum.Floor:
                    HAUCampus.SetActive(false);
                    SevenFloor.SetActive(true);
                    LabRoom.SetActive(false);
                    break;
                case BuildingEnum.Room:
                    HAUCampus.SetActive(false);
                    SevenFloor.SetActive(false);
                    LabRoom.SetActive(true);
                    break;
            }
        }
        
        //
        public void ShowNPCCampus(bool active = true)
        {
            NPCCampus.SetActive(active);
        }
    }
}