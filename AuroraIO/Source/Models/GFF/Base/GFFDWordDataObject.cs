using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFDWordDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.DWORD;
        }

        public int subItemCount() {
            return 0;
        }

        private uint value;

        public GFFDWordDataObject(UInt32 value) {
            this.value = value;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is uint) {
                this.value = (uint)value;
            } else if (value is String) {
                var stringValue = value as String;
                this.value = uint.Parse(stringValue);
            }
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return 4;
        }

        public byte[] toBytes() {
            return BitConverter.GetBytes(value);
        }

        public override string ToString() {
            return String.Format("{0}", value);
        }

        public string asciiEncoding() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("value {0}", value));
            return sb.ToString();
        }
    }
}
