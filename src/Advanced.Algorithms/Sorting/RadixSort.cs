using System;
using System.Collections.Generic;
using System.Linq;

namespace Advanced.Algorithms.Sorting
{
    /// <summary>
    /// A radix sort implementation.
    /// </summary>
    public class RadixSort
    {
        private static int Max(in Indexable<int> array)
        {
            int max = int.MinValue;
            foreach (var element in array)
            {
                max = Math.Max(max, element);
            }
            return max;
        }

        public static Indexable<int> Sort(Indexable<int> array, SortDirection sortDirection = SortDirection.Ascending)
        {
            return Sort(array, Comparer<int>.Default, sortDirection);
        }

        public static Indexable<int> Sort(Indexable<int> array, IComparer<int> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            int i;
            for (i = 0; i < array.Length; i++)
            {
                if (array[i] < 0)
                {
                    throw new NotSupportedException("Negative numbers not supported.");
                }
            }

            var @base = 1;
            var max = Max(array);

            while (max / @base > 0)
            {
                //create a bucket for digits 0 to 9
                var buckets = new List<int>[10];

                for (i = 0; i < array.Length; i++)
                {
                    var bucketIndex = array[i] / @base % 10;

                    if (buckets[bucketIndex] == null)
                    {
                        buckets[bucketIndex] = new List<int>();
                    }

                    buckets[bucketIndex].Add(array[i]);
                }

                //now update array with what is in buckets
                var orderedBuckets = MergeSort<(int, List<int>)>.Sort(buckets.Select((b, i) => (i, b)).ToArray(),
                    new CustomComparer<(int, List<int>), int>(t => t.Item1, sortDirection, order));

                i = 0;
                foreach (var bucket in orderedBuckets)
                {
                    if (bucket.Item2 != null)
                    {
                        foreach (var item in bucket.Item2)
                        {
                            array[i] = item;
                            i++;
                        }
                    }
                }

                @base *= 10;
            }

            return array;
        }

    }
}
