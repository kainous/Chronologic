using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chronologic {
    public static class EnumerableUtilities {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(T,T)> WithLag<T>(this IEnumerable<T> items, T initialPairValue) {
            using var e = items.GetEnumerator();
            if (e.MoveNext()) {
                var prev = e.Current;
                yield return (initialPairValue, prev);
                while (e.MoveNext()) {
                    var next = e.Current;
                    yield return (prev, next);
                    prev = next;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Scan<T> (this IEnumerable<T> items, Func<T,T,T> operation) {
            using var e = items.GetEnumerator();
            if (e.MoveNext()) {
                var prev = e.Current;
                yield return prev;
                while (e.MoveNext()) {
                    var next = e.Current;
                    yield return operation(prev, next);
                    prev = next;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> LossyTruncation(this IEnumerable<long> values, int lossiness) =>
            values.Select(v => v >> lossiness);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> UnLossyTruncation(this IEnumerable<long> values, int lossiness) =>
            values.Select(v => v << lossiness);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaSubtraction(this IEnumerable<long> values) =>
            values.Scan((prev, next) => next - prev);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaAddition(this IEnumerable<long> values) =>
            values.Scan((prev, next) => next + prev);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaAddition(this IEnumerable<long> values) =>
            values.DeltaAddition().DeltaAddition();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaSubtraction(this IEnumerable<long> values) =>
            values.DeltaSubtraction().DeltaSubtraction();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaXor(this IEnumerable<long> values) =>
            values.Scan((prev, next) => next ^ prev);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaXor(this IEnumerable<long> values) =>
            values.DeltaXor().DeltaXor();
    }
}
