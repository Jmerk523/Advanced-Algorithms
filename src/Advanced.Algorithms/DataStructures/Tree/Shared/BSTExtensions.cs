using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.DataStructures
{
    internal static class BSTExtensions
    {
        internal static (BSTNodeBase<T>, int) Find<T>(this BSTNodeBase<T> current, T value) where T : IComparable<T>
        {
            return Find(current, value, Comparer<T>.Default);
        }

        //find the node with the given identifier among descendants of parent and parent
        //uses pre-order traversal
        //O(log(n)) worst O(n) for unbalanced tree
        internal static (BSTNodeBase<T>, int) Find<T>(this BSTNodeBase<T> current, T value, IComparer<T> comparer)
        {
            int position = 0;

            while (true)
            {
                if (current == null)
                {
                    return (null, -1);
                }

                var compareResult = comparer.Compare(current.Value, value);

                if (compareResult == 0)
                {
                    position += (current.Left != null ? current.Left.Count : 0);
                    return (current, position);
                }

                if (compareResult > 0)
                {
                    current = current.Left;
                }
                else
                {
                    position += (current.Left != null ? current.Left.Count : 0) + 1;
                    current = current.Right;
                }
            }
        }

        internal static BSTNodeBase<T> FindMax<T>(this BSTNodeBase<T> node)
        {
            if (node == null)
            {
                return null;
            }

            while (true)
            {
                if (node.Right == null) return node;
                node = node.Right;
            }
        }

        internal static BSTNodeBase<T> FindMin<T>(this BSTNodeBase<T> node)
        {
            if (node == null)
            {
                return null;
            }

            while (true)
            {
                if (node.Left == null) return node;
                node = node.Left;
            }
        }

        internal static BSTNodeBase<T> NextLower<T>(this BSTNodeBase<T> node)
        {
            //root or left child
            if (node.Parent == null || node.IsLeftChild)
            {
                if (node.Left != null)
                {
                    node = node.Left;

                    while (node.Right != null)
                    {
                        node = node.Right;
                    }

                    return node;
                }
                else
                {
                    while (node.Parent != null && node.IsLeftChild)
                    {
                        node = node.Parent;
                    }

                    return node?.Parent;
                }
            }
            //right child
            else
            {
                if (node.Left != null)
                {
                    node = node.Left;

                    while (node.Right != null)
                    {
                        node = node.Right;
                    }

                    return node;
                }
                else
                {
                    return node.Parent;
                }
            }

        }

        internal static BSTNodeBase<T> NextHigher<T>(this BSTNodeBase<T> node)
        {
            //root or left child
            if (node.Parent == null || node.IsLeftChild)
            {
                if (node.Right != null)
                {
                    node = node.Right;

                    while (node.Left != null)
                    {
                        node = node.Left;
                    }

                    return node;
                }
                else
                {
                    return node?.Parent;
                }
            }
            //right child
            else
            {
                if (node.Right != null)
                {
                    node = node.Right;

                    while (node.Left != null)
                    {
                        node = node.Left;
                    }

                    return node;
                }
                else
                {
                    while (node.Parent != null && node.IsRightChild)
                    {
                        node = node.Parent;
                    }

                    return node?.Parent;
                }
            }
        }

        internal static void UpdateCounts<T>(this BSTNodeBase<T> node, bool spiralUp = false)
        {
            while (node != null)
            {
                int leftCount = node.Left?.Count ?? 0;
                var rightCount = node.Right?.Count ?? 0;

                node.Count = leftCount + rightCount + 1;

                node = node.Parent;

                if (!spiralUp)
                {
                    break;
                }
            }
        }

        //get the kth smallest element under given node
        internal static BSTNodeBase<T> KthSmallest<T>(this BSTNodeBase<T> node, int k)
        {
            var leftCount = node.Left != null ? node.Left.Count : 0;

            if (k == leftCount)
            {
                return node;
            }

            if (k < leftCount)
            {
                return KthSmallest(node.Left, k);
            }

            return KthSmallest(node.Right, k - leftCount - 1);
        }

        internal static int Position<T>(this BSTNodeBase<T> node, T item) where T : IComparable<T>
        {
            return Position(node, item, Comparer<T>.Default);
        }

        //get the sorted order position of given item under given node
        internal static int Position<T>(this BSTNodeBase<T> node, T item, IComparer<T> comparer)
        {
            if (node == null)
            {
                return -1;
            }

            var leftCount = node.Left != null ? node.Left.Count : 0;

            if (comparer.Compare(node.Value, item) == 0)
            {
                return leftCount;
            }

            if (comparer.Compare(item, node.Value) < 0)
            {
                return Position(node.Left, item, comparer);
            }

            var position = Position(node.Right, item, comparer);

            return position < 0 ? position : position + leftCount + 1;
        }


    }
}
