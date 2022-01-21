using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFInt64DataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.INT64;

        }

        public int subItemCount() {
            return 0;
        }

        private long value;

        public GFFInt64DataObject(Int64 value) {
            this.value = value;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is long) {
                this.value = (long)value;
            } else if (value is String) {
                var stringValue = value as String;
                this.value = long.Parse(stringValue);
            }
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return 8;
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
