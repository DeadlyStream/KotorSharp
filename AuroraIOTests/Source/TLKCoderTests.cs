using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.TLK;
using AuroraIOTests.Source.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuroraIOTests.Source {
    [TestClass]
    public class TLKCoderTests {
        TLKCoder coder = new TLKCoder();

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
        public void testDecodeEntryWithText() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeEntryWithSoundResref() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeEntryWithSoundLength() {
            var file = Snapshot.DataResource();
            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeMultipleEntries() {
            var file = Snapshot.DataResource();
            Snapshot.Verify(coder.decode(file));
        }
    }
}
