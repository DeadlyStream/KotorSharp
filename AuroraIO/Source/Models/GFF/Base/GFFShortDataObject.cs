using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFShortDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.SHORT;
        }

        public int subItemCount() {
            return 0;
        }

        private short value;

        public GFFShortDataObject(Int16 value) {
            this.value = value;
        }

        public void setValueForPath(object value, GFFPath path) {
            this.value = short.Parse((string)value);
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return 4;
        }

        public byte[] toBytes() {
            ByteArray newByteArray = new ByteArray();
            newByteArray.AddRange(BitConverter.GetBytes(value));
            newByteArray.AddRange(new byte[] { 0, 0 });
            return newByteArray.ToArray();
        }

        public override string ToString() {
            return String.Format("{0}", value);
        }

        public string asciiEncoding() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("{0}", value));
            return sb.ToString();
        }
    }
}
