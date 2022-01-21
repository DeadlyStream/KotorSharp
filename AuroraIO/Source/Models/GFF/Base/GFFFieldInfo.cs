using AuroraIO.Source.Models.GFF;
using AuroraIO.Source.Models.GFF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFFieldInfo {
        public GFFFieldType fieldType;
        public uint labelIndex;
        public uint dataOrDataOffset;

        public GFFFieldInfo(GFFFieldType fieldType, uint labelIndex, uint dataOrDataOffset) {
            this.fieldType = fieldType;
            this.labelIndex = labelIndex;
            this.dataOrDataOffset = dataOrDataOffset;
        }

        public byte[] toBytes() {
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes((uint)fieldType));
            byteArray.AddRange(BitConverter.GetBytes(labelIndex));
            byteArray.AddRange(BitConverter.GetBytes(dataOrDataOffset));
            return byteArray.ToArray();
        }
    }
}
