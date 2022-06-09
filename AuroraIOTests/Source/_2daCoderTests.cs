using AuroraIO.Source.Coders;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class _2daCoderTests {

        _2DACoder coder = new _2DACoder();


        [TestMethod]
        public void testDecodeGameFile() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testEncodeGameFile() {
            var table = coder.decode(Snapshot.DataResource());

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }
    }
}
