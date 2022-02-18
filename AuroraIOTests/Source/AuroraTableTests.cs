using AuroraIO.Models;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models._2da;
using AuroraIOTests.Properties;
using AuroraIOTests.Source.Asserts;
using AuroraIOTests.Source.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace AuroraIOTests.Source {
    [TestClass]
    public class AuroraTableTests {

        [TestMethod]
        public void testWrite()
        {
            var array = AuroraTableStubs.stub1();

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testAddRow()
        {
            var array = AuroraTableStubs.stub1();
            var row = array.addRow();

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testAddRowValues() {
            var array = AuroraTableStubs.stub1();
            array.addRow(new string[] { "c1r4", "c2r4", "c3r4", "c4r4" });

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testAddRowMap()
        {
            var array = AuroraTableStubs.stub1();
            array.addRow(new Dictionary<string, string>
            {
                { "column0", "c0r4" },
                { "column1", "c1r4" },
                { "column2", "c2r4" },
                { "column3", "c3r4" }
            });

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testModifyValueInNewRowWithIndex()
        {
            var array = AuroraTableStubs.stub1();
            var row = array.addRow();
            row[0] = "mod";

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testModifyValueInNewRowWithKey()
        {
            var array = AuroraTableStubs.stub1();
            var row = array.addRow();
            row["column0"] = "mod";

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testModifyValueInExistingRowWithIndex()
        {
            var array = AuroraTableStubs.stub1();
            array[0][0] = "mod";

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testModifyValueInExistingRowWithKey()
        {
            var array = AuroraTableStubs.stub1();
            array[0]["column0"] = "mod";

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testModifyMultipleValuesInExistingRow()
        {
            var array = AuroraTableStubs.stub1();
            array[0]["column0"] = "mod";
            array[2]["column3"] = "mod";

            Snapshot.Verify(array);
        }

        [TestMethod]
        public void testClearValue()
        {
            var array = AuroraTableStubs.stub1();
            array[0]["column0"] = null;

            Snapshot.Verify(array);
        }
    }
}
