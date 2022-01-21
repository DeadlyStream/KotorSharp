using AppToolbox.Classes;
using AuroraIO.Collections;
using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.Helpers;
using KotorManifest.Paths;
using KPatcherBase.Source.Patcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    class InstallListProcessor {

        public struct Info {
            public Dictionary<String, String> fileMap;
        public Info(Dictionary<string, string> fileMap) {
                this.fileMap = fileMap;
            }
        }

        public static Info process(ChangesCollection changes) {
            //Dict of source path, to destination path
            Dictionary<String, String> fileMap = new Dictionary<string, string>();

            KotorRootPath gameRoot = PathManager.shared.gameDirectory;
            KotorRootPath tempRoot = PathManager.shared.temp;

            foreach (KeyValuePair<String, String[]> pair in changes.installList) {
                //Build directory from key (game and temp)
                foreach (String fileToInstall in pair.Value) {
                    ApplicationPath patchFilePath = PathManager.shared.workingPatchDataDirectory + fileToInstall;
                    ApplicationPath tempInstallDirectory = tempRoot + pair.Key;

                    //The directory being pointed to has an extension which indicates
                    //it's probably an archive
                    if (Path.HasExtension(tempInstallDirectory)) {
                        String lastDirName = tempInstallDirectory.pathComponents.Last();
                        tempInstallDirectory = tempInstallDirectory.parentDirectory + ("." + lastDirName);
                    }
                    ApplicationPath tempInstallPath = tempInstallDirectory + fileToInstall;

                    //install files
                    Log.infoLine(String.Format("Installing {0} to {1}", fileToInstall, tempInstallPath.parentDirectory.directoryName));
                    PatchFileManager.shared.copyFile(patchFilePath, tempInstallPath);

                    //kvp <installed_to, source> 
                    fileMap[tempInstallPath] = patchFilePath;
                }
            }

            return new Info(fileMap);
        } 
    }
}
