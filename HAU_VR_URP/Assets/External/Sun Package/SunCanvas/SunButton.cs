using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public abstract class SunButton : MonoBehaviour
    {
        #region Variables
     
        //
        public enum InitializeIn { None, Awake, Start }
        
        //
        [field: FoldoutGroup("Variables"), SerializeField] public InitializeIn InitInAction { get; set; }
        [field: FoldoutGroup("Variables"), SerializeField] public float DelayTimeClick { get; set; } = .5f;
        [field: FoldoutGroup("Variables"), SerializeField] public Button MainButton { get; set; }
        
        //
        public bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > _lastTimeClick + DelayTimeClick;
                if (result) _lastTimeClick = Time.unscaledTime;
                return result;
            }
        }
        
        //
        private float _lastTimeClick;

        #endregion

        #region Functions

        //
        protected virtual void Reset()
        {
            MainButton = GetComponent<Button>();
            InitInAction = InitializeIn.None;
        }

        //
        protected virtual void Awake()
        {
            if (InitInAction == InitializeIn.Awake) Initialize();   
        }
        
        //
        protected virtual void Start()
        {
            if (InitInAction == InitializeIn.Start) Initialize();
        }
        
        //
        public virtual void Initialize()
        {
            
        }

        #endregion
    }
}