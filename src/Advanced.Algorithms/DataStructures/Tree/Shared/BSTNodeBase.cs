using System;

namespace Advanced.Algorithms.DataStructures
{
    public abstract class BSTNodeBase<T> where T : IComparable<T>
    {
        //Count of nodes under this node including this node.
        //Used to fasten kth smallest computation.
        public int Count { get; set; } = 1;

        public virtual BSTNodeBase<T> Parent { get; set; }

        public virtual BSTNodeBase<T> Left { get; set; }
        public virtual BSTNodeBase<T> Right { get; set; }

        public T Value { get; set; }

        public bool IsLeftChild => Parent.Left == this;
        public bool IsRightChild => Parent.Right == this;

        public bool IsLeaf => Left == null && Right == null;

    }
}
