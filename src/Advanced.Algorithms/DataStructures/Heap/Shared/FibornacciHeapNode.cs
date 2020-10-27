using System;

namespace Advanced.Algorithms.DataStructures
{
    internal class FibonacciHeapNode<T> : IComparable<FibonacciHeapNode<T>> where T : IComparable<T>
    {
        internal T Value { get; set; }

        internal int Degree;
        internal FibonacciHeapNode<T> ChildrenHead { get; set; }

        internal FibonacciHeapNode<T> Parent { get; set; }
        internal bool LostChild { get; set; }

        internal FibonacciHeapNode<T> Previous;
        internal FibonacciHeapNode<T> Next;

        internal FibonacciHeapNode(T value)
        {
            Value = value;
        }

        public int CompareTo(FibonacciHeapNode<T> other)
        {
            return Value.CompareTo(other.Value);
        }
    }

}
