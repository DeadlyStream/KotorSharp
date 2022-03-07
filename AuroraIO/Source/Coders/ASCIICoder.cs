using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders {
    public interface ASCIIEncodingProtocol {
        string asciiEncoding(string indent = "");
    }
    public class ASCIICoder {
        public String encode(ASCIIEncodingProtocol obj) {
            return obj.asciiEncoding("");
        }

        public String encode(ASCIIEncodingProtocol[] array) {
            return String.Join("", array.Select(value => value.asciiEncoding("")));
        }
    }
}
