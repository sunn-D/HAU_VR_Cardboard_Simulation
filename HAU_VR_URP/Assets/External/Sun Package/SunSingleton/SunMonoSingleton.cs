using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public abstract class SunMonoSingleton<T> : MonoBehaviour where T : SunMonoSingleton<T>
    {
        #region Variables

        //
        [field: FoldoutGroup("Variables")]
        [field: SerializeField] public bool DontDestroy { get; set; }

        //
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        var singletonObject = new GameObject($"Singleton - {typeof(T)}");
                        _instance = singletonObject.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Unity callback functions

        //
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                LoadInAwake();
                if (DontDestroy) DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("Another instance of " + GetType() + " is already exist! Destroying self...");
                DestroyImmediate(gameObject);
            }
        }
        
        //
        private void Start()
        {
            LoadInStart();
        }

        //
        protected virtual void LoadInAwake() { }
        
        //
        protected virtual void LoadInStart() { }

        #endregion
    }
}