using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFResrefDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.RESREF;
        }

        public int subItemCount() {
            return 0;
        }

        private string value;

        public GFFResrefDataObject(String value) {
            this.value = value.Substring(0, Math.Min(value.Count(), 16));
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is string) {
                var stringValue = value as string;
                this.value = stringValue.Substring(0, Math.Min(stringValue.Count(), 16));
            }
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return Math.Min(16, value.Length) + 1;
        }

        public byte[] toBytes() {
            ByteArray newByteArray = new ByteArray();
            newByteArray.Add((byte)value.Length);
            if (value.Length > 16) {
                newByteArray.AddRange(Encoding.ASCII.GetBytes(value.Substring(0, 16)));
            } else {
                newByteArray.AddRange(Encoding.ASCII.GetBytes(value));
            }
            //FLAG: I may need to add a byte here just for an empty string

            return newByteArray.ToArray();
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
