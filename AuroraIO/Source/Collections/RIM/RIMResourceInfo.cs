using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Collections {
    public class RIMResourceInfo {
        public int fileOffset;
        public int fileSize;

        public RIMResourceInfo(int fileOffset, int fileSize) {
            this.fileOffset = fileOffset;
            this.fileSize = fileSize;
        }
    }
}
