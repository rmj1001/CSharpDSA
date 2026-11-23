namespace DataStructures
{
    public static class Comparer<T>
    {
        /// <summary>
        /// Helper function to determine if a generic is equal to another.
        /// </summary>
        /// <param name="first">First item to compare</param>
        /// <param name="second">Second item to compare</param>
        /// <returns>true if items are equal, false otherwise</returns>
        public static bool EQ(T first, T second)
        {
            return EqualityComparer<T>.Default.Equals(first, second);
        }

        /// <summary>
        /// Compare two objects and determine if one is greater than
        /// the other.
        /// </summary>
        /// <param name="first">The first item</param>
        /// <param name="second">The second item</param>
        /// <returns>true if first > second, otherwise false</returns>
        public static bool GT(T first, T second)
        {
            return System.Collections.Generic.Comparer<T>.Default.Compare(first, second) > 0;
        }

        /// <summary>
        /// Compare two objects and determine if one is greater than
        /// or equal to the other.
        /// </summary>
        /// <param name="first">The first item</param>
        /// <param name="second">The second item</param>
        /// <returns>true if first >= second, otherwise false</returns>
        public static bool GTE(T first, T second)
        {
            return GT(first, second) || EQ(first, second);
        }

        /// <summary>
        /// Compare two objects and determine if one is less than
        /// the other.
        /// </summary>
        /// <param name="first">The first item</param>
        /// <param name="second">The second item</param>
        /// <returns>true if first < second, otherwise false</returns>
        public static bool LT(T first, T second)
        {
            return System.Collections.Generic.Comparer<T>.Default.Compare(first, second) < 0;
        }

        /// <summary>
        /// Compare two objects and determine if one is less than
        /// or equal to the other.
        /// </summary>
        /// <param name="first">The first item</param>
        /// <param name="second">The second item</param>
        /// <returns>true if first <= second, otherwise false</returns>
        public static bool LTE(T first, T second)
        {
            return LT(first, second) || EQ(first, second);
        }

        /// <summary>
        /// Helper function to determine if a generic is not equal to another.
        /// </summary>
        /// <param name="first">First item to compare</param>
        /// <param name="second">Second item to compare</param>
        /// <returns>true if first != second, false otherwise</returns>
        public static bool NE(T first, T second)
        {
            return !EQ(first, second);
        }
    }
}
