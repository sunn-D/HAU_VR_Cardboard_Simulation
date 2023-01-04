using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerSmoothFade : MonoBehaviour
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")]
        [field: SerializeField, Range(0f, 1f)] public float Alpha { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public SpriteRenderer RendererFade { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public float FadeDuration { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public bool FadeOnStart { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public bool ColorOnStart { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool EnableBeforePlay { get; set; }
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool DisableAfterPlay { get; set; }
        
        //
        public Action OnStartAction { get; set; }
        public Action OnFinishAction { get; set; }

        #endregion

        #region Functions

        //
        private void OnValidate()
        {
            if (RendererFade == null) RendererFade = transform.Find("Renderer").GetComponent<SpriteRenderer>();
            RendererFade.color = new Color(RendererFade.color.r, RendererFade.color.g, RendererFade.color.b, Alpha);
        }
        
        //
        private void Start()
        {
            if (RendererFade == null) RendererFade = transform.Find("Renderer").GetComponent<SpriteRenderer>();
            if (ColorOnStart)
            {
                Alpha = 1f;
                SetAlpha(1f);
            }
            if (FadeOnStart) FadeIn();
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
            var currentAlpha = alphaIn;
                
            DOTween.To(() => currentAlpha, x => currentAlpha = x, alphaOut, FadeDuration)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    if (EnableBeforePlay) RendererFade.gameObject.SetActive(true);
                    var startAction = OnStartAction;
                    OnStartAction = () => { };
                    startAction?.Invoke();
                })
                .OnUpdate(() => SetAlpha(currentAlpha))
                .OnComplete(() =>
                {
                    var finishAction = OnFinishAction;
                    OnFinishAction = () => { };
                    finishAction?.Invoke();
                    if (DisableAfterPlay) RendererFade.gameObject.SetActive(false);
                });
        }
        
        //
        public void SetAlpha(float newAlpha)
        {
            Alpha = newAlpha;
            RendererFade.color = new Color(RendererFade.color.r, RendererFade.color.g, RendererFade.color.b, newAlpha);
        }

        #endregion
    }
}