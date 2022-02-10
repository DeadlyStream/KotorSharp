using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFIntDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.INT;
        }

        public int subItemCount() {
            return 0;
        }

        private int value;

        public GFFIntDataObject(Int32 value) {
            this.value = value;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is int) {
                this.value = (int)value;
            } else if (value is String) {
                var stringValue = value as String;
                this.value = int.Parse(stringValue);
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

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("value {0}", value));
            return sb.ToString();
        }
    }
}
