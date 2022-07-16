using KPatcher.Source.Ini;
using KPatcher.Source.Patcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;
using YAMLEncoding;
using KSnapshot;

namespace KPatcherTests.Source {

    [TestClass]
    public class PatcherTests {

        ResourceBundle resources = ResourceBundle.GetCurrent();

        [TestMethod]
        public void testTLKChanges() { 
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var patchDataDirectory = "tlk_tslpatchdata";
            var changesIniPath = resources.GetFilePath(Path.Combine(patchDataDirectory, "changes.ini"));

            fileInterface.LoadDirectory(resources.GetDirectory(patchDataDirectory));
            fileInterface.LoadDirectory(GameRoot.Directory);

            Patcher.Run(changesIniPath, GameRoot.Directory, fileInterface, 0);

            Snapshot.Verify(fileInterface, true);
        }

        [TestMethod]
        public void testInstallChanges() {
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var date = new DateTime(2003, 7, 15);

            var patchDataDirectory = "install_tslpatchdata";
            var changesIniPath = resources.GetFilePath(Path.Combine(patchDataDirectory, "changes.ini"));

            fileInterface.LoadDirectory(resources.GetDirectory(patchDataDirectory));
            fileInterface.LoadDirectory(GameRoot.Directory);

            Patcher.Run(changesIniPath, GameRoot.Directory, fileInterface, date);

            Snapshot.Verify(fileInterface, true);
        }

        [TestMethod]
        public void testInstall2daChanges() {
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var date = new DateTime(2003, 7, 15);

            var patchDataDirectory = "2da_tslpatchdata";
            var changesIniPath = resources.GetFilePath(Path.Combine(patchDataDirectory, "changes.ini"));

            fileInterface.LoadDirectory(resources.GetDirectory(patchDataDirectory));
            fileInterface.LoadDirectory(GameRoot.Directory);

            Patcher.Run(changesIniPath, GameRoot.Directory, fileInterface, date);

            Snapshot.Verify(fileInterface, true);
        }
    }
}
