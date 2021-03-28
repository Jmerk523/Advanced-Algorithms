using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced.Algorithms.Sorting
{
    public readonly ref struct Indexable<T>
    {
        private readonly IList<T> list;
        private readonly Span<T> span;
        private readonly ReadOnlySpan<T> roSpan;

        public bool IsReadOnly => list == null && span.IsEmpty;

        public int Length => list?.Count ?? span.Length;

        public T this[int index]
        {
            get
            {
                if (list == null)
                {
                    return roSpan[index];
                }
                else
                {
                    return list[index];
                }
            }
            set
            {
                if (list == null)
                {
                    span[index] = value;
                }
                else
                {
                    list[index] = value;
                }
            }
        }

        public Indexable(ReadOnlySpan<T> span)
        {
            this.span = default;
            roSpan = span;
            list = null;
        }

        public Indexable(Span<T> span)
        {
            this.span = span;
            roSpan = span;
            list = null;
        }

        public Indexable(List<T> list)
        {
            span = default;
            roSpan = default;
            this.list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public bool IsSpan(out Span<T> span, out IList<T> list)
        {
            span = this.span;
            list = this.list;
            return list == null;
        }

        public SpanEnumerator<T> GetEnumerator() => new SpanEnumerator<T>(this);

        public static implicit operator Indexable<T>(List<T> list) => new Indexable<T>(list);
        public static implicit operator Indexable<T>(Span<T> span) => new Indexable<T>(span);
        public static implicit operator Indexable<T>(ReadOnlySpan<T> span) => new Indexable<T>(span);
        public static implicit operator Indexable<T>(T[] array) => new Indexable<T>(array);
    }

    public ref struct SpanEnumerator<T>
    {
        private readonly Indexable<T> span;

        private int index;

        public T Current => span[index];

        public SpanEnumerator(Indexable<T> span)
        {
            this.span = span;
            index = -1;
        }

        public void Dispose()
        {
            index = span.Length;
        }

        public bool MoveNext()
        {
            if (++index < span.Length)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            index = -1;
        }
    }

    public static class SpanExtensions
    {
        public static ReadOnlySpan<T> AsReadOnly<T>(this Span<T> span) => span;

        public static int Max(this ReadOnlySpan<int> span)
        {
            int max = int.MinValue;
            foreach (var element in span)
            {
                max = Math.Max(max, element);
            }
            return max;
        }

        public static void ForEach<T>(this ReadOnlySpan<T> span, Action<T> action)
        {
            foreach (var element in span)
            {
                action(element);
            }
        }

        public static SpanEnumerator<T> GetEnumerator<T>(this ReadOnlySpan<T> span)
        {
            return new SpanEnumerator<T>(span);
        }
    }
}
