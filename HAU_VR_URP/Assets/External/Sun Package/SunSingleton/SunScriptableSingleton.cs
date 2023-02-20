using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class SunScriptableSingleton<T> : ScriptableObject where T : SunScriptableSingleton<T>
    {
        //
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length < 1)
                    {
                        Debug.LogWarning("Couldn't find any scriptable in Resources!");
                    }
                    else if (assets.Length > 1)
                    {
                        Debug.LogWarning("Multiple instance of the singleton scriptable object found in Resources!");
                        _instance = assets[0];
                    }
                    else
                    {
                        _instance = assets[0];
                    }
                }

                return _instance;
            }
        }
    }
}