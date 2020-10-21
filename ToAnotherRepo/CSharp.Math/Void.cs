using System;

namespace Metimur.Math {
    // This is equivalent to System.Linq.Reactive.Unit
    // We don't have a coercion between them, becuase one library doesn't know of the other
    public readonly struct Void : IEquatable<Void> {
        public static Void Default { get; } = new Void();

        public bool Equals(Void other) =>
            true;

        public override bool Equals(object obj) =>
            obj is Void;

        public override int GetHashCode() =>
            0;

        public override string ToString() =>
            "()";
    }
}
