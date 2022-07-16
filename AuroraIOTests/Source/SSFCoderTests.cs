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

        ResourceBundle resources = ResourceBundle.GetCurrent();

        SSFCoder coder = new SSFCoder();


        [TestMethod]
        public void testEncodeEmpty() {
            var data = resources.GetFileBytes("empty.ssf");
            var newData = coder.encode(coder.decode(data));
            Snapshot.Verify(coder.decode(newData));
        }

        [TestMethod]
        public void testEncodeGameFile() {
            var data = resources.GetFileBytes("gameFile.ssf");
            var newData = coder.encode(coder.decode(data));
            Snapshot.Verify(coder.decode(newData));
        }

        [TestMethod]
        public void testDecodeEmpty() {
            var data = resources.GetFileBytes("empty.ssf");
            Snapshot.Verify(coder.decode(data));
        }

        [TestMethod]
        public void testDecodeGameFile() {
            var data = resources.GetFileBytes("gameFile.ssf");
            Snapshot.Verify(coder.decode(data));
        }
    }
}
