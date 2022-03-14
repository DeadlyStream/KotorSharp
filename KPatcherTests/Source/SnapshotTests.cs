using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherTests.Source {
    [TestClass]
    public class SnapshotTests {

        [TestMethod]
        public void testSnapshotBaseDirectory() {
            var path = Snapshot.ResourceDirectory();

            Snapshot.VerifyDirectory(path);
        }
    }
}
