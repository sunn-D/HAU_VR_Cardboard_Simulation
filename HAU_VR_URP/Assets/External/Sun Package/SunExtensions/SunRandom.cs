using System;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunRandom
    {
        //
        private static System.Random rnd = new System.Random();
        
        //
        public static int Range(int a, int b)
        {
            return rnd.Next(a, b);
        }

        //
        public static float Range(float a, float b)
        {
            return Mathf.Lerp(a, b, (float) rnd.NextDouble());
        }
        
        //
        public static bool Boolean()
        {
            return rnd.Next(0, 2) == 1;
        }
        
        //
        public static int RandomWithoutValue(int min, int max, int value)
        {
            int returnValue;

            do
            {
                returnValue = Range(min, max);
            } while (returnValue == value);
            
            return returnValue;
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