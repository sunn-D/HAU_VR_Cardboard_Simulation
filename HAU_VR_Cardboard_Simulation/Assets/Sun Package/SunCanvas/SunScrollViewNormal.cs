using Sirenix.OdinInspector;
using UnityEngine;

namespace Sun_Package
{
    public class SunScrollViewNormal : MonoBehaviour
    {
        #region Variables

        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Content")] 
        [SerializeField] private RectTransform topContent;
        [FoldoutGroup("Variables/Content")] 
        [SerializeField] private RectTransform bottomContent;
        [FoldoutGroup("Variables/Content")] 
        [SerializeField] private RectTransform content;
        //
        [FoldoutGroup("Variables/Info")] 
        [SerializeField] private bool initOnStart;
        [FoldoutGroup("Variables/Info")] 
        [SerializeField] private bool resetOnEnable;
        [FoldoutGroup("Variables/Info")] 
        [SerializeField] private int numberOfElementShowing;
        
        //
        public GameObject[] Elements { get; set; }
        
        //
        private bool _isInitialize;

        #endregion

        #region Functions
        
        //
        private void Start()
        {
            if (initOnStart) Initialize();
        }

        //
        private void OnEnable()
        {
            if (resetOnEnable)
            {
                content.anchoredPosition = Vector2.zero;
            }
        }

        //
        public void Initialize()
        {
            //
            Elements = new GameObject[content.childCount];
            for (var i = 0; i < content.childCount; i++)
            {
                Elements[i] = content.GetChild(i).gameObject;
            }

            //
            _isInitialize = true;
        }
        
        //
        private void Update()
        {
            if (!_isInitialize) return;
            if (Elements.Length < numberOfElementShowing) return;
         
            // Deactive elements over top/bottom
            foreach (var elementGO in Elements)
            {
                var distanceTop = topContent.position.y - elementGO.GetComponent<RectTransform>().position.y;
                var distanceBottom = bottomContent.position.y - elementGO.GetComponent<RectTransform>().position.y;
                elementGO.transform.GetChild(0).gameObject.SetActive(distanceTop >= 0f && distanceBottom <= 0f);
            }
        }
        
        #endregion
    }
}