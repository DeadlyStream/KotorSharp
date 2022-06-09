using AuroraIO.Source.Models.TLK;
using KPatcher.Source.Patcher;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMLEncoding;

namespace KPatcherTests.Source {

    [TestClass]
    public class TLKPatcherTests {

        [TestMethod]
        public void testPatchDialogFile() {
            TalkTable dialogTLK = new TalkTable(TalkTable.LanguageID.English, new TalkTable.Entry[] {
                new TalkTable.Entry("Text1", "resref1", 0.0f)
            });

            TalkTable appendTLK = new TalkTable(TalkTable.LanguageID.English, new TalkTable.Entry[] {
                new TalkTable.Entry("Text2", "resref2", 0.0f)
            });

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["StrRef0"] = "0";

            TokenRegistry tokenRegistry = new TokenRegistry();

            TLKPatcher.Process(dict, dialogTLK, appendTLK, new TokenRegistry());

            Snapshot.Verify(dialogTLK);
        }

        [TestMethod]
        public void testStoreTokenRegistry() {
            TalkTable dialogTLK = new TalkTable(TalkTable.LanguageID.English, new TalkTable.Entry[] {
                new TalkTable.Entry("Text1", "resref1", 0.0f),
                new TalkTable.Entry("Text2", "resref2", 0.0f),
            });

            TalkTable appendTLK = new TalkTable(TalkTable.LanguageID.English, new TalkTable.Entry[] {
                new TalkTable.Entry("Text3", "resref3", 0.0f),
                new TalkTable.Entry("Text4", "resref4", 0.0f)
            });

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["StrRef0"] = "0";
            dict["StrRef1"] = "1";

            TokenRegistry tokenRegistry = new TokenRegistry();

            TLKPatcher.Process(dict, dialogTLK, appendTLK, tokenRegistry);

            Snapshot.Verify(tokenRegistry);
        }
    }
}
