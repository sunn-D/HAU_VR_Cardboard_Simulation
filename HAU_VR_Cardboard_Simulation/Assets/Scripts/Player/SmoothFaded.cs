using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class SmoothFaded : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Configs")]
        [SerializeField] private float fadeDuration = .5f;
        [FoldoutGroup("Variables/Configs")]
        [SerializeField] private Color fadeColor = new Color(.6f, .6f, .6f, 0f);
        [FoldoutGroup("Variables/Configs")]
        [SerializeField] private bool fadeOnStart;
        [FoldoutGroup("Variables/Configs")]
        [SerializeField] private bool colorOnStart;
        [FoldoutGroup("Variables/Configs")]
        [SerializeField] private bool enableBeforePlay;
        [FoldoutGroup("Variables/Configs")]
        [SerializeField] private bool disableAfterPlay;
        
        //
        public Action OnStartAction { get; set; }
        public Action OnFinishAction { get; set; }

        //
        public float FadeDuration => fadeDuration;

        //
        private Renderer _renderer;
        private static readonly int ColorMaterialField = Shader.PropertyToID("_Color");

        #endregion

        #region Functions

        //
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (colorOnStart)
            {
                var color = fadeColor;
                color.a = 1f;
                _renderer.material.SetColor(ColorMaterialField, color);
            }
            if (fadeOnStart) FadeIn();
        }
        
        //
        public void FadeIn()
        {
            Fade(1f, 0f);
        }
        
        //
        public void FadeOut()
        {
            Fade(0f, 1f);   
        }

        //
        public void Fade(float alphaIn, float alphaOut)
        {
            var newColor = fadeColor;
            newColor.a = alphaIn;
            _renderer.material.SetColor(ColorMaterialField, newColor);
            
            if (enableBeforePlay) gameObject.SetActive(true);
            
            var currentAlpha = alphaIn;
                
            DOTween.To(() => currentAlpha, x => currentAlpha = x, alphaOut, fadeDuration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    newColor = fadeColor;
                    newColor.a = currentAlpha;
                    _renderer.material.SetColor(ColorMaterialField, newColor);
                })
                .OnComplete(() =>
                {
                    var finishAction = OnFinishAction;
                    OnFinishAction = () => { };
                    finishAction?.Invoke();
                    if (disableAfterPlay) gameObject.SetActive(false);
                });
        }

        #endregion
    }
}