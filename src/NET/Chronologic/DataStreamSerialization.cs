using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;

namespace Chronologic {


    public static class DataStreamSerialization {
        public static ReadOnlySpan<byte> FixedLengthEncoding(this ulong[] values) {
            const int bytesize = sizeof(ulong);
            var result = new byte[values.Length * bytesize];
            for (int i = 0; i < values.Length; i++) {
                var value = values[i];
                var primaryOffset = i * bytesize;
                for (int b = bytesize - 1; value != 0 && b >= 0; b--, value >>=8) {
                    result[b + primaryOffset] = (byte)value;
                }
            }
            return result;
        }

        public static IEnumerable<ulong> FixedLengthDecodingUInt64(this byte[] data) {
            var index = 0;
            while (index < data.Length) {
                var result = 0UL;
                for (int i = 0; i < sizeof(ulong); i++) {
                    result = (result << 8) | data[index++];
                }
                yield return result;
            }
        }

        public static ReadOnlySpan<byte> VariableLengthEncoding(this ulong[] values) {
            int index = values.Length * (((sizeof(ulong) << 3) + 6) / 7);
            var buffer = new byte[index];

            for (int i = values.Length - 1; i >= 0; i--) {
                var value = values[i];
                buffer[--index] = (byte)(value & 0x7F);
                value >>= 7;

                while (value != 0) {
                    buffer[--index] = (byte)(0x80 | value);
                    value >>= 7;
                }
            }

            return buffer.AsSpan(index);
        }

        public static IEnumerable<ulong> VariableLengthDecoding(this byte[] data) {
            var index = 0;
            while (index < data.Length) {
                var result = 0UL;
                while (data[index] > 0x7F) {
                    result = (result << 7) | (data[index++] & 0x7FUL);
                }
                result = (result << 7) | data[index++];
                yield return result;
            }
        }

        public static ReadOnlySpan<byte> VariableLengthEncoding(this uint[] values) {
            int index = values.Length * (((sizeof(uint) << 3) + 6) / 7);
            var buffer = new byte[index];

            for (int i = values.Length - 1; i >= 0; i--) {
                var value = values[i];
                buffer[--index] = (byte)(value & 0x7F);
                value >>= 7;

                while (value != 0) {
                    buffer[--index] = (byte)(0x80 | value);
                    value >>= 7;
                }
            }

            return buffer.AsSpan(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this IEnumerable<long> values, CompressionOptions options) {
            var a =
                values.LossyTruncation(options.ValueLossiness);

            var b =
                options.CompressionLevel == 0
                ? a
                    .Select(o => unchecked((ulong)o))
                    .ToArray()
                    .FixedLengthEncoding()
                : a
                    .DoubleDeltaSubtraction()
                    .Select(EnumerableUtilities.ZigZagEncode)
                    .ToArray()
                    .VariableLengthEncoding();

            return
                options.CompressionLevel > 1
                ? b.ToArray().GZipCompress(CompressionLevel.Optimal)
                : b.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this long[] values, CompressionOptions options) {
            var a =
                values.LossyTruncation(options.ValueLossiness);

            var b =
                options.CompressionLevel == 0
                ? a
                    .Select(o => unchecked((ulong)o))
                    .FixedLengthEncoding()
                : a
                    .DoubleDeltaSubtraction()
                    .Select(EnumerableUtilities.ZigZagEncode)
                    .ToArray()
                    .VariableLengthEncoding();

            return
                options.CompressionLevel > 1
                ? b.ToArray().GZipCompress(CompressionLevel.Optimal)
                : b.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> ToInt64(this byte[] data, CompressionOptions options) {
            var bytes =
                options.CompressionLevel > 1
                ? data.GZipDecompress()
                : data;

            var sdf =
                options.CompressionLevel == 0
                ? bytes
                    .FixedLengthDecodingUInt64()
                    .Select(o => (long)o)
                : bytes
                    .VariableLengthDecoding()
                    .Select(EnumerableUtilities.ZigZagDecode)
                    .DoubleDeltaAddition();

            return 
                sdf.UnLossyTruncation(options.ValueLossiness);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<string, byte[]> GetBytes(this IEnumerable<DateTimeOffset> values, CompressionOptions options) =>
            new Dictionary<string, byte[]> {
                ["TZ"] =
                        values
                        .Select(o => (long)o.Offset.TotalMinutes)
                        .GetBytes(options),
                ["TS"] =
                        values
                        .Select(o => o.Ticks)
                        .GetBytes(options)
            };
    }
}
