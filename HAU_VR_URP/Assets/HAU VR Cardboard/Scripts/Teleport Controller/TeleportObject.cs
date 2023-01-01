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
        [FoldoutGroup("Variables")]
        [SerializeField] private int teleportID;
        [FoldoutGroup("Variables")]
        [SerializeField] private bool isFirstTeleport;
        [FoldoutGroup("Variables")]
        [SerializeField] private BoxCollider teleCollider;
        [FoldoutGroup("Variables")]
        [SerializeField] private SpriteRenderer teleRenderer;
        [FoldoutGroup("Variables")]
        [SerializeField] private Vector3 telePoint;

        //
        public int TeleportID
        {
            get => teleportID;
            set => teleportID = value;
        }
        public bool IsFirstTeleport
        {
            get => isFirstTeleport;
            set => isFirstTeleport = value;
        }
        public BoxCollider TeleCollider
        {
            get => teleCollider;
            set => teleCollider = value;
        }
        public SpriteRenderer TeleRenderer
        {
            get => teleRenderer;
            set => teleRenderer = value;
        }
        public Vector3 TelePoint
        {
            get => telePoint;
            set => telePoint = value;
        }
        
        //
        private Sequence _tweenTeleport;
        private Tweener _tweenColor;
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
            TeleCollider = GetComponent<BoxCollider>();
            TeleRenderer = GetComponent<SpriteRenderer>();
            TelePoint = transform.position;
        }
        
        //
        public void Initialize()
        {
            _tweenTeleport = DOTween.Sequence();
            _tweenTeleport.Append(transform.DOMoveY(1.25f, .75f).SetRelative().SetEase(Ease.Linear));
            _tweenTeleport.AppendInterval(.5f);
            _tweenTeleport.SetLoops(-1, LoopType.Yoyo);
            _tweenTeleport.Play();
        }
        
        //
        public void EnableTeleport()
        {
            TeleRenderer.color = NormalColor;
            TeleCollider.enabled = true;
            _tweenTeleport?.Play();
            _isDisable = false;
        }
        
        //
        public void DisableTeleport()
        {
            _tweenColor?.Kill();
            _tweenTeleport?.Pause();
            TeleRenderer.color = TransparentColor;
            TeleCollider.enabled = false;
        }

        #endregion
        
        #region Implement Interface

        //
        public void OnPointerEnter()
        {
            _tweenColor?.Kill();
            _tweenColor = TeleRenderer.DOColor(PointEnterColor, .25f);
        }

        //
        public void OnPointerClick()
        {
            SunEventManager.EmitEvent(EventID.Teleport_OnTeleportClicked, sender: TeleportID);
            SunEventManager.EmitEvent(EventID.Teleport_OnTeleportPlayer, sender: TelePoint);
            _isDisable = true;
            Debug.Log("Teleport player and set status of this teleport point.");
        }

        //
        public void OnPointerExit()
        {
            if (_isDisable) return;
            _tweenColor?.Kill();
            _tweenColor = TeleRenderer.DOColor(NormalColor, .25f);
        }

        #endregion
    }
}