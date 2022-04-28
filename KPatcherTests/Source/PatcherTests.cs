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

namespace KPatcherTests.Source {

    [TestClass]
    public class PatcherTests {

        [TestMethod]
        public void testTLKChanges() {
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            Patcher.Run(Snapshot.PatchDataDirectory(), Snapshot.RootGameDirectory(), fileInterface, 0);

            var dialogTLKPath = Path.Combine(Snapshot.RootGameDirectory(), "dialog.tlk");
            var dialogTLK = fileInterface.ReadTLK(dialogTLKPath);
            Snapshot.Verify(dialogTLK);
        }
    }
}
