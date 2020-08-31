using System;

namespace Chronologic.Demo {
    class Program {
        static void Main(string[] args) {
            var test = new long[] {
                151, 153, 155, 156, 158, 160, 161, 162, 163
            };


            var serializer = new TimeSeriesSerializer();
            //serializer.GetBytes(test, new CompressionOptions());
            var result = test.DoubleDeltaSubtraction();
            var back = result.DoubleDeltaAddition();
        }
    }
}
