using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source {
    public static class Bundle {
        public static string ProjectDirectory = getProjectDirectory();
        public static string SnapshotDirectory = Path.Combine(ProjectDirectory, "Snapshots");

        private static string getProjectDirectory([CallerFilePath] string className = "") {
            return Path.GetDirectoryName(className);
        }
    }
}
