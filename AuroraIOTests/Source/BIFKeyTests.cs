using AuroraIO.Source.Archives.BIFKey;
using AuroraIO.Source.Coders;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using YAMLEncoding;

namespace AuroraIOTests.Source {

    [TestClass]
    public class BIFKeyTests {

        YAMLCoder coder = new YAMLCoder();

        public BIFKeyTable testKeyFile() {
            return BIFKeyFile.Read(Snapshot.ResourcePath("dataResource"));
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
