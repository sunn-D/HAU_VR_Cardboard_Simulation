using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sun_Package
{
    public static class SunExtensions
    {
        //
        public static TComponent GetOrAddComponent<TComponent>(this GameObject target) where TComponent : Component
        {
            if (target == null)
            {
                Debug.LogWarning("Target gameobject must not be null.");
            }

            var comp = target.GetComponent<TComponent>();
            if (comp == null)
            {
                comp = target.AddComponent<TComponent>();
            }

            return comp;
        }

        //
        public static void ShuffleList<T>(this List<T> list)
        {
            var rng = new System.Random();
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        //
        public static int TryParseInt(this string s)
        {
            int.TryParse(s, out var result);
            return result;
        }
        
        //
        public static float TryParseFloat(this string s)
        {
            float.TryParse(s, out var result);
            return result;
        }
        
        //
        public static double TryParseDouble(this string s)
        {
            double.TryParse(s, out var result);
            return result;
        }

        //
        public static T GetRandomEnumValue<T>()
        {
            var value = Enum.GetValues(typeof(T));
            var randomIndex = UnityEngine.Random.Range(0, value.Length);
            return (T) value.GetValue(randomIndex);
        }
    }
}