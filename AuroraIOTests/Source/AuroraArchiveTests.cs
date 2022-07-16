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

        // coder = new YAMLCoder();
        ERFRIMCoder coder = new ERFRIMCoder();

        ResourceBundle resources = ResourceBundle.GetCurrent();

        [TestMethod]
        public void testReadGameERF() {
            var file = resources.GetFileBytes("001EBO_dlg.erf");
            var archive = coder.decode(file);
            Snapshot.Verify(archive, true);
        }

        [TestMethod]
        public void testReadGameRIM() {
            var file = resources.GetFileBytes("001EBO.rim");
            var archive = coder.decode(file);
            Snapshot.Verify(archive, true);
        }

        [TestMethod]
        public void testWriteGameERF() {
            var file = resources.GetFileBytes("001EBO_dlg.erf");
            var archive = coder.decode(file);
            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive, true);
        }

        [TestMethod]
        public void testWriteGameRIM() {
            var file = resources.GetFileBytes("001EBO.rim");
            
            var archive = coder.decode(file);
            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive, true);
        }


        [TestMethod]
        public void testAddEntry() {
            var archive = coder.decode(resources.GetFileBytes("test.erf"));
            var fileName = "existingFile3.txt";
            var fileEntry = new AuroraFileEntry(fileName, resources.GetFileBytes(fileName));
            archive.Add(fileEntry);

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testRemoveEntry() {
            var archive = coder.decode(resources.GetFileBytes("test.erf"));
            archive.Get("existingFile0.txt").Delete();

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testModifyEntry() {
            var archive = coder.decode(resources.GetFileBytes("test.erf"));
            archive.Get("existingFile0.txt").Update((data) => {
                return Encoding.ASCII.GetBytes("This is a modified file");
            });

            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testReadERF() {
            var archive = coder.decode(resources.GetFileBytes("test.erf"));
            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testWriteERF() {
            var archive = coder.decode(resources.GetFileBytes("test.erf"));

            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive);
        }

        [TestMethod]
        public void testReadRIM() {
            var archive = coder.decode(resources.GetFileBytes("test.rim"));
            Snapshot.Verify(archive);
        }

        [TestMethod]
        public void testWriteRIM() {
            var archive = coder.decode(resources.GetFileBytes("test.rim"));

            var newArchive = coder.decode(coder.encode(archive));
            Snapshot.Verify(newArchive);
        }
    }
}
