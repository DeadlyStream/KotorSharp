using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.ERF {
    public static class ERFFile {
        public enum Format {
            Auto,
            ERF,
            RIM,
            HAK,
            SAV,
            MOD
        }

        public static void CreateFromDirectory(string path, Format format = Format.Auto) {

        }

        public static void ExtractToDirectory(String path, Format format = Format.Auto) {

        }

        public static ERFArchive Load(string path, Format format = Format.Auto) {
            return null;
        }

        public static ERFArchive Write(string path, Format format = Format.Auto) {
            return null;
        }
    }
}
