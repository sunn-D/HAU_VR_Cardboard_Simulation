using DunnGSunn;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Object_Controller
{
    public class Teleport : MonoBehaviour, IPointerAction
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Configs")]
        public int teleportID;
        //
        [FoldoutGroup("Variables/Collider")]
        public CapsuleCollider teleCollider;
        //
        [FoldoutGroup("Variables/Renderer")] 
        public GameObject teleRenderer;
        //
        [FoldoutGroup("Variables/Point")] 
        public Transform telePoint;

        #endregion

        #region Functions

        //
        public void EnableTeleport()
        {
            teleRenderer.SetActive(true);
            teleCollider.enabled = true;
        }
        
        //
        public void DisableTeleport()
        {
            teleRenderer.SetActive(false);
            teleCollider.enabled = false;
        }

        #endregion

        #region Interface

        //
        public void OnPointerEnter()
        {
            // Debug.Log("Change color or something else on renderer.");
        }

        //
        public void OnPointerClick()
        {
            SunEventManager.EmitEvent(EventID.OnTeleport, sender: telePoint.position);
            SunEventManager.EmitEvent(EventID.OnTeleportPointerClick, sender: teleportID);
            Debug.Log("Teleport player.");
        }

        //
        public void OnPointerExit()
        {
            // Debug.Log("Change color or something else on renderer.");
        }

        #endregion
    }
}