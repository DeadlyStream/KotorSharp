using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Collections {
    internal class BiffVariableResourceEntry {
        internal int fileSize;
        internal int fileOffset;

        internal BiffVariableResourceEntry(int fileOffset, int fileSize) {
            this.fileOffset = fileOffset;
            this.fileSize = fileSize;
        }
    }
}
