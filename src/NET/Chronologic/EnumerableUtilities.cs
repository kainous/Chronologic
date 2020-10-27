using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Chronologic {
    public static class Functions {
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> function1, Func<T2, T3> function2) =>
            x => function2(function1(x));

        public static Func<T, T> Compose<T>(this IEnumerable<Func<T, T>> functions) =>
            functions.Aggregate(Compose);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Arrays {
        public static T[] Flatten<T>(this T[][] arrays) {
            var size = arrays.Sum(a => a.Length);
            var result = new T[size];
            for (int index = 0, i = 0; i < arrays.Length; index += arrays[i++].Length) {
                arrays[i].CopyTo(result, index);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult[] Select<TSource, TResult>(this TSource[] array, Func<TSource, TResult> selector, bool allowParallelism = false) {
            var result = new TResult[array.Length];
            if (allowParallelism) {
                Parallel.For(0, array.Length, i => {
                    result[i] = selector(array[i]);
                });
            }
            else {
                for (int i = array.Length - 1; i >= 0; i--) {
                    result[i] = selector(array[i]);
                }
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform<T>(this T[] array, Func<T, T> transformation, bool allowParallelism = false) {
            if (allowParallelism) {
                Parallel.For(0, array.Length, i => {
                    array[i] = transformation(array[i]);
                });
            }
            else {
                for (int i = array.Length - 1; i >= 0; i--) {
                    array[i] = transformation(array[i]);
                }
            }
        }
    }

    public static class EnumerableUtilities {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GZipCompress(this byte[] data, CompressionLevel compressionLevel) {
            MemoryStream memory;
            using (memory = new MemoryStream()) {
                using var stream = new GZipStream(memory, compressionLevel);
                stream.Write(data);
            }
            return memory.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GZipDecompress(this byte[] data) {
            MemoryStream memory;
            using (memory = new MemoryStream()) {
                using var inner = new MemoryStream(data);
                using var stream = new GZipStream(inner, CompressionMode.Decompress);
                int count;
                var bytes = new byte[4096];
                while ((count = stream.Read(bytes, 0, bytes.Length)) != 0) {
                    memory.Write(bytes, 0, count);
                }
            }
            return memory.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(T, T)> WithLag<T>(this IEnumerable<T> items, T initialPairValue) {
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
        public static IEnumerable<T> Scan<T>(this IEnumerable<T> items, Func<T, T, T, T> operation) {
            using var e = items.GetEnumerator();
            if (e.MoveNext()) {
                var x0 = e.Current;
                yield return x0;
                var y0 = x0;
                while (e.MoveNext()) {
                    var x1 = e.Current;
                    var y1 = operation(x1, x0, y0);
                    yield return y1;
                    y0 = y1;
                    x0 = x1;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> LossyTruncation(this IEnumerable<long> values, int lossiness) {
            if (lossiness == 0) {
                return values;
            }
            else {
                var roundingFactor = 1 << (lossiness - 1);
                return
                    values
                    .Select(v => (v + roundingFactor) >> lossiness);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] LossyTruncation(this long[] values, int lossiness) {
            if (lossiness == 0) {
                return values;
            }
            else {
                var roundingFactor = 1 << (lossiness - 1);
                return
                    values
                    .Select(v => (v + roundingFactor) >> lossiness);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LossyTruncation(ref long[] values, int lossiness) {
            if (lossiness != 0) {
                var roundingFactor = 1 << (lossiness - 1);
                values.Transform(v => (v + roundingFactor) >> lossiness);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> UnLossyTruncation(this IEnumerable<long> values, int lossiness) =>
            values.Select(v => v << lossiness);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> DoubleDelta<T>(this IEnumerable<T> values, Func<T, T, T, T> operation) =>
            values.Scan(operation).Scan(operation);        


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaSubtraction(this IEnumerable<long> values) =>
            values.Scan((x1, x0, _) => x1 - x0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaAddition(this IEnumerable<long> values) =>
            values.Scan((x1, _, y0) => x1 + y0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaAddition(this IEnumerable<long> values) =>
            values.DoubleDelta((x1, _, y0) => x1 + y0);//.DeltaAddition().DeltaAddition();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaSubtraction(this IEnumerable<long> values) =>
            values.DoubleDelta((x1, x0, _) => x1 - x0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaXorEncode(this IEnumerable<long> values) =>
            values.Scan((x1, x0, _) => x1 ^ x0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DeltaXorDecode(this IEnumerable<long> values) =>
            values.Scan((x1, _, y0) => x1 ^ y0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaXorEncode(this IEnumerable<long> values) =>
            values.DoubleDelta((x1, x0, _) => x1 ^ x0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> DoubleDeltaXorDecode(this IEnumerable<long> values) =>
            values.DoubleDelta((x1, _, y0) => x1 ^ y0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ZigZagEncode(this long value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0L
            ? 0UL
            : unchecked((ulong)((value << 1) ^ (value >> (sizeof(long) << 3) - 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ZigZagEncode(this int value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0
            ? 0u
            : unchecked((uint)((value << 1) ^ (value >> (sizeof(int) << 3) - 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ZigZagDecode(this ulong value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0UL
            ? 0L
            : unchecked((long)(value >> 1) ^ -((long)value & 1L));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ZigZagDecode(this uint value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0u
            ? 0
            : unchecked((int)(value >> 1) ^ -((int)value & 1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ZigZagEncode(this short value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0
            ? (ushort)0
            : unchecked((ushort)((value << 1) ^ (value >> (sizeof(short) << 3) - 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ZigZagDecode(this ushort value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0
            ? (short)0
            : unchecked((short)((short)(value >> 1) ^ -((short)value & 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ZigZagEncode(this sbyte value) =>
            // Quickly test for 0 first, before doing all the bit twiddling                
            value == 0
            ? (byte)0
            : unchecked((byte)((value << 1) ^ (value >> (sizeof(byte) << 3) - 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ZigZagDecode(this byte value) =>
            // Quickly test for 0 first, before doing all the bit twiddling
            value == 0
            ? (sbyte)0
            : unchecked((sbyte)((sbyte)(value >> 1) ^ -((sbyte)value & 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTarget[] SelectToArray<TSource, TTarget>(this TSource[] values, Func<TSource, TTarget> selector) {
            var results = new TTarget[values.Length];
            for (int i = 0; i < values.Length; i++) {
                results[i] = selector(values[i]);
            }
            return results;
        }
    }
}
