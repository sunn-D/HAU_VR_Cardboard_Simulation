using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Sun_Package
{
    public class SunMaskTextMesh : MonoBehaviour
    {
        #region Variables

        //
        [FoldoutGroup("Variables")]
        [SerializeField] private float maxWidth = -1;
        [FoldoutGroup("Variables")]
        [SerializeField] private TextMeshProUGUI text;
        [FoldoutGroup("Variables")]
        [SerializeField] private RectTransform textRect;
        [FoldoutGroup("Variables")]
        [SerializeField] private bool playAfterInitialize;
        
        //
        [FoldoutGroup("Animation")] 
        [SerializeField] private Ease ease = Ease.Linear;
        [FoldoutGroup("Animation")] 
        [SerializeField] private LoopType loopType = LoopType.Yoyo;
        [FoldoutGroup("Animation")] 
        [SerializeField] private float startDelay = 1f;
        [FoldoutGroup("Animation")] 
        [SerializeField] private float intervalDelay = .75f;

        //
        private Sequence _textSequence;

        #endregion

        #region Variables

        //
        private void Reset()
        {
            textRect = transform.GetChild(0).GetComponent<RectTransform>();
            text = transform.GetComponentInChildren<TextMeshProUGUI>();
            maxWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
        }

        //
        private void OnEnable()
        {
            _textSequence?.Play();
        }

        //
        private void OnDisable()
        {
            _textSequence?.Pause();
        }
        
        //
        public void Initialize(string textToShow)
        {
            UpdateTextSequence(textToShow);
            if (!playAfterInitialize) StopTextSequence();
            else PlayTextSequence();
        }
        
        //
        public void PlayTextSequence()
        {
            _textSequence?.Play();
        }

        //
        public void StopTextSequence()
        {
            _textSequence?.Pause();
        }

        //
        public void UpdateTextSequence(string textToShow)
        {
            text.text = textToShow;
            text.ForceMeshUpdate();
            var trueWidth = text.bounds.size.x;
            if (trueWidth > maxWidth)
            {
                var endPos = new Vector2(maxWidth - trueWidth, 0f);
                var duration = (trueWidth - maxWidth) < 100f ? (trueWidth - maxWidth) / 25f : (trueWidth - maxWidth) / 100f;
                _textSequence = DOTween.Sequence();
                _textSequence.SetDelay(startDelay);
                _textSequence.Append(textRect.DOAnchorPos(endPos, duration).SetRelative().SetEase(ease));
                _textSequence.AppendInterval(intervalDelay);
                _textSequence.SetLoops(-1, loopType);
            }
        }

        //
        public void DestroySequence()
        {
            _textSequence.Kill(true);
        }

        #endregion
    }
}