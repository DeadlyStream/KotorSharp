using System;
using KPatcherBase.Source.Patcher;
using AppToolbox.Classes;
using KotorManifest.Paths;
using KPatcher.Source.Paths;

namespace KPatcher.Source.Managers
{
    public class ProgramController {
        enum Arguments {
            fileType, // -c or -n
            patchDataDirectoryPath,
            gameDirectoryPath,
            option //
        }

        static class FileType {
            public const String Changes = "-c";
            public const String Namespaces = "-n";
        }
        private ChangesCollection changes;

        public void acceptArgs(String[] args) {

            TSLPatchDataPath patchDataDirectory = args[(int)Arguments.patchDataDirectoryPath];
            KotorRootPath gameDirectory = args[(int)Arguments.gameDirectoryPath];
            PathManager.shared.patchDataDirectory = patchDataDirectory;
            PathManager.shared.gameDirectory = gameDirectory;

            String fileTypeArg = args[(int)Arguments.fileType];

            //Evaluate file Type
            String fileType = args[(int)Arguments.fileType];

            ChangesCollection changes = new ChangesCollection();
            PatchLoader loader = new PatchLoader();
            switch (fileType) {
                case FileType.Changes:
                    //Load changes ini            
                    changes = loader.readChanges(patchDataDirectory.ChangesINI);
                    PathManager.shared.workingPatchDataDirectory = patchDataDirectory;
                    break;
                case FileType.Namespaces:
                    //Load namespaces ini
                    String option = args[(int)Arguments.option];
                    NamespaceCollection namespaces = loader.readNamespaces(patchDataDirectory.NamespacesINI);
                    NamespaceInfo namespaceInfo = namespaces[option];
                    TSLPatcherPath workingPatchDataDirectory = patchDataDirectory + namespaceInfo.dataPath;
                    PathManager.shared.workingPatchDataDirectory = workingPatchDataDirectory;

                    ApplicationPath changesIni = workingPatchDataDirectory + namespaceInfo.iniName;

                    changes = loader.readChanges(changesIni);
                    break;
                default:
                    Console.WriteLine(String.Format("{0} not recognized", fileType));
                    break;
            }

            this.changes = changes;
            runInstall();
        }

        public void runInstall() {
            PathManager.shared.cleanForInstall();
            KeyValueManifest manifest = new KeyValueManifest();
            //Folder verify game directory
            //find override
            //find chitin.key
            //find dialog.tlk
            //find modules
            //find lips
            //basically find all of the folders
            //Anything that goes wrong here, log as a warning

            String gameDirectory = PathManager.shared.gameDirectory;
            Log.warn(String.Format("Verifying installation at {0}...", gameDirectory));
            String[] missingDirectories = PathManager.shared.verifyAndReturnMissingComponents();

            if (missingDirectories.Length > 0) {
                Log.warnLine("Failed");
                Log.warnLine("Missing directories, installation may not work");
                foreach (String directory in missingDirectories) {
                    Log.warnLine(String.Format("Missing {0}", directory));
                }
            } else {
                Log.warnLine("Done");
            }

            //Load in TLKList first
            //Load append.TLK
            //Load in Dialog.tlk because the append.tlk needs to be added to this
            //need to grab onto the strref values for later usage
            TLKProcessor.Info tlkInfo;
            try {
                tlkInfo = TLKProcessor.process(changes, manifest);
            } catch (Exception e) {
                Log.errorLine(e.Message);
                return;
            }

            //Install list
            //run through all of these and install the file in the respective folder
            //Always replace, but log warning if we encounter an exiting file
            InstallListProcessor.Info installInfo;
            try {
                installInfo = InstallListProcessor.process(changes);
            } catch (Exception e) {
                Log.errorLine(e.Message);
            }

            _2daProcessor.Info _2daInfo = new _2daProcessor.Info();

            //2da list
            try {
                _2daInfo = _2daProcessor.process(changes, manifest);
            } catch (Exception e) {
                Log.errorLine(e.Message);
            }

            //gff list
            //run through instructions
            //anything that has !Destination is where to install the particular file
            try {
                GFFProcessor.process(changes, manifest);
            } catch (Exception e) {
                Log.errorLine(e.Message);
            }

            //compile script list

            //need to read script source and inject tokens
            return;
        }
    }
}
