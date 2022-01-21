using AuroraIO;
using KPatcher.Patching.Models;
using KPatcher.Patching.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {
    public class ModFilePatchSet: ModPackageComponent {
        private string fileName;
        private AuroraResourceType fileType;
        private ModFilePatchCommand[] commands;

        public ModFilePatchSet(string fileName, 
                               AuroraResourceType fileType,
                               ModFilePatchCommand[] commands)
        {
            this.commands = commands;
            this.fileName = fileName;
            this.fileType = fileType;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("patch {0} {1}:\n", fileName, fileType.stringValue()));
            foreach (ModFilePatchCommand command in commands) {
                sb.Append(command.ToString());
            }
            sb.Append(ReservedWord.General.End + "\n");
            return sb.ToString();
        }
    }
}
