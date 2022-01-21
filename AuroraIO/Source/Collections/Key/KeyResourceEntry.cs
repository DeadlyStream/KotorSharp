using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Collections {
    public class KeyResourceEntry {
        public uint resId;
        private AuroraResourceType resourceType;

        public KeyResourceEntry(AuroraResourceType resourceType, uint resID) {
            this.resourceType = resourceType;
            this.resId = resID;
        }
    }
}
