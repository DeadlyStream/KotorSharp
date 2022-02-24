using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders {
    public interface ASCIIOutputProtocol {
        string asciiEncoding(string indent = "");
    }
    public class ASCIICoder {
        public String encode(ASCIIOutputProtocol obj) {
            return obj.asciiEncoding("");
        }
    }
}
