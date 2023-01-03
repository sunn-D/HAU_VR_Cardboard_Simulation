using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerSmoothFade : MonoBehaviour
    {
        #region Variables

        //
        [FoldoutGroup("Variables")]
        [SerializeField, Range(0f, 1f)] private float alpha;
        [FoldoutGroup("Variables")] 
        [SerializeField] private SpriteRenderer rendererFade;

        //
        public float Alpha
        {
            get => alpha;
            set => alpha = value;
        }
        public SpriteRenderer RendererFade
        {
            get => rendererFade;
            set => rendererFade = value;
        }

        #endregion

        #region Functions

        //
        private void OnValidate()
        {
            if (RendererFade == null) RendererFade = transform.Find("Renderer").GetComponent<SpriteRenderer>();
            var currentColor = RendererFade.color;
            currentColor.a = alpha;
            RendererFade.color = currentColor;
        }

        //
        public void SetAlpha(float newAlpha)
        {
            Alpha = newAlpha;
            var currentColor = RendererFade.color;
            currentColor.a = Alpha;
            RendererFade.color = currentColor;
        }

        #endregion
    }
}