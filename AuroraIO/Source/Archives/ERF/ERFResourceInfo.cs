using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Archives {
    public class ERFResourceInfo {
        public int fileOffset;
        public int fileSize;

        public ERFResourceInfo(int fileSize, int fileOffset) {
            this.fileSize = fileSize;
            this.fileOffset = fileOffset;
        }
    }
}
