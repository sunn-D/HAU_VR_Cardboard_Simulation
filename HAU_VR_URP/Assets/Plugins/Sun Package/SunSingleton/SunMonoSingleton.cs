using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public abstract class SunMonoSingleton<T> : MonoBehaviour where T : SunMonoSingleton<T>
    {
        #region Variables

        //
        [FoldoutGroup("Config")] 
        [SerializeField] private bool dontDestroyOnLoad;

        //
        private bool _isInitialized;

        //
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        Debug.LogWarning("No instance of " + typeof(T) + ", a temporary one is created.");

                        _instance = new GameObject("Temp Instance of " + typeof(T)).GetOrAddComponent<T>();

                        if (_instance == null)
                        {
                            Debug.LogError("Problem during the creation of " + typeof(T));
                        }
                    }

                    if (!_instance._isInitialized)
                    {
                        if (_instance.dontDestroyOnLoad) DontDestroyOnLoad(_instance.gameObject);
                        _instance._isInitialized = true;
                        _instance.Init();
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
                if (!_isInitialized)
                {
                    if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
                    _isInitialized = true;
                    Init();
                    LoadInAwake();
                }
            }
            else if (_instance != this)
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
        protected virtual void Init() { }

        //
        protected virtual void LoadInAwake() { }
        
        //
        protected virtual void LoadInStart() { }

        #endregion
    }
}