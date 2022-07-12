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

        //[TestMethod]
        //public void testRunPatcher() {
        //    VirtualFileInterface fileInterface = new VirtualFileInterface();
        //    var patchDataDirectory = Snapshot.PatchDataDirectory();
        //    var changesIniPath = Path.Combine(patchDataDirectory, "changes.ini");
        //    var rootGameDirectory = Snapshot.RootGameDirectory();

        //    fileInterface.LoadDirectory(patchDataDirectory);
            

        //    Patcher.Run(changesIniPath, rootGameDirectory, fileInterface, 0);

        //    Snapshot.Verify(fileInterface, true);
        //}

        [TestMethod]
        public void testTLKChanges() { 
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var patchDataDirectory = Snapshot.PatchDataDirectory();
            var changesIniPath = Path.Combine(patchDataDirectory, "changes.ini");
            var rootGameDirectory = Snapshot.RootGameDirectory();

            fileInterface.LoadDirectory(patchDataDirectory);
            fileInterface.LoadFile(Path.Combine(rootGameDirectory, "dialog.tlk"));

            Patcher.Run(changesIniPath, rootGameDirectory, fileInterface, 0);

            Snapshot.Verify(fileInterface);
        }

        [TestMethod]
        public void testInstallChanges() {
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var date = new DateTime(2003, 7, 15);

            var patchDataDirectory = Snapshot.PatchDataDirectory();
            var changesIniPath = Path.Combine(patchDataDirectory, "changes.ini");
            var rootGameDirectory = Snapshot.RootGameDirectory();

            fileInterface.LoadDirectory(patchDataDirectory);
            fileInterface.LoadDirectory(rootGameDirectory);

            Patcher.Run(changesIniPath, rootGameDirectory, fileInterface, date);

            Snapshot.Verify(fileInterface);
        }

        [TestMethod]
        public void testInstall2daChanges() {
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var date = new DateTime(2003, 7, 15);

            var patchDataDirectory = Snapshot.PatchDataDirectory();
            var changesIniPath = Path.Combine(patchDataDirectory, "changes.ini");
            var rootGameDirectory = Snapshot.RootGameDirectory();

            fileInterface.LoadDirectory(patchDataDirectory);
            fileInterface.LoadDirectory(rootGameDirectory);

            Patcher.Run(changesIniPath, rootGameDirectory, fileInterface, date);

            Snapshot.Verify(fileInterface);
        }
    }
}
