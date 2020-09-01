using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Chronologic {
    public readonly struct Sample<TValue, TTracker> {
        public TTracker Tracker { get; }
        public TValue Value { get; }
        public JObject Metadata { get; }
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

    public class SampleBlock<TValue, TTracker> {
        private readonly SortedList<TTracker, Sample<TValue, TTracker>> _samples =
            new SortedList<TTracker, Sample<TValue, TTracker>>();

        public SampleBlock(IEnumerable<Sample<TValue, TTracker>> samples) {
            foreach (var sample in samples) {
                _samples.Add(sample.Tracker, sample);
            }
        }

        public IReadOnlyList<Sample<TValue, TTracker>> AsReadOnlyList()
    }

    public class TimeSeries {
        //public void Add<T>(Guid signalID, )
    }
}
