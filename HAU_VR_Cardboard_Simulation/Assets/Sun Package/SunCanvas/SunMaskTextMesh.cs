using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DunnGSunn
{
    public class SunMaskTextMesh : MonoBehaviour
    {
        [FoldoutGroup("Variables")]
        public float maxWidth = -1;
        [FoldoutGroup("Variables")]
        public TextMeshProUGUI text;
        [FoldoutGroup("Variables")]
        public RectTransform textRect;
        [FoldoutGroup("Variables")]
        public bool playAfterInitialize;

        [FoldoutGroup("Animation")] 
        public Ease ease = Ease.Linear;
        [FoldoutGroup("Animation")] 
        public LoopType loopType = LoopType.Yoyo;
        [FoldoutGroup("Animation")] 
        public float startDelay = 1f;
        [FoldoutGroup("Animation")] 
        public float intervalDelay = .75f;

        //
        private Sequence _textSequence;

        private void Reset()
        {
            textRect = transform.GetChild(0).GetComponent<RectTransform>();
            text = transform.GetComponentInChildren<TextMeshProUGUI>();
            maxWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
        }

        public void Initialize(string textToShow)
        {
            UpdateTextSequence(textToShow);
            if (!playAfterInitialize) StopTextSequence();
            else PlayTextSequence();
        }

        private void OnEnable()
        {
            _textSequence?.Play();
        }

        private void OnDisable()
        {
            _textSequence?.Pause();
        }

        public void PlayTextSequence()
        {
            _textSequence?.Play();
        }

        public void StopTextSequence()
        {
            _textSequence?.Pause();
        }

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

        public void DestroySequence()
        {
            _textSequence.Kill(true);
        }
    }
}