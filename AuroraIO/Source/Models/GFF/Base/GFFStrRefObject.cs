using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFStrRefObject : GFFFieldDataObject {

        public ulong value;
        public GFFStrRefObject(ulong strRefID) {
            this.value = strRefID;
        }

        public GFFFieldType fieldType() {
            return GFFFieldType.STRREF;
        }

        public object getValueAtPath(GFFPath path) {
            return value;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is ulong) {
                this.value = (ulong)value;
            } else if (value is String) {
                this.value = ulong.Parse((string)value);
            } else {
                this.value = ulong.Parse(value.ToString());
            }
        }

        public int subItemCount() {
            return 0;
        }

        public int dataLength() {
            return 8;
        }

        public byte[] toBytes() {
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes(4));
            byteArray.AddRange(BitConverter.GetBytes(value));
            return byteArray.ToArray();
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
