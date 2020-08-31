using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chronologic {

    public struct Flag8 : IEquatable<Flag8> {
        private readonly byte _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Flag8(byte value) =>
            _value = value;

        public static Flag8 Zero { get; } =
            new Flag8();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Flag8(byte value) =>
            new Flag8(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte(Flag8 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Flag8 other) =>
            _value.Equals(other._value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) =>
            obj is Flag8 f && Equals(f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag8 first, Flag8 second) =>
            first.Equals(second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag8 first, Flag8 second) =>
            !first.Equals(second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            _value.GetHashCode();
    }

    public struct CompressionOptions {
        public byte CompressionLevel { get; }
        public byte ValueLossiness { get; }
        public byte TimeLossiness { get; }
    }

    public class TimeSeriesSerializer {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<byte> GetBytes(IEnumerable<long> values, CompressionOptions options) {
            yield break;

            // Perform lossy truncation

            // Do a Double-Delta Subtraction

            // Do ZigZag encoding

            // Do Variable Length Encoding

            // GZIP


            // Do ANS on the total
        }

        public IEnumerable<byte> GetBytes(IEnumerable<Flag8> values, CompressionOptions options) {
            yield break;
            // Do a Double-Delta XOR

            // Transpose 8x8 bit-matrices

            // GZIP
        }

        public IEnumerable<byte> GetBytes(IEnumerable<float> values, CompressionOptions options) {
            yield break;
            // Valuewise convert to Posit32
            // Bitwise cast to int32
            // Do a lossy truncation
            // Do double-delta subtraction encoding
            // Zigzag encoding
            // VLE
            // GZIP
        }
    }
}
