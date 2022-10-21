using System;
using System.Collections;
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
        public float fadeDuration;
        [FoldoutGroup("Variables/Configs")]
        public Color fadeColor;
        [FoldoutGroup("Variables/Configs")]
        public bool fadeOnStart;
        [FoldoutGroup("Variables/Configs")]
        public bool colorOnStart;
        [FoldoutGroup("Variables/Configs")]
        public bool enableBeforePlay;
        [FoldoutGroup("Variables/Configs")]
        public bool disableAfterPlay;
        
        //
        public Action OnStartAction { get; set; }
        public Action OnFinishAction { get; set; }

        //
        private Renderer _renderer;
        private static readonly int ColorMaterialField = Shader.PropertyToID("_Color");

        #endregion

        #region Functions

        //
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (fadeOnStart) FadeIn();
            if (colorOnStart)
            {
                var color = fadeColor;
                color.a = 1f;
                _renderer.material.SetColor(ColorMaterialField, color);
            }
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
            if (enableBeforePlay)
            {
                gameObject.SetActive(true);
            }
            StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
        }
        
        //
        private IEnumerator FadeCoroutine(float alphaIn, float alphaOut)
        {
            var timer = 0f;
            OnStartAction?.Invoke();

            while (timer <= fadeDuration)
            {
                var newColor = fadeColor;
                newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
                _renderer.material.SetColor(ColorMaterialField, newColor);
                timer += Time.deltaTime;
                yield return null;
            }
            
            var color = fadeColor;
            color.a = alphaOut;
            _renderer.material.SetColor(ColorMaterialField, color);
            OnFinishAction?.Invoke();
            if (disableAfterPlay)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}