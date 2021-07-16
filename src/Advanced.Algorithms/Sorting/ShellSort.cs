using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.Sorting
{
    public class ShellSort
    {
        public static Indexable<T> Sort<T>(Indexable<T> array, SortDirection sortDirection = SortDirection.Ascending)
             where T : IComparable<T>
        {
            return ShellSort<T>.Sort(array, Comparer<T>.Default, sortDirection);
        }
    }

    /// <summary>
    /// A shell sort implementation.
    /// </summary>
    public class ShellSort<T>
    {
        public static Indexable<T> Sort(Indexable<T> array, IComparer<T> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            var comparer = new CustomComparer<T>(sortDirection, order);

            var k = array.Length / 2;
            var j = 0;

            while (k >= 1)
            {
                for (int i = k; i < array.Length; i = i + k, j = j + k)
                {
                    if (comparer.Compare(array[i], array[j]) >= 0)
                    {
                        continue;
                    }

                    swap(array, i, j);

                    if (i <= k)
                    {
                        continue;
                    }

                    i -= k * 2;
                    j -= k * 2;
                }

                j = 0;
                k /= 2;
            }

            return array;
        }

        private static void swap(Indexable<T> array, int i, int j)
        {
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}
