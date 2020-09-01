using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CSharp.Math;
using Newtonsoft.Json.Linq;

namespace Chronologic {
    public readonly struct Sample<TValue, TTracker> {
        public TTracker Tracker { get; }
        public TValue Value { get; }
        public JObject Metadata { get; }

        public Sample(TValue value, TTracker tracker, JObject metadata) {
            Value = value;
            Tracker = tracker;
            Metadata = metadata;
        }

        public Sample<TValue, TTracker> With(Optional<TValue> value = default, Optional<TTracker> tracker = default, JObject? metadata = default) =>
            new Sample<TValue, TTracker>(value | Value, tracker | Tracker, metadata ?? Metadata);

        public void Deconstruct(out TValue value, out TTracker tracker, out JObject metadata) {
            value = Value;
            tracker = Tracker;
            metadata = Metadata;
        }
    }

    //public interface IField<T> where T : IField {
    //    public T MultiplyWith(T value);
    //    public T AddWith(T value);
    //    public T AdditiveInverse();
    //    public Option<T> MultiplicativeInverse();

    //}

    //public interface IVectorSpace<TVector, TScalar> 
    //  where TVector : IVectorSpace<TVector, TScalar> {
    //    TVector MultiplyWith(TScalar scale);
    //    TVector Subtract(TVector vector);
    //    TVector AddWith(TVector vector);

    //    public TVector Average(IReadOnlyCollection<TVector> items) {
    //        var accumulator = default(TScalar);
    //        foreach (var item in items) {
    //            accumulator
    //        }
    //    }
    //}

    //public static class AffineSpace {
    //    //public T Average<T>(IReadOnlyCollection<T> items)
    //    //  where T : IAffineSpace<T> {

    //    //}
    //}

    public class SampleBlock<TValue, TTracker> : IEnumerable<Sample<TValue, TTracker>> {
        private readonly SortedList<TTracker, Sample<TValue, TTracker>> _samples =
            new SortedList<TTracker, Sample<TValue, TTracker>>();

        public SampleBlock(IEnumerable<Sample<TValue, TTracker>> samples) {
            foreach (var sample in samples) {
                _samples.Add(sample.Tracker, sample);
            }
        }

        public IEnumerator<Sample<TValue, TTracker>> GetEnumerator() =>
            _samples.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public class TimeSeries {
        //public void Add<T>(Guid signalID, )
    }
}
