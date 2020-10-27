using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Zstandard.Net;

namespace Chronologic.Demo {
    //public readonly struct Tracker {

    //}

    //public readonly struct ISample<T> {
    //    public Timestamp
    //}

    public static class Timestamps {
        private static readonly DateTimeOffset _ProtobufEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
        public static long AsNanosecondsFromProtobufEpoch(this DateTimeOffset timestamp) =>
            (timestamp - _ProtobufEpoch).Ticks * 100L;
    }

    

    class Program {
        //public static byte[] ANS(byte[] data) {
        //    MemoryStream memory;
        //    using (memory = new MemoryStream())
        //    using (var zip = new ZstandardStream(memory, 1)) {
        //        zip.Write(data, 0, data.Length);
        //    }

        //    return memory.ToArray();
        //}

        //public static byte[] GZIP(byte[] data, CompressionLevel compressionLevel) {
        //    MemoryStream memory;
        //    using (memory = new MemoryStream())
        //    using (var zip = new GZipStream(memory, compressionLevel)) {
        //        zip.Write(data, 0, data.Length);
        //    }

        //    return memory.ToArray();
        //}

        static void Main(string[] args) {

            using var database = new SqlConnection(@"Data Source=(local)\SQL2014_PRIMARY;Integrated Security=True;Initial Catalog=IWC_Filled");
            using var command = new SqlCommand(@"SELECT*FROM[Data]WHERE[WellTagID]=96", database);
            using var table = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            var rows = table.Rows.Cast<DataRow>();
            var timestamps = rows.Select(row => ((DateTimeOffset)row["Timestamp"]).AsNanosecondsFromProtobufEpoch()).ToArray();
            var timezones = rows.Select(row => (long)((DateTimeOffset)row["Timestamp"]).Offset.TotalMinutes).ToArray();
            var cycles = rows.Select(row => (long)row["ProcessCycleID"]).ToArray();
            var values = rows.Select(row => BitConverter.ToSingle((byte[])row["Value"])).ToArray();



            //var jobj = new JObject {
            //    ["Timestamps"] = new JArray(timestamps),
            //    //["Timezones"] = new JArray(timezones),
            //    //["Cycles"] = new JArray(cycles),
            //    ["Values"] = new JArray(values)
            //};
            //File.WriteAllText(@"D:\_TODELETE\query.json", jobj.ToString(Formatting.None));


            var opt1 = new CompressionOptions(0, 0, 0);
            var opt2 = new CompressionOptions(1, 0, 0);
            var opt3 = new CompressionOptions(2, 0, 0);




            using var file = File.OpenWrite(@"D:\_TODELETE\query.dat");
            //using var stream = new Zstandard.Net.ZstandardStream(file, 22);
            //stream.Write(bytes, 0, bytes.Length);

            var bytes2 = cycles.GetBytes(opt2);
            //var bytes3 = values.GetBytes(opt3);

            //var items1 = bytes1.ToInt64(opt1).ToArray();
            //var items2 = bytes2.ToInt64(opt2).ToArray();
            //var items3 = bytes3.ToInt64(opt3).ToArray();

            //var items = serializer.ToInt64(bytes1, opt1.ValueLossiness).ToArray();
        }
    }
}
