using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.Sorting
{
    public class BubbleSort
    {
        /// <summary>
        /// Time complexity: O(n^2).
        /// </summary>
        public static Indexable<T> Sort<T>(Indexable<T> array, SortDirection sortDirection = SortDirection.Ascending)
            where T : IComparable<T>
        {
            return BubbleSort<T>.Sort(array, Comparer<T>.Default, sortDirection);
        }
    }

    /// <summary>
    /// A bubble sort implementation.
    /// </summary>
    public class BubbleSort<T>
    {
        /// <summary>
        /// Time complexity: O(n^2).
        /// </summary>
        public static Indexable<T> Sort(Indexable<T> array, IComparer<T> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            var comparer = new CustomComparer<T>(sortDirection, order);
            var swapped = true;

            while (swapped)
            {
                swapped = false;

                for (int i = 0; i < array.Length - 1; i++)
                {
                    //compare adjacent elements 
                    if (comparer.Compare(array[i], array[i + 1]) > 0)
                    {
                        var temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                        swapped = true;
                    }
                }
            }

            return array;
        }
    }
}
