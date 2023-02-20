using HAU_VR_Cardboard.Scripts.Manager;
using HAU_VR_Cardboard.Scripts.Player_Controller;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Teleport_Controller
{
    public class Teleport : MonoBehaviour, IPointerAction
    {
        #region Variables

        [field: FoldoutGroup("Variables"), SerializeField] public int TeleportID { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public Vector3 TeleportPosition { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public BoxCollider TeleportCollider { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public ParticleSystem TeleportMesh { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public ParticleSystem TeleportGround { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public ParticleSystem TeleportEffect { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public bool TeleportToOtherPoint { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField, ShowIf(nameof(TeleportToOtherPoint))] public int TeleportToID { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public bool ChangeOutside { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField, ShowIf(nameof(ChangeOutside))] public BuildingEnum ShowBuilding { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public bool ChangeLight { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField, ShowIf(nameof(ChangeLight))] public Vector3 LightRotation { get; set; }

        #endregion
        
        //
        private void Reset()
        {
            TeleportCollider = GetComponent<BoxCollider>();
            TeleportMesh = transform.Find("Particle Mesh").GetComponent<ParticleSystem>();
            TeleportGround = transform.Find("Particle Ground").GetComponent<ParticleSystem>();
            TeleportEffect = transform.Find("Particle Effect").GetComponent<ParticleSystem>();
            TeleportPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            TeleportID = SunFuncString.ParseInt(gameObject.name.Split(' ')[2]);
        }

        //
        public void EnableTeleport()
        {
            TeleportMesh.Play();
            TeleportGround.Play();
            TeleportEffect.Play();
            TeleportCollider.enabled = true;
        }
        
        //
        public void DisableTeleport()
        {
            TeleportMesh.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            TeleportEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            TeleportCollider.enabled = false;
        }

        //
        public void OnPointerEnter()
        {
        }

        //
        public void OnPointerClick()
        {
            AudioManager.Instance.PlaySound("Splash");
            SunEventManager.EmitEvent(SunEventID.Teleport_TeleportClicked, sender: TeleportID);
            SunEventManager.EmitEvent(SunEventID.Teleport_TeleportPlayer, sender: TeleportPosition);
        }

        //
        public void OnPointerExit()
        {
        }
    }
}