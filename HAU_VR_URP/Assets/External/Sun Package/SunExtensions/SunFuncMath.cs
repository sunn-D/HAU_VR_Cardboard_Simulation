// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunFuncMath
    {
        //
        public static int GetSignInteger(float param)
        {
            return param >= 0 ? 1 : -1;
        }

        //
        public static bool IsBetween(float param, float a, float b)
        {
            return (param <= a && param >= b) || (param >= a && param <= b);
        }
    }
}