using AppToolbox.Classes;
using AuroraIO.Models;
using KotorManifest.Paths;
using KPatcher.Source.Exceptions;
using KPatcher.Source.Paths;
using KPatcherBase.Source.Patcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    public class TLKProcessor {

        public struct Info {}

        public static Info process(ChangesCollection changes, KeyValueManifest manifest) {
            KotorRootPath gameRoot = PathManager.shared.gameDirectory;
            TSLPatcherPath patchDataPath = PathManager.shared.workingPatchDataDirectory;
            //Load append.tlk
            TalkTable appendTLK;
            try {
                 appendTLK = new TalkTable(patchDataPath.AppendTLK);
            } catch {
                throw new FileNotOpenedException(patchDataPath.AppendTLK);
            }

            //load dialog.tlk
            TalkTable dialogTLK;

            try {
                dialogTLK = PatchFileManager.shared.loadDialogTLK();
            } catch {
                throw new FileNotOpenedException(gameRoot.DialogTLK);
            }

            //apply append.tlk to dialog.tlk
            foreach (KeyValuePair<String, int> pair in changes.tlkList) {
                TlkStrRefEntry entry = appendTLK[pair.Value];
                dialogTLK.Add(entry);
                Log.infoLine(String.Format("Adding {0} to dialog.tlk", pair.Key));
                manifest[pair.Key] = dialogTLK.Count - 1;
            }

            return new Info();
        }
    }
}
