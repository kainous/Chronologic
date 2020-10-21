using System;
using System.Threading.Tasks;
using Metimur.Math;
using Metimur.Math.Monads;

namespace Metior.Math.Measurement {
    public class Class1 {
        public async Option<string> Test(Option<Task<int>> item) {
            var s = await (await item);
            return s.ToString();
        }
    }
}
