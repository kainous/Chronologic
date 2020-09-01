namespace CSharp.Math {
    public readonly struct NoneOf {
        public static NoneOf Default { get; } =
            new NoneOf();

        public static implicit operator Void(NoneOf _) =>
            Void.Default;        
    }
}
