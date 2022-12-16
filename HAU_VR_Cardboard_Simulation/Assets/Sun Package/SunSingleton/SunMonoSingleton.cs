using Sirenix.OdinInspector;
using UnityEngine;

namespace Sun_Package
{
    public abstract class SunMonoSingleton<T> : MonoBehaviour where T : SunMonoSingleton<T>
    {
        #region Variables

        //
        [FoldoutGroup("Load")]
        [SerializeField] private bool dontDestroyOnLoad;

        //
        public static T Instance { get; private set; }

        #endregion

        #region Functions

        //
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                LoadInAwake();
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //
        protected virtual void LoadInAwake() { }

        #endregion
    }
}