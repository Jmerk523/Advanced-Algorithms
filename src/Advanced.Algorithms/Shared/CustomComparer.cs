using System;
using System.Collections.Generic;

namespace Advanced.Algorithms
{
    internal sealed class CustomComparer<T> : IComparer<T>
    {
        private readonly bool isMax;
        private readonly IComparer<T> comparer;

        internal CustomComparer(SortDirection sortDirection, IComparer<T> comparer)
        {
            this.isMax = sortDirection == SortDirection.Descending;
            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return !isMax ? compare(x, y) : compare(y, x);
        }

        private int compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }
    }

    internal sealed class CustomComparer<T, U> : IComparer<T>
    {
        private readonly bool isMax;
        private readonly IComparer<U> comparer;
        private readonly Func<T, U> select;

        internal CustomComparer(Func<T, U> select, SortDirection sortDirection, IComparer<U> comparer)
        {
            this.isMax = sortDirection == SortDirection.Descending;
            this.comparer = comparer;
            this.select = select;
        }

        public int Compare(T x, T y)
        {
            return !isMax ? compare(select(x), select(y)) : compare(select(y), select(x));
        }

        private int compare(U x, U y)
        {
            return comparer.Compare(x, y);
        }
    }
}
