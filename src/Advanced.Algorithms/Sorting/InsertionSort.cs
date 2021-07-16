using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.Sorting
{
    public class InsertionSort
    {
        /// <summary>
        /// Time complexity: O(n^2).
        /// </summary>
        public static Indexable<T> Sort<T>(Span<T> array, SortDirection sortDirection = SortDirection.Ascending)
            where T : IComparable<T>
        {
            return InsertionSort<T>.Sort(array, Comparer<T>.Default, sortDirection);
        }
    }

    /// <summary>
    /// An insertion sort implementation.
    /// </summary>
    public class InsertionSort<T>
    {
        /// <summary>
        /// Time complexity: O(n^2).
        /// </summary>
        public static Indexable<T> Sort(Indexable<T> array, IComparer<T> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            var comparer = new CustomComparer<T>(sortDirection, order);

            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (comparer.Compare(array[j], array[j - 1]) < 0)
                    {
                        var temp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = temp;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return array;
        }
    }
}
