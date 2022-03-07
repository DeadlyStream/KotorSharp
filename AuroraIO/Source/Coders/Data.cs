using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders {

    public class Data : List<byte> {

        public void copyBytesToOffset(byte[] bytes, int offset) {
            for (int i = 0; i < bytes.Length; i++) {
                this[offset + i] = bytes[i];
            }
        }

        public Data() { }

        private Data(byte[] value) {
            AddRange(value);
        }

        public static implicit operator Data(byte[] value) {
            return new Data(value);
        }

        public static implicit operator byte[](Data data) {
            return data.ToArray();
        }
    }
}
