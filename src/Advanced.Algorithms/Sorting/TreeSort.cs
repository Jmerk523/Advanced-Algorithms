using Advanced.Algorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advanced.Algorithms.Sorting
{
    public class TreeSort
    {
        public static IEnumerable<T> Sort<T>(IEnumerable<T> enumerable, SortDirection sortDirection = SortDirection.Ascending)
            where T : IComparable<T>
        {
            return TreeSort<T>.Sort(enumerable, Comparer<T>.Default, sortDirection);
        }
    }

    /// <summary>
    /// A tree sort implementation.
    /// </summary>
    public class TreeSort<T>
    {
        /// <summary>
        /// Time complexity: O(nlog(n)).
        /// </summary>
        public static IEnumerable<T> Sort(IEnumerable<T> enumerable, IComparer<T> order, SortDirection sortDirection = SortDirection.Ascending)
        {
            //create BST
            var tree = new RedBlackTree<T>(comparer: order);
            foreach (var item in enumerable)
            {
                tree.Insert(item);
            }

            return sortDirection == SortDirection.Ascending ? tree.AsEnumerable() : tree.AsEnumerableDesc();
        }
    }
}
