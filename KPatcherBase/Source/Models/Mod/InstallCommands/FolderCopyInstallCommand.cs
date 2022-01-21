using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.InstallCommands {
    class FolderCopyInstallCommand : ModFileInstallCommand {
        private string folderName;

        public FolderCopyInstallCommand(string folderName) {
            this.folderName = folderName;
        }
    }
}
