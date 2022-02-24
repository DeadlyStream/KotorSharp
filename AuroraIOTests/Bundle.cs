using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace AuroraIOTests {
    public static class Bundle {
        public static string ProjectDirectory = getProjectDirectory();
        public static string SnapshotDirectory = Path.Combine(ProjectDirectory, "Snapshots");

        private static string getProjectDirectory([CallerFilePath] string className = "") {
            return Path.GetDirectoryName(className);
        }
    }
}
