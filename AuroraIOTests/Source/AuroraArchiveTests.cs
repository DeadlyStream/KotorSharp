using AuroraIO.Source.Archives;
using AuroraIO.Source.Archives.ERFRIM;
using AuroraIO.Source.Coders;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using YAMLEncoding;

namespace AuroraIOTests.Source {
    [TestClass]
    public class AuroraArchiveTests {

        YAMLCoder coder = new YAMLCoder();
        string testERFFile() {
            return Snapshot.ResourcePath();
        }

        string testRIMFile() {
            return Snapshot.ResourcePath();
        }

        byte[] testGameERF() {
            return Snapshot.DataResource();
        }

        byte[] testGameRIM() {
            return Snapshot.DataResource();
        }

        [TestMethod]
        public void testReadGameERF() {
            var coder = new ERFRIMCoder();
            var archive = coder.decode(testGameERF());
            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testReadGameRIM() {
            var coder = new ERFRIMCoder();
            var archive = coder.decode(testGameRIM());
            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testWriteGameERF() {
            var coder = new ERFRIMCoder();
            var archive = coder.decode(testGameERF());
            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive);
        }

        [TestMethod]
        public void testWriteGameRIM() {
            var coder = new ERFRIMCoder();
            var archive = coder.decode(testGameRIM());
            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive);
        }


        [TestMethod]
        public void testAddEntry() {
            var archive = AuroraArchiveFile.Load(testERFFile());
            var file = new AuroraFileEntry("existingFile3.txt", Encoding.ASCII.GetBytes("This is an added file"));
            archive.Add(file);

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testRemoveEntry() {
            var archive = AuroraArchiveFile.Load(testERFFile());
            archive.Get("existingFile0.txt").Delete();

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testModifyEntry() {
            var archive = AuroraArchiveFile.Load(testERFFile());
            archive.Get("existingFile0.txt").Update((data) => {
                return Encoding.ASCII.GetBytes("This is a modified file");
            });

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testReadERF() {
            var archive = AuroraArchiveFile.Load(testERFFile(), ERFRIMCoder.Format.ERF);
            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testWriteERF() {
            var coder = new ERFRIMCoder();
            var archive = AuroraArchiveFile.Load(testERFFile(), ERFRIMCoder.Format.ERF);

            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive);
        }

        [TestMethod]
        public void testReadRIM() {
            var archive = AuroraArchiveFile.Load(testRIMFile(), ERFRIMCoder.Format.RIM);
            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testWriteRIM() {
            var coder = new ERFRIMCoder();
            var archive = AuroraArchiveFile.Load(testRIMFile(), ERFRIMCoder.Format.RIM);

            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive);
        }
    }
}
