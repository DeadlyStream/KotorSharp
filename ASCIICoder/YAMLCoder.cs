using System;
using System.Linq;

namespace YAMLEncoding {
    public interface YAMLEncodingProtocol {
        string asciiEncoding(string indent = "");
    }
    public class YAMLCoder {
        public String encode(YAMLEncodingProtocol obj) {
            return obj.asciiEncoding("");
        }

        public String encode(YAMLEncodingProtocol[] array) {
            return String.Join("", array.Select(value => value.asciiEncoding("")));
        }
    }
}
