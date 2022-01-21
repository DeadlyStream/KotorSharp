using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Helpers {
    public class AuroraFile {
        public static bool isArchive(String filePath) {
            if (Path.HasExtension(filePath)) {
                return Path.GetExtension(filePath).toAuroraResourceType().isResourceCollection();
            } else {
                return false;
            }
        }
    }
}
