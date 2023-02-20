// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunSortCondition
    {
        /// <summary>
        /// Tăng dần
        /// </summary>
        public static int SortAscending(int condition1, int condition2)
        {
            if (condition1 < condition2) return -1;
            if (condition1 > condition2) return 1;
            return 0;
        }
        
        /// <summary>
        /// Giảm dần
        /// </summary>
        public static int SortDescending(int condition1, int condition2)
        {
            if (condition1 < condition2) return 1;
            if (condition1 > condition2) return -1;
            return 0;
        }
        
        /// <summary>
        /// Tăng dần
        /// </summary>
        public static int SortAscending(float condition1, float condition2)
        {
            if (condition1 < condition2) return -1;
            if (condition1 > condition2) return 1;
            return 0;
        }
        
        /// <summary>
        /// Giảm dần
        /// </summary>
        public static int SortDescending(float condition1, float condition2)
        {
            if (condition1 < condition2) return 1;
            if (condition1 > condition2) return -1;
            return 0;
        }
    }
}