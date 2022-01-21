using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class AuroraTextObject: AuroraResource {
        private string text;

        public AuroraTextObject(AuroraResourceInfo resInfo, byte[] fileArray) {
            this.fileName = resInfo.resref;
            this.fileType = resInfo.resourceType;
            text = Encoding.ASCII.GetString(fileArray);
        }

        public override byte[] toBytes() {
            return Encoding.ASCII.GetBytes(text);
        }
    }
}
