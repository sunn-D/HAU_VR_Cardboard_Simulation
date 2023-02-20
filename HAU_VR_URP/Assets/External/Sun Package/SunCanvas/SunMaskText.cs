using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public class SunMaskText : MonoBehaviour
    {
        #region Variables
     
        //
        public enum InitializeIn { None, Awake, Start }
        
        //
        [field: FoldoutGroup("Variables"), SerializeField] public TextMeshProUGUI TextMesh { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public RectTransform TextRect { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public InitializeIn InitializeInAction { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public bool PlayAfterInitialize { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public float MaxWidth { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public string DefaultText { get; set; }
        //
        [field: FoldoutGroup("Animation"), SerializeField] public Ease Ease { get; set; }
        [field: FoldoutGroup("Animation"), SerializeField] public LoopType LoopType { get; set; }
        [field: FoldoutGroup("Animation"), SerializeField] public float DelayStart { get; set; }
        [field: FoldoutGroup("Animation"), SerializeField] public float DelayInterval { get; set; }
        
        //
        private Sequence _textSequence;

        #endregion
        
        #region Functions

        //
        private void Reset()
        {
            var childCount = transform.childCount;
            if (childCount == 1)
            {
                TextRect = transform.GetChild(0).GetComponent<RectTransform>();
                TextMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                InitializeInAction = InitializeIn.None;
                PlayAfterInitialize = false;
                MaxWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
                DefaultText = "Default Text";
                Ease = Ease.Linear;
                LoopType = LoopType.Yoyo;
                DelayStart = 1f;
                DelayInterval = .75f;
            }
            else
            {
                Debug.LogWarning("Check mask textmesh.");
            }
        }

        //
        protected virtual void Awake()
        {
            if (InitializeInAction == InitializeIn.Awake) Initialize();  
        }
        
        //
        protected virtual void Start()
        {
            if (InitializeInAction == InitializeIn.Start) Initialize();
        }
        
        //
        protected virtual void Initialize()
        {
            UpdateText(DefaultText);
            if (PlayAfterInitialize) Play();
            else Stop();
        }
        
        //
        public void UpdateText(string text)
        {
            TextMesh.alignment = TextAlignmentOptions.MidlineLeft;
            TextRect.anchoredPosition = Vector2.zero;
            TextMesh.text = text;
            TextMesh.ForceMeshUpdate();
            var trueWidth = TextMesh.bounds.size.x;
            if (trueWidth > MaxWidth)
            {
                var endPos = new Vector2(MaxWidth - trueWidth, 0f);
                var duration = (trueWidth - MaxWidth) < 100f ? (trueWidth - MaxWidth) / 25f : (trueWidth - MaxWidth) / 100f;
                _textSequence?.Kill(true);
                TextRect.anchoredPosition = Vector2.zero;
                _textSequence = DOTween.Sequence();
                _textSequence.SetDelay(DelayStart);
                _textSequence.Append(TextRect.DOAnchorPos(endPos, duration).SetRelative().SetEase(Ease));
                _textSequence.AppendInterval(DelayInterval);
                _textSequence.SetLoops(-1, LoopType);
            }
            else
            {
                TextMesh.alignment = TextAlignmentOptions.Midline;
                _textSequence = null;
            }
        }

        //
        public void UpdateTextWithoutTween(string text)
        {
            TextMesh.text = text;
            TextMesh.ForceMeshUpdate();
        }
        
        //
        public void Play() => _textSequence?.Play();
        
        //
        public void Stop() => _textSequence?.Pause();
        
        //
        public void Kill() => _textSequence?.Kill(true);

        #endregion
    }
}