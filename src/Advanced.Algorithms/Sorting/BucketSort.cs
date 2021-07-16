using System;
using System.Collections.Generic;
using System.Linq;

namespace Advanced.Algorithms.Sorting
{

    /// <summary>
    /// A bucket sort implementation.
    /// </summary>
    public class BucketSort
    {
        /// <summary>
        /// Sort given integers using bucket sort with merge sort as sub sort.
        /// </summary>
        public static int[] Sort(Indexable<int> array, int bucketSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            return Sort(array, bucketSize, Comparer<int>.Default, sortDirection);
        }

        /// <summary>
        /// Sort given integers using bucket sort with merge sort as sub sort.
        /// </summary>
        public static int[] Sort(Indexable<int> array, int bucketSize, IComparer<int> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (bucketSize < 0 || bucketSize > array.Length)
            {
                throw new Exception("Invalid bucket size.");
            }

            var buckets = new Dictionary<int, List<int>>();

            int i;
            for (i = 0; i < array.Length; i++)
            {
                if (bucketSize == 0)
                {
                    continue;
                }

                var bucketIndex = array[i] / bucketSize;

                if (!buckets.ContainsKey(bucketIndex))
                {
                    buckets.Add(bucketIndex, new List<int>());
                }

                buckets[bucketIndex].Add(array[i]);
            }

            i = 0;
            var bucketKeys = new Indexable<int>(new int[buckets.Count]);
            foreach (var bucket in buckets.ToList())
            {
                var list = new List<int>(bucket.Value);
                MergeSort<int>.Sort(bucket.Value, order, sortDirection);
                buckets[bucket.Key] = list;

                bucketKeys[i] = bucket.Key;
                i++;
            }

            bucketKeys = MergeSort<int>.Sort(bucketKeys, order, sortDirection);

            var result = new int[array.Length];

            i = 0;
            foreach (var bucketKey in bucketKeys)
            {
                var bucket = buckets[bucketKey];
                Array.Copy(bucket.ToArray(), 0, result, i, bucket.Count);
                i += bucket.Count;
            }

            return result;
        }
    }
}
