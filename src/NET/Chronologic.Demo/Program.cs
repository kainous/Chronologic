using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Chronologic.Demo {
    class Program {
        static void Main(string[] args) {

            using var database = new SqlConnection(@"Data Source=(local)\SQL2014_PRIMARY;Integrated Security=True;Initial Catalog=IWC_Filled");
            using var command = new SqlCommand(@"SELECT*FROM[Data]WHERE[WellTagID]=96", database);
            using var table = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            var rows = table.Rows.Cast<DataRow>();
            var timestamps = rows.Select(row => ((DateTimeOffset)row["Timestamp"]).UtcDateTime);
            var timezones = rows.Select(row => (long)((DateTimeOffset)row["Timestamp"]).Offset.TotalMinutes);
            var cycles = rows.Select(row => (long)row["ProcessCycleID"]);
            var values = rows.Select(row => BitConverter.ToSingle((byte[])row["Value"]));

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
            //var serializer = new TimeSeriesSerializer();
            //var values = new long[] {
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158,
            //    151, 153, 155, 156, 158, 160, 161, 162, 163, 164, 166, 168, 170,
            //    172, 174, 173, 172, 171, 170, 169, 168, 166, 164, 162, 160, 158
            //};

            var opts = new[] { opt1, opt2, opt3 };

            byte[] bytes = new byte[0];
            for (int i = 2; i < opts.Length; i++) {

                var opt = opts[i];

                var b1 = timestamps.Select(a => a.Ticks).GetBytes(opt);
                var b2 = timezones.GetBytes(opt);
                var b3 = cycles.GetBytes(opt);
                var b4 = values.Select(a => (long)BitConverter.SingleToInt32Bits(a)).GetBytes(opt);


                bytes =
                    b1
                    .Concat(b2)
                    .Concat(b3)
                    .Concat(b4)
                    .ToArray();

                File.WriteAllBytesAsync(@$"D:\_TODELETE\query.{i + 8}.dat", bytes);
            }

            //using var file = File.OpenWrite(@"D:\_TODELETE\query.9.dat");
            //using var stream = new Zstandard.Net.ZstandardStream(file, 22);
            //stream.Write(bytes, 0, bytes.Length);

            //var bytes2 = values.GetBytes(opt2);
            //var bytes3 = values.GetBytes(opt3);

            //var items1 = bytes1.ToInt64(opt1).ToArray();
            //var items2 = bytes2.ToInt64(opt2).ToArray();
            //var items3 = bytes3.ToInt64(opt3).ToArray();

            //var items = serializer.ToInt64(bytes1, opt1.ValueLossiness).ToArray();
        }
    }
}
