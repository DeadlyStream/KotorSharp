using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AuroraIO {
    public class GFFVoidDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.VOID;
        }

        public int subItemCount() {
            return 0;
        }

        private byte[] byteArray;

        public GFFVoidDataObject(byte[] byteArray) {
            this.byteArray = byteArray;
        }

        public void setValueForPath(object value, GFFPath path) {
            if (value is string) {
                var stringValue = value as string;
                byteArray = Enumerable.Range(0, stringValue.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(stringValue.Substring(x, 2), 16))
                    .ToArray();
            }

        }

        public Object getValueAtPath(GFFPath path) {
            return byteArray;
        }

        public int dataLength() {
            return byteArray.Length;
        }

        public byte[] toBytes() {
            return byteArray;
        }

        public override string ToString() {
            return String.Join("", byteArray.Select(b => {
                return String.Format("{0:x2}", b);
            }).ToArray());
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("value\n{0}", ToString()));
            return sb.ToString();
        }
    }
}
