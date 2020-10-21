using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologic.Tests {
    [TestClass]
    public class DeltaTesting {
        private static JObject _json;
        [ClassInitialize]
        public static void Initialize(TestContext context) {
            var path = Path.Combine(context.DeploymentDirectory, "TestData.json");
            using var sReader = new StreamReader(path);
            using var jReader = new JsonTextReader(sReader);
            _json = JObject.Load(jReader);
        }

        [TestMethod]
        public void DoubleDeltaSubtraction() {
            foreach (var item in _json["DoubleDeltaSubtraction"]) {
                var inputs = item["Inputs"].Select(x => (long)x).ToArray();
                var outputs = item["Outputs"].Select(x => (long)x).ToArray();

                var testOutputs = inputs.DoubleDeltaSubtraction().ToArray();
                CollectionAssert.AreEqual(outputs, testOutputs);
                var testInputs = testOutputs.DoubleDeltaAddition().ToArray();
                CollectionAssert.AreEqual(inputs, testInputs);
            }
        }

        [TestMethod]
        public void DoubleDeltaXor() {
            foreach (var item in _json["DoubleDeltaXor"]) {
                var inputs = item["Inputs"].Select(x => (long)x).ToArray();
                var outputs = item["Outputs"].Select(x => (long)x).ToArray();

                var testOutputs = inputs.DoubleDeltaXorEncode().ToArray();
                CollectionAssert.AreEqual(outputs, testOutputs);
                var testInputs = testOutputs.DoubleDeltaXorDecode().ToArray();
                CollectionAssert.AreEqual(inputs, testInputs);
            }
        }

        [TestMethod]
        public void ZigZagEncoding() {
            var testIn = (long)4611686018427387904;
            var testOut = (long)testIn.ZigZagEncode();

            var json = _json["ZigZagEncodings"];
            var inputs = json["Inputs"].Select(o => (long)o).ToArray();
            var outputs = json["Outputs"].Select(o => ulong.Parse((string)o)).ToArray();
            var testOutputs = inputs.SelectToArray(o => o.ZigZagEncode());
            CollectionAssert.AreEqual(testOutputs, outputs);
            var testInputs = outputs.SelectToArray(o => o.ZigZagDecode());
            CollectionAssert.AreEqual(testInputs, inputs);
        }
    }
}
