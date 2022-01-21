using AuroraIO.Models;
using AuroraIO.Source.Common;
using AuroraIO.Source.Models._2da;
using AuroraIOTests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuroraIOTests.Source.Models {
    [TestClass]
    public class _2daTests {
        [TestMethod]
        public void testReadAndWriteExisting2da() {
            byte[] portraits = Resources.portraits;

            _2daCoder _2daCoder = new _2daCoder();
            ASCIICoder asciiCoder = new ASCIICoder();

            Array2D expected2DA = _2daCoder.decode(portraits);
            Array2D new2da = _2daCoder.decode(_2daCoder.encode(expected2DA));
     
            Assert.AreEqual(asciiCoder.encode(expected2DA), asciiCoder.encode(new2da));
        }

        static Array2D test2daArray() {
            Array2D test2DA = new Array2D();
            test2DA.columns = new string[] {
                "column1", "column2", "column3", "column4"
            };

            for (int i = 0; i < 4; i++) {
                Array2D.Row r = test2DA.addNewRow();
                r[0] = string.Format("c1r{0}", i);
                r[1] = string.Format("c2r{0}", i);
                r[2] = string.Format("c3r{0}", i);
                r[3] = string.Format("c4r{0}", i);
            }
            return test2DA;
        }

        [TestMethod]
        public void testWrite2daToFile() {
            Array2D test2DA = test2daArray();

            _2daCoder coder = new _2daCoder();
            ASCIICoder asciiCoder = new ASCIICoder();

            string textValue = asciiCoder.encode(test2DA);

            Assert.AreEqual(textValue, "column1\tcolumn2\tcolumn3\tcolumn4\nc1r0\tc2r0\tc3r0\tc4r0\nc1r1\tc2r1\tc3r1\tc4r1\nc1r2\tc2r2\tc3r2\tc4r2\nc1r3\tc2r3\tc3r3\tc4r3");
        }

        [TestMethod]
        public void testAddValueTo2da() {
            _2daCoder _2daCoder = new _2daCoder();
            ASCIICoder asciiCoder = new ASCIICoder();

            Array2D test2DA = test2daArray();

            string modifiedValue = "modifiedValue";
            test2DA[3]["column1"] = modifiedValue;

            string textValue = asciiCoder.encode(test2DA);

            Assert.AreEqual(textValue, string.Format("column1\tcolumn2\tcolumn3\tcolumn4\nc1r0\tc2r0\tc3r0\tc4r0\nc1r1\tc2r1\tc3r1\tc4r1\nc1r2\tc2r2\tc3r2\tc4r2\n{0}\tc2r3\tc3r3\tc4r3", modifiedValue));
        }

        [TestMethod]
        public void testClearValuein2da() {
            _2daCoder _2daCoder = new _2daCoder();
            ASCIICoder asciiCoder = new ASCIICoder();

            Array2D test2DA = test2daArray();

            string modifiedValue = "****";
            test2DA[3]["column1"] = modifiedValue;

            string textValue = asciiCoder.encode(test2DA);

            Assert.AreEqual(textValue, string.Format("column1\tcolumn2\tcolumn3\tcolumn4\nc1r0\tc2r0\tc3r0\tc4r0\nc1r1\tc2r1\tc3r1\tc4r1\nc1r2\tc2r2\tc3r2\tc4r2\n{0}\tc2r3\tc3r3\tc4r3", modifiedValue));
        }
    }
}
