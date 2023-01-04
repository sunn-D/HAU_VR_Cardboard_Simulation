using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.Player_Controller
{
    public class PlayerTextLogging : MonoBehaviour
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField, TextArea] public string TextLogging {get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public TextMeshPro TextMesh {get; set; }

        #endregion

        #region Functions

        //
        private void OnValidate()
        {
            if (TextMesh == null) TextMesh = transform.Find("TextMesh").GetComponent<TextMeshPro>();
            TextMesh.text = TextLogging;
        }
        
        //
        private void Start()
        {
            if (TextMesh == null) TextMesh = transform.Find("TextMesh").GetComponent<TextMeshPro>();
        }

        //
        public void SetTextLogging(string text)
        {
            TextLogging = text;
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