using AuroraIO.Source.Models.GFF;
using AuroraIO.Source.Models.GFF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFListDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.LIST;
        }

        public int subItemCount() {
            return structInfoArray.Count;
        }

        public List<GFFStruct> structInfoArray;

        public GFFListDataObject(GFFStruct[] structInfoArray) {
            this.structInfoArray = new List<GFFStruct>(structInfoArray);
        }

        public void setValueForPath(object value, GFFPath path) {
            GFFPath firstPathComponent = path.first();
            GFFPath remainingPath = path.removingFirst();
            String lastPathComponent = path.last();

            if (value is GFFStruct) {
                var gffStruct = value as GFFStruct;

                if (!firstPathComponent.isEmpty) {
                    int index = Convert.ToInt32(firstPathComponent);
                    GFFStruct structInfo = structInfoArray[index];
                    structInfo[remainingPath] = value;
                } else {
                    structInfoArray.Add(gffStruct);
                }
            }

        }

        public Object getValueAtPath(GFFPath path) {
            GFFPath firstPathComponent = path.first();
            GFFPath remainingPath = path.removingFirst();
            String lastPathComponent = path.last();

            if (!firstPathComponent.isEmpty) {
                int index = Convert.ToInt32(firstPathComponent);
                return structInfoArray[index];
            } else {
                return null;
            }
        }

        public int dataLength() {
            if (structInfoArray.Count > 0) {
                return (structInfoArray.Count + 1) * 4;
            } else {
                return 0;
            }
        }

        public byte[] toBytes() {
            throw new NotImplementedException();
        }

        public byte[] toBytes(ByteArray listIndicesArray) {
            //dataOrDataoffset is offset into listIndices array, which points to a list array element
            //starts with size (DWORD) then an array of Dwords pointing to struct indices
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes((uint)listIndicesArray.Count));
            return byteArray.ToArray();
        }

        public override string ToString() {
            return String.Format("{{structs: [{0}]}}",
                String.Join(",", structInfoArray.Select(structInfo => structInfo.ToString()))
            );
        }
        public string asciiEncoding() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("value");
            foreach (GFFStruct gffStruct in structInfoArray) {
                sb.AppendLine(String.Format("{0}", gffStruct.asciiEncoding()));
            }
            
            return sb.ToString();
        }
    }
}
