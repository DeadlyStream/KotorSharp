using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.TLK;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuroraIOTests.Source {
    [TestClass]
    public class TLKCoderTests {

        ResourceBundle resources = ResourceBundle.GetCurrent();
        TLKCoder coder = new TLKCoder();

        [TestMethod]
        public void testEncodeGameFile() {
            var table = coder.decode(resources.GetFileBytes("gameFile.tlk"));

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testEncodeLanguageId() {
            var table = new TalkTable(TalkTable.LanguageID.English);
            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testEncodeEntryWithText() {
            var table = new TalkTable(TalkTable.LanguageID.English,
                new TalkTable.Entry[] {
                    new TalkTable.Entry(
                        "BadStrRef",
                        null,
                        0.0f
                    )
                });

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testEncodeEntryWithSoundResref() {
            var table = new TalkTable(TalkTable.LanguageID.English,
                new TalkTable.Entry[] {
                    new TalkTable.Entry(
                        null,
                        "snd_resref",
                        0.0f
                    )
                });

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testEncodeEntryWithSoundLength() {
            var table = new TalkTable(TalkTable.LanguageID.English,
                new TalkTable.Entry[] {
                    new TalkTable.Entry(
                        null,
                        null,
                        1.0f
                    )
                });

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testEncodeMultipleEntries() {
            var table = new TalkTable(TalkTable.LanguageID.English,
                new TalkTable.Entry[] {
                    new TalkTable.Entry(
                        "Bad StrRef",
                        null,
                        -1.0f
                    ),
                    new TalkTable.Entry(
                        "Bad StrRef",
                        null,
                        -1.0f
                    ),
                });

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testDecodeGameFile() {
            var file = resources.GetFileBytes("gameFile.tlk");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeEntryWithText() {
            var file = resources.GetFileBytes("textEntry.tlk");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeEntryWithSoundResref() {
            var file = resources.GetFileBytes("soundResref.tlk");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeEntryWithSoundLength() {
            var file = resources.GetFileBytes("soundLength.tlk");
            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeMultipleEntries() {
            var file = resources.GetFileBytes("multipleEntries.tlk");
            Snapshot.Verify(coder.decode(file));
        }
    }
}
