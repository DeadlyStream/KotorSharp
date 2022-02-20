using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.TLK;
using AuroraIOTests.Source.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source {

    [TestClass]
    public class TalkTableTests {
        [TestMethod]
        public void testEmpty() {
            var table = new TalkTable(TalkTable.LanguageID.English);

            Snapshot.Verify(table);
        }

        [TestMethod]
        public void testTableWithRow() {
            var table = new TalkTable(TalkTable.LanguageID.English, new TalkTable.Entry[] {
                new TalkTable.Entry(
                    "This is a test string",
                    "s_sound_000",
                    2.0f)
            });

            Snapshot.Verify(table);
        }
    }
}
