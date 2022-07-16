using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Table;
using KPatcher.Source.Patcher;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherTests.Source {
    [TestClass]
    public class _2daPatcherTests {

        ResourceBundle resources = ResourceBundle.GetCurrent();

        [TestMethod]
        public void testAddRow() {
            var tableData = resources.GetFileBytes("heads.2da");
            var table = new _2DACoder().decode(tableData);

            var values = new Dictionary<string, string>();

            values["head"] = "NewRow";

            _2DAPatcher.ProcessAddRow(table, values, new TokenRegistry());

            Snapshot.Verify(table);
        }

        [TestMethod]
        public void testChangeRow() {
            var tableData = resources.GetFileBytes("heads.2da");
            var table = new _2DACoder().decode(tableData);

            var values = new Dictionary<string, string>();

            values["RowIndex"] = "10";
            values["head"] = "Modified_Row";

            _2DAPatcher.ProcessChangeRow(table, values, new TokenRegistry());

            Snapshot.Verify(table);
        }

        [TestMethod]
        public void testStoreRowIndexInTokenRegistry() {
            var tableData = resources.GetFileBytes("heads.2da");
            var table = new _2DACoder().decode(tableData);

            var values = new Dictionary<string, string>();

            values["2DAMEMORY0"] = "RowIndex";

            var tokenRegistry = new TokenRegistry();
            _2DAPatcher.ProcessAddRow(table, values, tokenRegistry);

            Snapshot.Verify(tokenRegistry, true);
        }

        [TestMethod]
        public void testSetValueFromTokenRegistry() {
            var tableData = resources.GetFileBytes("heads.2da");
            var table = new _2DACoder().decode(tableData);
            var tokenRegistry = new TokenRegistry();

            var values = new Dictionary<string, string>();

            tokenRegistry["2DAMEMORY0"] = "25";
            values["RowIndex"] = "10";
            values["head"] = "Modified_Row";
            values["alttexture"] = "2DAMEMORY0";

            _2DAPatcher.ProcessChangeRow(table, values, tokenRegistry);

            Snapshot.Verify(table, true);
        }

        //        Lookup existing 2da in override
        //* Extract 2da file from 2da.bif
        // Add row
        // Modify existing row values
        // Delete row
        // Add column
        // Modify existing column
        //Delete column
        //Capture tokens from adding/modifying rows or columns
        //Use stored tokens as values
        //write modified file to directory/archive
    }
}
