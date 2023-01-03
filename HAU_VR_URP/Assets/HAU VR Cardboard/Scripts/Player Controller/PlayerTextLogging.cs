using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerTextLogging : MonoBehaviour
    {
        #region Variables

        //
        [FoldoutGroup("Variables")] 
        [SerializeField, TextArea] private string textLogging;
        [FoldoutGroup("Variables")] 
        [SerializeField] private TextMeshPro textMesh;
        

        //
        public string TextLogging
        {
            get => textLogging;
            set => textLogging = value;
        }
        public TextMeshPro TextMesh
        {
            get => textMesh;
            set => textMesh = value;
        }

        #endregion

        #region Functions

        //
        private void OnValidate()
        {
            if (TextMesh == null) TextMesh = transform.Find("TextMesh").GetComponent<TextMeshPro>();
            TextMesh.text = textLogging;
        }
        
        //
        public void SetTextLogging(string text)
        {
            textLogging = text;
            TextMesh.text = text;
        }
        
        //
        public void SetActiveTextMesh(bool value)
        {
            TextMesh.gameObject.SetActive(value);
        }

        #endregion
    }
}