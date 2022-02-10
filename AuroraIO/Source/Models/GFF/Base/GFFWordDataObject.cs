using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFWordDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.WORD;
        }

        public int subItemCount() {
            return 0;
        }

        private ushort value;

        public GFFWordDataObject(UInt16 value) {
            this.value = value;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is ushort) {
                var c = (ushort)value;
                this.value = c;
            } else if (value is String) {
                var stringValue = value as String;
                this.value = ushort.Parse(stringValue);
            }
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

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("value {0}", value));
            return sb.ToString();
        }
    }
}
