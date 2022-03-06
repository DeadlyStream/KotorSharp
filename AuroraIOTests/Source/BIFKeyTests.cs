using AuroraIO.Source.Archives.BIFKey;
using AuroraIO.Source.Coders;
using AuroraIOTests.Source.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class BIFKeyTests {

        ASCIICoder coder = new ASCIICoder();

        public BIFKeyTable testKeyFile() {
            return BIFKeyFile.Read(Snapshot.ResourcePath());
        }

        [TestMethod]
        public void testReadKeyFile() {
            var bifKeyTable = testKeyFile();

            Snapshot.Verify(bifKeyTable);
        }

        [TestMethod]
        public void testExtractFromBIFArchive() {
            var bifKeyTable = testKeyFile();

            var archive = bifKeyTable["data\\2da.bif"];

            var file = archive.extract("appearance.2da").data;
            archive.Close();

            var coder = new _2DACoder();

            var table = coder.decode(file);
            Snapshot.Verify(table);
        }

        [TestMethod]
        public void testExtractAllFromBIFArchive() {
            var bifKeyTable = testKeyFile();

            var archive = bifKeyTable["data\\2da.bif"];

            archive.Load();

            var files = archive.extractAll();

            archive.Close();

            Snapshot.Verify(files);
        }
    }
}
