using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFCExoStringDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.CEXOSTRING;
        }

        public int subItemCount() {
            return 0;
        }


        private string value;

        public GFFCExoStringDataObject(String value) {
            this.value = value;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is String) {
                var stringValue = value as String;
                this.value = stringValue;
            } else {
                var stringValue = value.ToString();
                this.value = stringValue;
            }
        }

        public Object getValueAtPath(GFFPath path) {
            return value;
        }

        public int dataLength() {
            return value.Length + 4;
        }

        public byte[] toBytes() {
            ByteArray newByteArray = new ByteArray();
            newByteArray.AddRange(BitConverter.GetBytes((UInt32)value.Length));
            newByteArray.AddRange(Encoding.ASCII.GetBytes(value));
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
