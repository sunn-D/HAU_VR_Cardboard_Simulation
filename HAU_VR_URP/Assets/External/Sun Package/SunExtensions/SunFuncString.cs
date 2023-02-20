using UnityEngine.Networking;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunFuncString
    {
        //
        public static int ParseInt(string str)
        {
            int.TryParse(str, out var returnValue);
            return returnValue;
        }

        //
        public static float ParseFloat(string str)
        {
            float.TryParse(str, out var returnValue);
            return returnValue;
        }

        //
        public static string UnescapeString(string str)
        {
            return UnityWebRequest.UnEscapeURL(str);
        }
    }
}