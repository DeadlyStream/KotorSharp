using AuroraIO.Source.Models.Table;
using KPatcher.Source.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;

namespace KPatcher.Source.Patcher {
    internal static class _2DAPatcher {

        public static void ProcessAddRow(AuroraTable table, Dictionary<string, string> values, PatchInfo patchInfo) {

            var r = table.addRow();

            foreach (var pair in values) { 
                if (Regex.IsMatch(pair.Key, @"2DAMEMORY")) {
                    patchInfo.tokenRegistry[pair.Key] = (table.rowList.Count - 1).ToString();
                } else {
                    r[pair.Key] = pair.Value;
                } 
            }
        }

        public static void ProcessChangeRow(AuroraTable table, Dictionary<string, string> values, PatchInfo patchInfo) {
            int rowIndex = int.Parse(values["RowIndex"]);

            var r = table[rowIndex];

            foreach (var pair in values.Where((pair) => !Regex.IsMatch(pair.Key, @"RowIndex"))) {
                if (Regex.IsMatch(pair.Key, @"2DAMEMORY")) {
                    if (Regex.IsMatch(pair.Value, @"RowIndex")) {
                        patchInfo.tokenRegistry[pair.Key] = rowIndex.ToString();
                    }
                } else {
                    if (Regex.IsMatch(pair.Value, @"2DAMEMORY")) {
                        r[pair.Key] = patchInfo.tokenRegistry[pair.Value];
                    } else {
                        r[pair.Key] = pair.Value;
                    }
                }
            }
        }
    }
}
