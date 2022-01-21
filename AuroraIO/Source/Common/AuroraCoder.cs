using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Common {
    public abstract class AuroraCoder<T> where T: AuroraResource {

        public T decodeFileAtPath(String filePath) {
            try {
                return decode(File.ReadAllBytes(filePath));
            } catch (Exception e) {
                throw new AuroraDecodeException(String.Format("Failed to decode {0}, reason: {1}", filePath, e.Message));
            }            
        }

        public void encodeToFilePath(String filePath, T obj) {
            File.WriteAllBytes(filePath, encode(obj));
        }

        public abstract T decode(byte[] byteArray);

        public abstract byte[] encode(T obj);
    }

    public class AuroraDecodeException : Exception {
        public AuroraDecodeException(string message) : base(message) {
        }
    }
}
