namespace CSharp.Math {
    /// <summary>
    /// Used for function arguments instead of Option&lt;T&gt
    /// </summary>
    public readonly struct Optional<T> {
        private readonly bool _hasValue;
        private readonly T _value;

        public Optional(T value) {
            _hasValue = true;
            _value = value;
        }

        /// <summary>
        /// This operator here is what makes it different from Option&lt;T&gt;
        /// Use this in functions to handle optional arguments where nullarity is not known
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Optional<T>(T value) =>
            new Optional<T>(value);

        public static implicit operator Option<T>(Optional<T> option) =>
            option._hasValue ? new Option<T>(option._value) : Option<T>.None;

        public static T operator |(Optional<T> option, T alternateValue) =>
            option._hasValue ? option._value : alternateValue;

        public static Option<T> operator |(Optional<T> option, Option<T> alternate) =>
            option._hasValue ? option : alternate;
    }
}
