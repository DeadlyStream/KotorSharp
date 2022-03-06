using AuroraIO.Source.Coders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.BIFKey {
    public class BIFKeyTable: ASCIIOutputProtocol {

        private Dictionary<string, BIFArchive> archiveMap = new Dictionary<string, BIFArchive>();

        internal BIFKeyTable(Dictionary<string, BIFArchive> archiveMap) {
            this.archiveMap = archiveMap;
        }

        public BIFArchive this[string key] {
            get {
                return archiveMap[key];
            }
        }

        public string asciiEncoding(string indent = "") {
            StringBuilder sb = new StringBuilder();

            foreach(KeyValuePair<string, BIFArchive> pair in archiveMap) {
                sb.AppendFormat("{0}:\n", pair.Key);
                sb.AppendLine(pair.Value.asciiEncoding("  "));
            }

            return sb.ToString();
        }
    }
}
