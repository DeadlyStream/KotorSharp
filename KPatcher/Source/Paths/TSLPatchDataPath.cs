using AppToolbox.Classes;
using KotorManifest.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Paths {
    class TSLPatchDataPath : TSLPatcherPath {
        public readonly ApplicationPath NamespacesINI;
        public readonly ApplicationPath ChangesINI;

        public TSLPatchDataPath(string rootDirectory) : base(rootDirectory) {
            ApplicationPath root = rootDirectory;
            NamespacesINI = root + "Namespaces.ini";
            ChangesINI = root + "Changes.ini";
        }

        public static implicit operator TSLPatchDataPath(String path) {
            return new TSLPatchDataPath(path);
        }

        public static implicit operator String(TSLPatchDataPath path) {
            return path.pathComponent;
        }
    }
}
