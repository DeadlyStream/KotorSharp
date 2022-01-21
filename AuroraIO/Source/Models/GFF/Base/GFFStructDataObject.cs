using AuroraIO.Source.Models.GFF;
using AuroraIO.Source.Models.GFF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFStructDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.STRUCT;
        }

        public int subItemCount() {
            return 1;
        }

        public GFFStruct structInfo;

        public GFFStructDataObject(GFFStruct structInfo) {
            this.structInfo = structInfo;
        }

        public void setValueForPath(object value, GFFPath path) {
            GFFPath subPath = path.removingFirst();
            if (!subPath.isEmpty) {
                structInfo[subPath.ToString()] = value;
            } else  if (value is GFFStruct) {
                structInfo = value as GFFStruct;      
            }
        }

        public Object getValueAtPath(GFFPath path) {
            GFFPath subPath = path.removingFirst();
            return structInfo[subPath.ToString()];
        }

        public int dataLength() {
            return 4;
        }

        public byte[] toBytes() {
            throw new NotImplementedException();
        }

        public byte[] toBytes(List<GFFStructInfo> structs) {
            //*As a special case, if the Field Type is a Struct, then DataOrDataOffset is an index into the Struct Array
            //instead of a byte offset into the Field Data Block.
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes((uint)structs.Count));
            return byteArray.ToArray();
        }

        public override string ToString() {
            return String.Format("{{id: {0} fields: {{1}}}}", structInfo.structType,
                String.Join(",", structInfo.fields.Select(field => field.ToString()))
            );
        }

        public string asciiEncoding() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("value");
            sb.AppendLine(String.Format("{0}", structInfo.asciiEncoding()));
            return sb.ToString();
        }
    }
}
