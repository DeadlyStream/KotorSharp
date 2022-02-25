using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.BIFKey {
    public class BIFArchive {
        private Dictionary<AuroraResourceName, AuroraFile> fileMap = new Dictionary<AuroraResourceName, AuroraFile>();

        public AuroraFile extract(string fileName) {
            return null;
        }

        public AuroraFile[] extractAll() {
            return null;
        }

        public void ExtractToDirectory(string path) {

        }
    }
}
