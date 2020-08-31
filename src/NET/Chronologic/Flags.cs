 
using System;
using System.Runtime.CompilerServices;

namespace Chronologic {
    public readonly partial struct Flag8 : IEquatable<Flag8> {
        private readonly byte _value;

        private Flag8(byte value) =>
            _value = value;

        private void TransposeUnsafe(Span<Flag8> data) {
            // Half the maximum value of byte
            var m = (byte)0x0F;

            for (int j = 4; j != 0; j >>= 1, m ^= (byte)(m << j)) {
                for (int k = 0; k < 8; k = ((k | j) + 1) & ~j) {
                    var t = (byte)((data[k] ^ (data[k | j] >> j)) & m);
                    data[k] ^= t;
                    data[k | j] ^= (byte)(t << j);
                }
            }
        }

        // THIS PARTICULAR implementation demands only the (^) operator
        public Flag8[] DoubleDeltaXor(Flag8[] data) {
            var result = new Flag8[data.Length];
            data.CopyTo(result, 0);
            if (result.Length == 0 || result.Length == 1) {
                return result;
            }
            else if (result.Length == 2) {
                result[1] ^= result[0];
                return result;
            }
            var prev2 = result[0] ^ result[1];
            var prev1 = result[1];

            for (int i = 2; i < result.Length; i++) {
                var prev0 = result[i];
                // Required explicit conversion due to bytes
                var next1 = (Flag8)(prev0 ^ prev1);
                result[i] = (Flag8)(next1 ^ prev2);
                prev2 = next1;
                prev1 = prev0;
            }

            return result;
        }

        public Flag8[] TransposeEncode(Flag8[] data) {
            const int bitsize = 8;
            // The new array size is made of 8-byte chunks:
            var result = new Flag8[(data.Length + bitsize - 1) & ~0x7];

            data.CopyTo(result, 0);
            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result;
        }

        public Span<Flag8> TransposeDecode(Flag8[] data, int count) {
            const int bitsize = 8;
            var result = (Flag8[])data.Clone();

            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result.AsSpan(0, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Flag8(byte value) =>
            new Flag8(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte(Flag8 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag8 first, Flag8 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag8 first, byte second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(byte first, Flag8 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag8 first, Flag8 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag8 first, byte second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(byte first, Flag8 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Flag8 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is Flag8 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();

        public override string ToString() =>
            _value.ToString("X2");
    }

    public readonly partial struct U8 : IEquatable<U8>  {
        private readonly byte _value;

        private U8(byte value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator U8(byte value) =>
            new U8(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte(U8 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U8 first, U8 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U8 first, byte second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(byte first, U8 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U8 first, U8 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U8 first, byte second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(byte first, U8 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(U8 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is U8 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }

    public readonly partial struct I8 : IEquatable<I8>  {
        private readonly sbyte _value;

        private I8(sbyte value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator I8(sbyte value) =>
            new I8(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator sbyte(I8 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I8 first, I8 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I8 first, sbyte second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(sbyte first, I8 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I8 first, I8 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I8 first, sbyte second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(sbyte first, I8 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(I8 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is I8 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }
    public readonly partial struct Flag16 : IEquatable<Flag16> {
        private readonly ushort _value;

        private Flag16(ushort value) =>
            _value = value;

        private void TransposeUnsafe(Span<Flag16> data) {
            // Half the maximum value of ushort
            var m = (ushort)0x00FF;

            for (int j = 8; j != 0; j >>= 1, m ^= (ushort)(m << j)) {
                for (int k = 0; k < 16; k = ((k | j) + 1) & ~j) {
                    var t = (ushort)((data[k] ^ (data[k | j] >> j)) & m);
                    data[k] ^= t;
                    data[k | j] ^= (ushort)(t << j);
                }
            }
        }

        // THIS PARTICULAR implementation demands only the (^) operator
        public Flag16[] DoubleDeltaXor(Flag16[] data) {
            var result = new Flag16[data.Length];
            data.CopyTo(result, 0);
            if (result.Length == 0 || result.Length == 1) {
                return result;
            }
            else if (result.Length == 2) {
                result[1] ^= result[0];
                return result;
            }
            var prev2 = result[0] ^ result[1];
            var prev1 = result[1];

            for (int i = 2; i < result.Length; i++) {
                var prev0 = result[i];
                // Required explicit conversion due to bytes
                var next1 = (Flag16)(prev0 ^ prev1);
                result[i] = (Flag16)(next1 ^ prev2);
                prev2 = next1;
                prev1 = prev0;
            }

            return result;
        }

        public Flag16[] TransposeEncode(Flag16[] data) {
            const int bitsize = 16;
            // The new array size is made of 16-byte chunks:
            var result = new Flag16[(data.Length + bitsize - 1) & ~0xF];

            data.CopyTo(result, 0);
            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result;
        }

        public Span<Flag16> TransposeDecode(Flag16[] data, int count) {
            const int bitsize = 16;
            var result = (Flag16[])data.Clone();

            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result.AsSpan(0, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Flag16(ushort value) =>
            new Flag16(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ushort(Flag16 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag16 first, Flag16 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag16 first, ushort second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ushort first, Flag16 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag16 first, Flag16 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag16 first, ushort second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ushort first, Flag16 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Flag16 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is Flag16 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();

        public override string ToString() =>
            _value.ToString("X4");
    }

    public readonly partial struct U16 : IEquatable<U16>  {
        private readonly ushort _value;

        private U16(ushort value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator U16(ushort value) =>
            new U16(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ushort(U16 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U16 first, U16 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U16 first, ushort second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ushort first, U16 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U16 first, U16 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U16 first, ushort second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ushort first, U16 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(U16 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is U16 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }

    public readonly partial struct I16 : IEquatable<I16>  {
        private readonly short _value;

        private I16(short value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator I16(short value) =>
            new I16(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator short(I16 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I16 first, I16 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I16 first, short second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(short first, I16 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I16 first, I16 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I16 first, short second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(short first, I16 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(I16 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is I16 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }
    public readonly partial struct Flag32 : IEquatable<Flag32> {
        private readonly uint _value;

        private Flag32(uint value) =>
            _value = value;

        private void TransposeUnsafe(Span<Flag32> data) {
            // Half the maximum value of uint
            var m = (uint)0x0000FFFF;

            for (int j = 16; j != 0; j >>= 1, m ^= (uint)(m << j)) {
                for (int k = 0; k < 32; k = ((k | j) + 1) & ~j) {
                    var t = (uint)((data[k] ^ (data[k | j] >> j)) & m);
                    data[k] ^= t;
                    data[k | j] ^= (uint)(t << j);
                }
            }
        }

        // THIS PARTICULAR implementation demands only the (^) operator
        public Flag32[] DoubleDeltaXor(Flag32[] data) {
            var result = new Flag32[data.Length];
            data.CopyTo(result, 0);
            if (result.Length == 0 || result.Length == 1) {
                return result;
            }
            else if (result.Length == 2) {
                result[1] ^= result[0];
                return result;
            }
            var prev2 = result[0] ^ result[1];
            var prev1 = result[1];

            for (int i = 2; i < result.Length; i++) {
                var prev0 = result[i];
                // Required explicit conversion due to bytes
                var next1 = (Flag32)(prev0 ^ prev1);
                result[i] = (Flag32)(next1 ^ prev2);
                prev2 = next1;
                prev1 = prev0;
            }

            return result;
        }

        public Flag32[] TransposeEncode(Flag32[] data) {
            const int bitsize = 32;
            // The new array size is made of 32-byte chunks:
            var result = new Flag32[(data.Length + bitsize - 1) & ~0x1F];

            data.CopyTo(result, 0);
            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result;
        }

        public Span<Flag32> TransposeDecode(Flag32[] data, int count) {
            const int bitsize = 32;
            var result = (Flag32[])data.Clone();

            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result.AsSpan(0, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Flag32(uint value) =>
            new Flag32(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint(Flag32 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag32 first, Flag32 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag32 first, uint second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(uint first, Flag32 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag32 first, Flag32 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag32 first, uint second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(uint first, Flag32 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Flag32 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is Flag32 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();

        public override string ToString() =>
            _value.ToString("X8");
    }

    public readonly partial struct U32 : IEquatable<U32>  {
        private readonly uint _value;

        private U32(uint value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator U32(uint value) =>
            new U32(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint(U32 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U32 first, U32 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U32 first, uint second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(uint first, U32 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U32 first, U32 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U32 first, uint second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(uint first, U32 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(U32 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is U32 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }

    public readonly partial struct I32 : IEquatable<I32>  {
        private readonly int _value;

        private I32(int value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator I32(int value) =>
            new I32(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(I32 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I32 first, I32 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I32 first, int second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int first, I32 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I32 first, I32 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I32 first, int second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int first, I32 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(I32 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is I32 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }
    public readonly partial struct Flag64 : IEquatable<Flag64> {
        private readonly ulong _value;

        private Flag64(ulong value) =>
            _value = value;

        private void TransposeUnsafe(Span<Flag64> data) {
            // Half the maximum value of ulong
            var m = (ulong)0x00000000FFFFFFFF;

            for (int j = 32; j != 0; j >>= 1, m ^= (ulong)(m << j)) {
                for (int k = 0; k < 64; k = ((k | j) + 1) & ~j) {
                    var t = (ulong)((data[k] ^ (data[k | j] >> j)) & m);
                    data[k] ^= t;
                    data[k | j] ^= (ulong)(t << j);
                }
            }
        }

        // THIS PARTICULAR implementation demands only the (^) operator
        public Flag64[] DoubleDeltaXor(Flag64[] data) {
            var result = new Flag64[data.Length];
            data.CopyTo(result, 0);
            if (result.Length == 0 || result.Length == 1) {
                return result;
            }
            else if (result.Length == 2) {
                result[1] ^= result[0];
                return result;
            }
            var prev2 = result[0] ^ result[1];
            var prev1 = result[1];

            for (int i = 2; i < result.Length; i++) {
                var prev0 = result[i];
                // Required explicit conversion due to bytes
                var next1 = (Flag64)(prev0 ^ prev1);
                result[i] = (Flag64)(next1 ^ prev2);
                prev2 = next1;
                prev1 = prev0;
            }

            return result;
        }

        public Flag64[] TransposeEncode(Flag64[] data) {
            const int bitsize = 64;
            // The new array size is made of 64-byte chunks:
            var result = new Flag64[(data.Length + bitsize - 1) & ~0x3F];

            data.CopyTo(result, 0);
            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result;
        }

        public Span<Flag64> TransposeDecode(Flag64[] data, int count) {
            const int bitsize = 64;
            var result = (Flag64[])data.Clone();

            for (int i = 0; i < result.Length; i += bitsize) {
                TransposeUnsafe(result.AsSpan(i, bitsize));
            }
            return result.AsSpan(0, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Flag64(ulong value) =>
            new Flag64(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(Flag64 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag64 first, Flag64 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Flag64 first, ulong second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong first, Flag64 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag64 first, Flag64 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Flag64 first, ulong second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong first, Flag64 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Flag64 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is Flag64 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();

        public override string ToString() =>
            _value.ToString("X16");
    }

    public readonly partial struct U64 : IEquatable<U64>  {
        private readonly ulong _value;

        private U64(ulong value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator U64(ulong value) =>
            new U64(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(U64 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U64 first, U64 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(U64 first, ulong second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong first, U64 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U64 first, U64 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(U64 first, ulong second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong first, U64 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(U64 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is U64 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }

    public readonly partial struct I64 : IEquatable<I64>  {
        private readonly long _value;

        private I64(long value) =>
            _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator I64(long value) =>
            new I64(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator long(I64 value) =>
            value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I64 first, I64 second) =>
            first._value == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(I64 first, long second) =>
            first._value == second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(long first, I64 second) =>
            first == second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I64 first, I64 second) =>
            first._value != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(I64 first, long second) =>
            first._value != second;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(long first, I64 second) =>
            first != second._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(I64 other) =>
            _value.Equals(other._value);

        public override bool Equals(object other) =>
            other is I64 x && Equals(x);

        public override int GetHashCode() =>
            _value.GetHashCode();
    
        public override string ToString() =>
            _value.ToString();
    }
}