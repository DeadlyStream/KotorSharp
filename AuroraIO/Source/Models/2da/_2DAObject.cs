using AuroraIO.Source.Coders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models._2da
{
    public class _2DAObject: ASCIIOutputProtocol {

        public class Row
        {
            public struct SetRowData
            {
                public string value;
                public int index;

                public SetRowData(string value, int index)
                {
                    this.value = value;
                    this.index = index;
                }
            }
            string[] columns;
            string[] rowData;
            Action<SetRowData> setRowAction;

            public Row(string[] columns,
                       string[] rowData,
                       Action<SetRowData> setRowAction)
            {
                this.columns = columns;
                this.rowData = rowData;
                this.setRowAction = setRowAction;
            }

            public string this[int index]
            {
                get
                {
                    return rowData[index];
                } set
                {
                    setRowAction(new SetRowData(value, index));
                }
            }

            public string this[string key]
            {
                get
                {
                    return rowData[columns.generateIndexMap()[key]];
                }
                set
                {
                    var indexMap = columns.generateIndexMap();
                    setRowAction(new SetRowData(value, indexMap[key]));
                }
            }
        }

        public string[] columns;
        public List<string[]> rowList { get; }

        public _2DAObject(string[] columns, string[][] rowData)
        {
            this.columns = columns;
            this.rowList = rowData.Select(singleRowData => singleRowData).ToList();
        }

        public Row this[int index]
        {
            get
            {
                return new Row(columns, rowList[index], (data) => {
                    rowList[index][data.index] = data.value.sanitize();
                });
            }
        }

        public Row addRow(string[] rowValues) {
            rowList.Add(rowValues);
            return this[rowList.Count - 1];
        }

        public Row addRow(Dictionary<string, string> rowMap)
        {
            string[] rowValues = new string[columns.Length];
            for(int i = 0; i < columns.Length; i++)
            {
                rowValues[i] = rowMap[columns[i]];
            }
            return addRow(rowValues);
        }

        public Row addRow()
        {
            var rowData = new string[columns.Length];
            for(int i = 0; i < rowData.Length; i++)
            {
                rowData[i] = "****";
            }
            return addRow(rowData);
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Join("\t", columns));
            sb.Append("\n");
            sb.Append(
                String.Join("\n", rowList.Select(row => 
                    String.Join("\t", row.Select(value => value ?? "****").ToArray())
                ).ToArray())
            );
            return sb.ToString();
        }
    }
}
