using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Metimur.Math {
    using static Basic;

    [AsyncMethodBuilder(typeof(Monads.OptionAwaiter<>))]
    public readonly struct Option<T> : IEquatable<Option<T>> {
        private readonly bool _hasValue;
        private readonly T _value;

        public Option(T value) {
            _hasValue = true;
            _value = value;
        }

        internal static Option<T> None { get; }

        public static Option<T> If(Func<bool> condition, Func<T> valueIfTrue) =>
            condition() ? new Option<T>(valueIfTrue()) : None;

        public TResult If<TResult>(Func<T, TResult> function, TResult alternateValue) =>
            _hasValue ? function(_value) : alternateValue;

        public TResult If<TResult>(Func<T, TResult> function, Func<TResult> alternate) =>
            _hasValue ? function(_value) : alternate();

        public T GetValue(T alternativeValue) =>
            _hasValue ? _value : alternativeValue;

        public static T operator |(Option<T> option, T alternateValue) =>
            option._hasValue ? option._value : alternateValue;

        public static Option<T> operator |(Option<T> option, Option<T> alternateOption) =>
            option._hasValue ? option : alternateOption;

#pragma warning disable CS8602
        public bool Equals(Option<T> other) =>
            _hasValue ? (other._hasValue && _value.Equals(other._value)) : !other._hasValue;
#pragma warning restore CS8602

        public override int GetHashCode() =>
            If(o => o!.GetHashCode(), 0);

        public override bool Equals(object other) =>
           other is Option<T> o && Equals(o);

        public static implicit operator Option<T>(OneOf<T> value) =>
            new Option<T>(value.Value);

        public static implicit operator Option<T>(NoneOf _) =>
            Option<T>.None;
    }

    public static class Option {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> AsOption<T>(this T value) =>
            value is null ? None : new Option<T>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TTarget> Select<TSource, TTarget>(this Option<TSource> option, Func<TSource, TTarget> function) =>
            option.If(x => new Option<TTarget>(function(x)), None);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SelectMany<T>(this Option<Option<T>> option) =>
            option.If(x => x, None);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TTarget> SelectMany<TSource, TTarget>(this Option<TSource> option, Func<TSource, Option<TTarget>> selector) =>
            option.If(x => selector(x), None);

        public static Option<TTarget> SelectMany<TSource, TMid, TTarget>(this Option<TSource> option, Func<TSource, Option<TMid>> middleSelector, Func<TSource, TMid, TTarget> resultSelector) =>
            option.If(x => middleSelector(x).If(y => new Option<TTarget>(resultSelector(x, y)), None), None);

        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.If(x => predicate(x) ? option : None, None);
    }

    namespace Monads {
        public static class OptionMonadicExtensions {
            public static OptionAwaiter<T> GetAwaiter<T>(this Option<T> option) =>
                new OptionAwaiter<T>(option);
        }

        public class OptionAwaiter<T> : INotifyCompletion {
            private readonly Option<T> _option;
            public OptionAwaiter(Option<T> option) =>
                _option = option;

            public bool IsCompleted { get; } =
                true;
            public T GetResult() =>
                _option.If(Id, () => throw new NullReferenceException());

            public void OnCompleted(Action continuation) {
                throw new NotImplementedException();
            }

            //public void SetResult(T result) =>

        }
    }
}
