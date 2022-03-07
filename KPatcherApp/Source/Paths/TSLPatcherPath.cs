using AppToolbox.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Paths {
    class TSLPatcherPath: ApplicationPath {
        public readonly ApplicationPath AppendTLK;

        public TSLPatcherPath(String rootDirectory) : base(rootDirectory) {
            ApplicationPath root = rootDirectory;
            AppendTLK = root + "Append.tlk";
        }

        public static implicit operator TSLPatcherPath(String path) {
            return new TSLPatcherPath(path);
        }

        public static implicit operator String(TSLPatcherPath path) {
            return path.pathComponent;
        }

        public static TSLPatcherPath operator +(TSLPatcherPath originalPath, String newPath) {
            return originalPath.pathComponent + System.IO.Path.DirectorySeparatorChar + newPath;
        }

        public static TSLPatcherPath operator +(TSLPatcherPath originalPath, ApplicationPath newPath) {
            return originalPath.pathComponent + System.IO.Path.DirectorySeparatorChar + newPath.pathComponent;
        }
    }
}
