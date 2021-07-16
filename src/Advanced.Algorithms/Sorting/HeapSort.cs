using Advanced.Algorithms.DataStructures;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Advanced.Algorithms.Sorting
{
    public class HeapSort
    {
        /// <summary>
        /// Time complexity: O(nlog(n)).
        /// </summary>
        public static T[] Sort<T>(IEnumerable<T> collection, SortDirection sortDirection = SortDirection.Ascending)
            where T : IComparable<T>
        {
            return HeapSort<T>.Sort(collection, Comparer<T>.Default, sortDirection);
        }
    }

    /// <summary>
    /// A heap sort implementation.
    /// </summary>
    public class HeapSort<T>
    {
        /// <summary>
        /// Time complexity: O(nlog(n)).
        /// </summary>
        public static T[] Sort(IEnumerable<T> collection, IComparer<T> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            //heapify
            var heap = new BHeap<T>(sortDirection, collection, order);

            //now extract min until empty and return them as sorted array
            var sortedArray = collection.ToArray();
            var j = 0;
            while (heap.Count > 0)
            {
                sortedArray[j] = heap.Extract();
                j++;
            }

            return sortedArray;
        }
    }
}
