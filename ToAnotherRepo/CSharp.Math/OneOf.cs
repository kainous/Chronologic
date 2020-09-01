namespace CSharp.Math {
    /// <summary>
    /// This is both a Monad and comonad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct OneOf<T> {
        public T Value { get; }
        public OneOf(T value) =>
            Value = value;
    }
}
