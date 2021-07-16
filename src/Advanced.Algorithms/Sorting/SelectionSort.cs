using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.Sorting
{
    public class SelectionSort
    {
        /// <summary>
        /// Time complexity: O(n^2).
        /// </summary>
        public static Indexable<T> Sort<T>(Indexable<T> array, SortDirection sortDirection = SortDirection.Ascending)
             where T : IComparable<T>
        {
            return SelectionSort<T>.Sort(array, Comparer<T>.Default, sortDirection);
        }
    }

    /// <summary>
    /// A selection sort implementation.
    /// </summary>
    public class SelectionSort<T>
    {
        /// <summary>
        /// Time complexity: O(n^2).
        /// </summary>
        public static Indexable<T> Sort(Indexable<T> array, IComparer<T> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            var comparer = new CustomComparer<T>(sortDirection, order);

            for (int i = 0; i < array.Length; i++)
            {
                //select the smallest item in sub array and move it to front
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (comparer.Compare(array[j], array[i]) >= 0)
                    {
                        continue;
                    }

                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }

            return array;
        }
    }
}
