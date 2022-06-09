using AuroraIO;
using AuroraIO.Source.Coders;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class SSFCoderTests {

        SSFCoder coder = new SSFCoder();

        Data gameFile() {
            return Snapshot.DataResource();
        }

        Data emptyFile() {
            return Snapshot.DataResource();
        }

        [TestMethod]
        public void testEncodeEmpty() {
            var data = emptyFile();
            var newData = coder.encode(coder.decode(data));
            Snapshot.Verify(coder.decode(newData));
        }

        [TestMethod]
        public void testEncodeGameFile() {
            var data = gameFile();
            var newData = coder.encode(coder.decode(data));
            Snapshot.Verify(coder.decode(newData));
        }

        [TestMethod]
        public void testDecodeEmpty() {
            var data = emptyFile();
            Snapshot.Verify(coder.decode(data));
        }

        [TestMethod]
        public void testDecodeGameFile() {
            var data = gameFile();
            Snapshot.Verify(coder.decode(data));
        }
    }
}
