using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.InstallCommands {
    public class SingleFileCopyInstallCommand: ModFileInstallCommand {
        private string newFileName;
        private string originalFileName;

        public SingleFileCopyInstallCommand(String originalFileName,
                                            String newFileName) {
            this.originalFileName = originalFileName;
            this.newFileName = newFileName;
        }

        public override string ToString() {
            String printOriginalFileName = originalFileName;
            StringBuilder sb = new StringBuilder();
            return String.Format("move {0}\n", originalFileName);
        }
    }
}
