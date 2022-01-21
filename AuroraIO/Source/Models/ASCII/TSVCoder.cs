using AuroraIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Source.Models.ASCII {
    public class TSVCoder {
        public Array2D decode(String stringInput) {
            Regex regex = new Regex("\r\n|\r|\n", RegexOptions.IgnoreCase);
            String[] rows = regex.Split(stringInput);

            List<String[]> cellMap = new List<string[]>();

            int maxColumnCount = 0;
            foreach (String rowValue in rows) {
                if (rowValue.Length > 0) {
                    String[] columnValues = rowValue.Split('\t').Where(item => item.Length > 0).ToArray();
                    cellMap.Add(columnValues);
                    maxColumnCount = Math.Max(columnValues.Length, maxColumnCount);
                }
            }

            String[] columns = cellMap.First().ToArray();

            cellMap.RemoveAt(0);

            return new Array2D(columns, cellMap.ToArray());
        }

        public String encode(Array2D array) {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Join("\t", array.columns));
            sb.Append("\n");
            sb.Append(String.Join("\n", array.Select(r => {
                return String.Join("\t", r.rowValues.Select(value => value).ToArray());
            }).ToArray()));
            return sb.ToString();
        }
    }
}
