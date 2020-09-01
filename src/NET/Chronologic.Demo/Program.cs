using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Chronologic.Demo {
    class Program {
        static void Main(string[] args) {

            var opt1 = new CompressionOptions(0, 0, 0);
            var opt2 = new CompressionOptions(1, 0, 0);
            var opt3 = new CompressionOptions(2, 0, 0);
            var serializer = new TimeSeriesSerializer();
            var values = new long[] {
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
                151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
                172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158
            };
            var bytes1 = values.GetBytes(opt1);
            var bytes2 = values.GetBytes(opt2);
            var bytes3 = values.GetBytes(opt3);
            var items1 = bytes1.ToInt64(opt1).ToArray();
            var items2 = bytes2.ToInt64(opt2).ToArray();
            var items3 = bytes3.ToInt64(opt3).ToArray();

            //var items = serializer.ToInt64(bytes1, opt1.ValueLossiness).ToArray();
        }
    }
}
