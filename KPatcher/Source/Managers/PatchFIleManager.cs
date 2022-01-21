using AppToolbox.Classes;
using AuroraIO;
using AuroraIO.Collections;
using AuroraIO.Models;
using AuroraIO.Source.Models._2da;
using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.GFF.Helpers;
using KotorManifest.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    class PatchFileManager {
        public static readonly PatchFileManager shared = new PatchFileManager();

        public void copyFile(String sourcePath, String filePath) {
            String directory = System.IO.Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            File.Copy(sourcePath, filePath);
        }

        public Array2D load2daFile(String fileName) {
            ApplicationPath[] paths = new ApplicationPath[] {
                PathManager.shared.temp.Override + fileName,
                PathManager.shared.gameDirectory.Override + fileName,
                PathManager.shared.patchDataDirectory + fileName
            };

            foreach (ApplicationPath path in paths) {
                if (File.Exists(path)) {
                    Log.debugLine(String.Format("Found {0} in {1}", fileName, path.parentDirectory.directoryName));
                    _2daCoder coder = new _2daCoder();
                    return coder.decodeFileAtPath(path);
                }
            }

            //If nothing works, load from bifs
            return null;
        }

        public void write2daFileWithName(Array2D table, string fileName) {
            ApplicationPath filePath = PathManager.shared.temp.Override + fileName;
            Log.infoLine(String.Format("Writing {0} to {1}", table.fileName, filePath));
            var coder = new _2daCoder();
            File.WriteAllBytes(filePath, coder.encode(table));
            table.writeToPath(filePath);
        }

        public GFFObject loadGFF(String fileName) {
            ApplicationPath[] paths = new ApplicationPath[] {
                PathManager.shared.temp.Override + fileName,
                PathManager.shared.gameDirectory.Override + fileName,
                PathManager.shared.patchDataDirectory + fileName
            };

            foreach (ApplicationPath path in paths) {
                if (File.Exists(path)) {
                    Log.debugLine(String.Format("Found {0} in {1}", fileName, path.parentDirectory.directoryName));
                    return new GFFCoder().decodeFileAtPath(path);
                }
            }
            return null;
            //throw new Exception(String.Format("{0} not found in any paths", fileName));
        }

        public void writeGFFFile(GFFObject gffObject, String filePath) {
            Log.infoLine(String.Format("Writing {0}", filePath));

            GFFCoder coder = new GFFCoder();
            File.WriteAllBytes(filePath, coder.encode(gffObject));
        }

        public TalkTable loadDialogTLK() {
            String dialogTLKFilePath = PathManager.shared.temp.DialogTLK;
            if (!File.Exists(dialogTLKFilePath)) {
                dialogTLKFilePath = PathManager.shared.gameDirectory.DialogTLK;   
            }
            Log.debugLine(String.Format("Loading {0}", dialogTLKFilePath));
            return new TalkTable(dialogTLKFilePath);
        }

        public void writeDialogTLK(TalkTable dialogTLK) {
            if (!Directory.Exists(PathManager.shared.temp)) {
                Log.debugLine(String.Format("{0} doesn't exist, creating...", PathManager.shared.temp));
                Directory.CreateDirectory(PathManager.shared.temp);
            }
            dialogTLK.writeToPath(PathManager.shared.temp.DialogTLK);
            Log.debugLine(String.Format("Wrote {0}", PathManager.shared.temp.DialogTLK));
        }
    }
}
