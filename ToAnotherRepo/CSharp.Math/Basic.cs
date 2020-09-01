using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSharp.Math {
    public static class Basic {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Id<T>(T value) =>
            value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<TIgnore, TConst> Const<TConst, TIgnore>(TConst value) =>
            _ => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<TConst> Const<TConst>(TConst value) =>
            () => value;


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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T3> ComposeWith<T1, T2, T3>(this Func<T1, T2> first, Func<T2, T3> second) =>
            x => second(first(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T4> ComposeWith<T1, T2, T3, T4>(this Func<T1, T2> function1, Func<T2, T3> function2, Func<T3, T4> function3) =>
            x => function3(function2(function1(x)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T5> ComposeWith<T1, T2, T3, T4, T5>(this Func<T1, T2> function1, Func<T2, T3> function2, Func<T3, T4> function3, Func<T4, T5> function4) =>
            x => function4(function3(function2(function1(x))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T, T> ComposeWith<T>(params Func<T, T>[] functions) =>
            functions.Aggregate(ComposeWith);

        public static NoneOf None;
        public static OneOf<T> Some<T>(T value) =>
            new OneOf<T>(value);
    }
}
