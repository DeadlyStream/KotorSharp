using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFByteDataObject : GFFFieldDataObject {
        public GFFFieldType fieldType() {
            return GFFFieldType.BYTE;
        }

        public int subItemCount() {
            return 0;
        }

        private byte value;

        public GFFByteDataObject(byte b) {
            this.value = b;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is byte) {
                var b = (byte)value;
                this.value = b;
            } else if (value is String) {
                var stringValue = value as String;
                this.value = byte.Parse(stringValue);
            }         
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return 4;
        }

        public byte[] toBytes() {
            return new byte[] { value, 0, 0, 0 };
        }

        public override string ToString() {
            return String.Format("{0}", value);
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("value {0}", value));
            return sb.ToString();
        }
    }
}
