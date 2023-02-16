using System;
using DG.Tweening;
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

        //
        public bool IsDisable { get; set; }
        
        //
        private readonly Color TransparentColor = new Color(1f, 1f, 1f, 0f);
        private readonly Color NormalColor = new Color(1f, 1f, 1f, 1f);
        private readonly Color PointEnterColor = new Color(.9f, .7f, .3f, 1f);
        
        #endregion
        
        //
        private void Reset()
        {
            TeleportCollider = GetComponent<BoxCollider>();
            TeleportMesh = transform.Find("Particle Mesh").GetComponent<ParticleSystem>();
            TeleportGround = transform.Find("Particle Ground").GetComponent<ParticleSystem>();
            TeleportEffect = transform.Find("Particle Effect").GetComponent<ParticleSystem>();
            TeleportPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            TeleportID = gameObject.name.Split(' ')[2].TryParseInt();
        }

        public void OnPointerExit()
        {
            Debug.Log($"Teleport {TeleportID} pointer exit.");
        }

        public void OnPointerEnter()
        {
            Debug.Log($"Teleport {TeleportID} pointer enter.");
        }

        public void OnPointerClick()
        {
            AudioManager.Instance.PlaySound("Splash");
            Debug.Log($"Teleport {TeleportID} pointer click.");
        }
    }
}