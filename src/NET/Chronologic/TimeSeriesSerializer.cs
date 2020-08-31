using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Chronologic {

    public class TimeSeriesSerializer {
        

        



        public IEnumerable<byte> GetBytes(IEnumerable<Flag8> values, CompressionOptions options) {
            yield break;
            // Do a Double-Delta XOR

            // Transpose 8x8 bit-matrices

            // GZIP
        }

        public IEnumerable<byte> GetBytes(IEnumerable<float> values, CompressionOptions options) {
            yield break;
            // Valuewise convert to Posit32
            // Bitwise cast to int32
            // Do a lossy truncation
            // Do double-delta subtraction encoding
            // Zigzag encoding
            // VLE
            // GZIP
        }
    }
}
