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

        string RootGameDirectory = GameRoot.Directory;
        ResourceBundle resources = ResourceBundle.GetCurrent();

        [TestMethod]
        public void testTLKChanges() { 
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var date = new DateTime(2003, 7, 15);

            Patcher.Run(resources.GetFilePath("tlk_tslpatchdata\\changes.ini"), RootGameDirectory, fileInterface, date);

            Snapshot.Verify(fileInterface);
        }
    }
}
