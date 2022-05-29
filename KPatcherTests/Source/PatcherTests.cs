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
        public void testRunPatcher() {
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            Patcher.Run(Snapshot.PatchDataDirectory(), Snapshot.RootGameDirectory(), fileInterface, 0);

            Snapshot.Verify(fileInterface, true);
        }

        [TestMethod]
        public void testTLKChanges() { 
            VirtualFileInterface fileInterface = new VirtualFileInterface();

            var date = new DateTime(2003, 7, 15);

            Patcher.Run(Snapshot.PatchDataDirectory(), Snapshot.RootGameDirectory(), fileInterface, date);

            Snapshot.Verify(fileInterface);
        }
    }
}
