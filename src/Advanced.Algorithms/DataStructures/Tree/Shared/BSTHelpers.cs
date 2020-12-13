using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.DataStructures
{
    internal class BSTHelpers
    {
        internal static void ValidateSortedCollection<T>(IEnumerable<T> sortedCollection) where T : IComparable<T>
        {
            ValidateSortedCollection(sortedCollection, Comparer<T>.Default);
        }

        internal static void ValidateSortedCollection<T>(IEnumerable<T> sortedCollection, IComparer<T> comparer)
        {
            if (!isSorted(sortedCollection, comparer))
            {
                throw new ArgumentException("Initial collection should have unique keys and be in sorted order.");
            }
        }

        internal static BSTNodeBase<T> ToBST<T>(BSTNodeBase<T>[] sortedNodes)
        {
            return toBST(sortedNodes, 0, sortedNodes.Length - 1);
        }

        internal static int AssignCount<T>(BSTNodeBase<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            node.Count = AssignCount(node.Left) + AssignCount(node.Right) + 1;

            return node.Count;
        }

        private static BSTNodeBase<T> toBST<T>(BSTNodeBase<T>[] sortedNodes, int start, int end)
        {
            if (start > end)
                return null;

            int mid = (start + end) / 2;
            var root = sortedNodes[mid];

            root.Left = toBST(sortedNodes, start, mid - 1);
            if (root.Left != null)
            {
                root.Left.Parent = root;
            }

            root.Right = toBST(sortedNodes, mid + 1, end);
            if (root.Right != null)
            {
                root.Right.Parent = root;
            }

            return root;
        }

        private static bool isSorted<T>(IEnumerable<T> collection) where T : IComparable<T>
        {
            return isSorted(collection, Comparer<T>.Default);
        }

        private static bool isSorted<T>(IEnumerable<T> collection, IComparer<T> comparer)
        {
            var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return true;
            }

            var previous = enumerator.Current;

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (comparer.Compare(current, previous) <= 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
