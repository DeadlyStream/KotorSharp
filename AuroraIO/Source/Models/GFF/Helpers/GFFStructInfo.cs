using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.GFF.Helpers {
    public struct GFFStructInfo {
        public readonly uint structType;
        public readonly uint fieldCount;
        public readonly uint dataOrDataOffset;

        public GFFStructInfo(uint structType, int fieldCount, int dataOrDataOffset) {
            this.structType = structType;
            this.fieldCount = (uint)fieldCount;
            this.dataOrDataOffset = (uint)dataOrDataOffset;
        }

        public byte[] toBytes() {
            ByteArray newByteArray = new ByteArray();
            newByteArray.AddRange(BitConverter.GetBytes((UInt32)structType));
            newByteArray.AddRange(BitConverter.GetBytes((UInt32)dataOrDataOffset));
            newByteArray.AddRange(BitConverter.GetBytes(fieldCount));
            return newByteArray.ToArray();
        }
    }
}
