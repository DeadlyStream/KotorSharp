using AppToolbox.Classes;
using KotorManifest.Paths;
using KPatcher.Source.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    class PathManager {

        public TSLPatchDataPath patchDataDirectory;
        public KotorRootPath gameDirectory;
        public TSLPatcherPath workingPatchDataDirectory;
        public readonly KotorRootPath temp;

        public readonly static PathManager shared = new PathManager();

        private PathManager() {
            temp = new KotorRootPath(Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + ".temp");
        }

        public String[] verifyAndReturnMissingComponents() {
            KotorRootPath rootPath = gameDirectory;
            return rootPath.mainGameDirectories.Select(
                item => {
                    return item.directoryName;
                }
            ).ToArray();
        }

        public void cleanForInstall() {
            if (Directory.Exists(temp)) {
                Directory.Delete(temp, true);
                Directory.CreateDirectory(temp);
            }
        }
    }
}