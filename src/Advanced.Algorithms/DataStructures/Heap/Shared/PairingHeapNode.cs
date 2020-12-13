using System;

namespace Advanced.Algorithms.DataStructures
{
    internal class PairingHeapNode<T>
    {
        internal T Value { get; set; }

        internal PairingHeapNode<T> ChildrenHead { get; set; }
        internal bool IsHeadChild => Previous != null && Previous.ChildrenHead == this;

        internal PairingHeapNode(T value)
        {
            this.Value = value;
        }

        internal PairingHeapNode<T> Previous;
        internal PairingHeapNode<T> Next;
    }

}
