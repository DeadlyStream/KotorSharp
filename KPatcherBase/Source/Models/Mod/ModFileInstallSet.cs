using KPatcher.Patching.Models.InstallCommands;
using KPatcher.Patching.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {
    public class ModFileInstallSet: ModPackageComponent {
        private ModFileInstallCommand[] commands;
        private string installDirectory;

        public ModFileInstallSet(string installDirectory, ModFileInstallCommand[] commands) {
            this.installDirectory = installDirectory;
            this.commands = commands;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("install {0}:\n", installDirectory));
            foreach (ModFileInstallCommand command in commands) {
                sb.Append(command.ToString());
            }
            sb.Append(ReservedWord.General.End + "\n");
            return sb.ToString();
        }
    }
}
