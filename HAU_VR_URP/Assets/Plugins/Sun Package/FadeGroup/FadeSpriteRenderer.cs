using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class FadeSpriteRenderer : FadeObjectBase
    {
        #region Variables

        //
        public SpriteRenderer MainRender { get; set; }

        #endregion

        #region Functions

        //
        public override void GetRenderObject()
        {
            MainRender = GetComponent<SpriteRenderer>();
            StartColor = MainRender.color;
            StartAlphaSelf = StartColor.a;
        }

        //
        public override void UpdateAlpha(float value)
        {
            var newColor = new Color(StartColor.r, StartColor.g, StartColor.b, Mathf.Lerp(0, StartAlphaSelf, value));
            MainRender.color = newColor;
        }

        #endregion
    }
}