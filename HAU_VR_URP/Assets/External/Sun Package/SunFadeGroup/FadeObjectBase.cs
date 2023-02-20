using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public abstract class FadeObjectBase : MonoBehaviour
    {
        #region Variables

        //
        public float StartAlphaSelf { get; set; }
        public Color StartColor { get; set; }
        
        //
        private bool _isFirstGetRenderOnEditor;

        #endregion

        #region Functions

        //
        [Button]
        public virtual void ResetOnEditor()
        {
            _isFirstGetRenderOnEditor = false;
        }
        
        //
        public virtual void UpdateAlphaOnEditor(float value)
        {
            if (!_isFirstGetRenderOnEditor)
            {
                _isFirstGetRenderOnEditor = true;
                GetRenderObject();
            }
            UpdateAlpha(value);
        }
        
        //
        public abstract void GetRenderObject();
        public abstract void UpdateAlpha(float value);

        #endregion
    }
}