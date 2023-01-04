using DG.Tweening;
using HAU_VR_Cardboard.Scripts.Player_Controller;
using Sirenix.OdinInspector;
using Sun_Package;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Teleport_Controller
{
    public class TeleportObject : MonoBehaviour, IPointerAction
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public int TeleportID { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool IsFirstTeleport { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public Vector3 TelePoint { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public BoxCollider TeleCollider { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public Transform TeleTransform { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public SpriteRenderer TeleRenderer { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public SpriteRenderer GroundRenderer { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public ParticleSystem ParticleFX { get; set; }

        //
        private Sequence _tweenRenderTeleport;
        private Tweener _tweenColorTeleport;
        private Tweener _tweenColorGround;
        private bool _isDisable;
        
        //
        private readonly Color TransparentColor = new Color(1f, 1f, 1f, 0f);
        private readonly Color NormalColor = new Color(1f, 1f, 1f, 1f);
        private readonly Color PointEnterColor = new Color(.9f, .7f, .3f, 1f);

        #endregion

        #region Functions

        //
        private void Reset()
        {
            TeleportID = gameObject.name.Split(' ')[1].TryParseInt();
            TelePoint = transform.position;
            TeleCollider = GetComponent<BoxCollider>();
            TeleTransform = transform.Find("Render Teleport").transform;
            TeleRenderer = transform.Find("Render Teleport").GetComponent<SpriteRenderer>();
            GroundRenderer = transform.Find("Render Ground").GetComponent<SpriteRenderer>();
            ParticleFX = GetComponentInChildren<ParticleSystem>();
        }
        
        //
        public void Initialize()
        {
            //
            _tweenRenderTeleport = DOTween.Sequence();
            _tweenRenderTeleport.Append(TeleTransform.DOMoveY(0f, .75f).SetEase(Ease.Linear));
            _tweenRenderTeleport.AppendInterval(.5f);
            _tweenRenderTeleport.SetLoops(-1, LoopType.Yoyo);
            _tweenRenderTeleport.Play();
            
            //
            _tweenColorGround = GroundRenderer.DOColor(TransparentColor, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            _tweenColorGround.Play();
            
            //
            ParticleFX.Play();
            GroundRenderer.transform.localScale = Vector3.one * 2;
        }
        
        //
        public void EnableTeleport()
        {
            TeleRenderer.color = NormalColor;
            TeleCollider.enabled = true;
            ParticleFX.Play();
            GroundRenderer.transform.localScale = Vector3.one * 2;
            _tweenRenderTeleport?.Play();
            _isDisable = false;
        }
        
        //
        public void DisableTeleport()
        {
            _tweenColorTeleport?.Kill();
            _tweenRenderTeleport?.Pause();
            ParticleFX.Stop();
            GroundRenderer.transform.localScale = Vector3.one;
            TeleRenderer.color = TransparentColor;
            TeleCollider.enabled = false;
        }

        #endregion
        
        #region Implement Interface

        //
        public void OnPointerEnter()
        {
            _tweenColorTeleport?.Kill();
            _tweenColorTeleport = TeleRenderer.DOColor(PointEnterColor, .25f);
        }

        //
        public void OnPointerClick()
        {
            SunEventManager.EmitEvent(EventID.Teleport_OnTeleportClicked, sender: TeleportID);
            SunEventManager.EmitEvent(EventID.Teleport_OnTeleportPlayer, sender: TelePoint);
            _isDisable = true;
            Debug.Log($"Teleport player and set status of Teleport {TeleportID} point.");
        }

        //
        public void OnPointerExit()
        {
            if (_isDisable) return;
            _tweenColorTeleport?.Kill();
            _tweenColorTeleport = TeleRenderer.DOColor(NormalColor, .25f);
        }

        #endregion
    }
}