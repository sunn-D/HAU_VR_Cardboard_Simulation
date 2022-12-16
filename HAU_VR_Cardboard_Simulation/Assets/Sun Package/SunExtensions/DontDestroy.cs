using UnityEngine;

namespace Sun_Package
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}