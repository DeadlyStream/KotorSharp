using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO {
    public class AuroraBinaryObject : AuroraResource {

        private byte[] fileArray;

        public AuroraBinaryObject(AuroraResourceInfo resInfo, byte[] fileArray) {
            this.fileName = resInfo.resref;
            this.fileType = resInfo.resourceType;
            this.fileArray = fileArray;
        }

        public AuroraBinaryObject(String filePath) : this (filePath.toAuroraResourceInfo(), File.ReadAllBytes(filePath)) { }

        public override byte[] toBytes() {
            return fileArray;
        }
    }
}
