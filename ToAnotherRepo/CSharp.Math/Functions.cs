using System;
using System.Runtime.CompilerServices;

namespace CSharp.Math {
    public static class Functions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Id<T>(T value) =>
            value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<TIgnore, TConst> Const<TConst, TIgnore>(TConst value) =>
            _ => value;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3> Curry<T1, T2, T3>(this Func<T1, Func<T2, T3>> function) =>
            (x, y) => function(x)(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4> Curry<T1, T2, T3, T4>(this Func<T1, Func<T2, Func<T3, T4>>> function) =>
            (x, y, z) => function(x)(y)(z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5> Curry<T1, T2, T3, T4, T5>(this Func<T1, Func<T2, Func<T3, Func<T4, T5>>>> function) =>
            (x, y, z, w) => function(x)(y)(z)(w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, T3>> Uncurry<T1, T2, T3>(this Func<T1, T2, T3> function) =>
            x => y => function(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, T4>>> Uncurry<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
            x => y => z => function(x, y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, T5>>>> Uncurry<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> function) =>
            x => y => z => w => function(x, y, z, w);
    }
}
