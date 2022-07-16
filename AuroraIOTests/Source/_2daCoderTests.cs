using AuroraIO.Source.Coders;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class _2daCoderTests {

        ResourceBundle resourceBundle = ResourceBundle.GetCurrent();

        _2DACoder coder = new _2DACoder();


        [TestMethod]
        public void testDecodeGameFile() {
            var file = resourceBundle.GetFileBytes("heads.2da");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testEncodeGameFile() {
            var file = resourceBundle.GetFileBytes("heads.2da");
            var table = coder.decode(file);

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }
    }
}
