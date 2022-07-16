using AuroraIO.Source.Archives.BIFKey;
using AuroraIO.Source.Coders;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YAMLEncoding;

namespace AuroraIOTests.Source {

    [TestClass]
    public class BIFKeyTests {

        ResourceBundle resources = ResourceBundle.GetCurrent();

        public BIFKeyTable testKeyFile() {
            return BIFKeyFile.Read(Path.Combine(resources.Directory, "chitin.key"));
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
