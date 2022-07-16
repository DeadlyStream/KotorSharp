using AuroraIO.Source.Coders;
using KPatcher.Source.Patcher;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherTests.Source {
    [TestClass]
    public class InstallPatcherTests {

        ResourceBundle resources = ResourceBundle.GetCurrent();

        [TestMethod]
        public void testPatchToArchive() {
            var archiveData = resources.GetFileBytes("test.erf");

            var archive = new ERFRIMCoder().decode(archiveData);
            string sourceDirectory = resources.GetDirectory("archive_tslpatchdata");
            string fileName = "testFile.txt";

            Dictionary<string, string> values = new Dictionary<string, string>();
            values["Replace0"] = fileName;

            var fileInterface = new VirtualFileInterface();
            fileInterface.LoadDirectory(sourceDirectory);

            InstallPatcher.ProcessArchiveInstall(archive, sourceDirectory, values, fileInterface);

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testPatchToDirectory() {

            string directory = "testDirectory";
            string sourceDirectory = "tslpatchdata";
            Dictionary<string, string> values = new Dictionary<string, string>();
            values["Replace0"] = "testFile.txt";

            var fileInterface = new VirtualFileInterface();
            fileInterface.WriteText("tslpatchdata\\testFile.txt", "this is a test file", true);

            InstallPatcher.ProcessDirectoryInstall(directory, sourceDirectory, values, fileInterface);

            Snapshot.Verify(fileInterface);
        }
    }
}
