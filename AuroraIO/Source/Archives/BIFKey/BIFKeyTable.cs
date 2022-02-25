using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.BIFKey {
    public class BIFKeyTable {
        private Dictionary<string, BIFArchive> archiveMap = new Dictionary<string, BIFArchive>();

        public BIFArchive this[string key] {
            get {
                return archiveMap[key];
            }
        }
    }
}
