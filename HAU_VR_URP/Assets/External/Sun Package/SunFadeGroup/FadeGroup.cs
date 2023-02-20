using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    [ExecuteAlways]
    public class FadeGroup : MonoBehaviour
    {
        #region Variables

        //
        public enum InitOnType { Awake, Start }
        
        //
        [FoldoutGroup("Variables")] 
        [FoldoutGroup("Variables/Config")] 
        [SerializeField] private InitOnType initOn;
        [FoldoutGroup("Variables/Config")] 
        [SerializeField, Range(0f, 1f)] private float valueOnEditor = 1f;
        [FoldoutGroup("Variables/List fade object")]
        [SerializeField] private List<FadeObjectBase> allFadedObject;

        //
        public InitOnType InitOn
        {
            get => initOn;
            set => initOn = value;
        }
        public List<FadeObjectBase> AllFadedObject
        {
            get => allFadedObject;
            set => allFadedObject = value;
        }

        //
        public Action<float> AlphaChanged { get; set; }
        
        //
        private bool _isInitialized;

        #endregion

        #region Button

        //
        [Button] 
        public void GetAllFadeObjectInChild()
        {
            allFadedObject = GetComponentsInChildren<FadeObjectBase>().ToList();
        }
        
        //
        [Button]
        public void ResetAllValueInChild()
        {
            if (AllFadedObject == null || AllFadedObject.Count == 0)
            {
                GetAllFadeObjectInChild();
            }

            foreach (var fadeObject in AllFadedObject)
            {
                fadeObject.ResetOnEditor();
            }
        }

        #endregion

        #region Functions

        //
        private void OnValidate()
        {
            OnAlphaChangedOnEditor(valueOnEditor);
        }
        
        //
        private void Reset()
        {
            var allChildrenObject = transform.GetComponentsInChildren<Transform>();
            foreach (var childTransform in allChildrenObject)
            {
                if (childTransform.GetComponent<SpriteRenderer>())
                {
                    if (childTransform.GetComponent<FadeSpriteRenderer>() == null)
                    {
                        childTransform.gameObject.AddComponent<FadeSpriteRenderer>();
                    }
                }
                else if (childTransform.GetComponent<ParticleSystem>())
                {
                    if (childTransform.GetComponent<FadeParticleRenderer>() == null)
                    {
                        childTransform.gameObject.AddComponent<FadeParticleRenderer>();  
                    }
                }
            }
            GetAllFadeObjectInChild();
            ResetAllValueInChild();
        }
        
        //
        private void OnDestroy()
        {
            foreach (var fadeObject in AllFadedObject)
            {
                DestroyImmediate(fadeObject);
            }
        }

        //
        private void Awake()
        {
            if (InitOn == InitOnType.Awake) Initialize();
        }

        //
        private void Start()
        {
            if (InitOn == InitOnType.Start) Initialize();
        }
        
        //
        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                if (AllFadedObject == null || AllFadedObject.Count == 0)
                {
                    GetAllFadeObjectInChild();
                }
                foreach (var fadeObject in AllFadedObject)
                {
                    fadeObject.GetRenderObject();
                }
                AlphaChanged += OnAlphaChanged;
            }
        }

        //
        private void OnAlphaChanged(float value)
        {
            foreach (var fadeObject in AllFadedObject)
            {
                fadeObject.UpdateAlpha(value);
            }
        }
        
        //
        private void OnAlphaChangedOnEditor(float value)
        {
            foreach (var fadeObject in AllFadedObject)
            {
                fadeObject.UpdateAlphaOnEditor(value);
            }
        }

        #endregion
    }
}