using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFCharDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.CHAR;
        }

        public int subItemCount() {
            return 0;
        }

        private char value;

        public GFFCharDataObject(char c) {
            this.value = c;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is char) {
                var c = (char)value;
                this.value = c;
            } else if (value is String) {
                var stringValue = value as String;
                this.value = char.Parse(stringValue);
            }
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return 4;
        }

        public byte[] toBytes() {
            return new byte[] { (byte)value, 0, 0, 0 };
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
