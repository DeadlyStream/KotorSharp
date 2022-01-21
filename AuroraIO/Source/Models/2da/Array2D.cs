using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using AuroraIO.Source.Models._2da;
using AuroraIO.Source.Common;

namespace AuroraIO.Models {
    public class Array2D: AuroraResource, ICollection<Array2D.Row>, ASCIIOutputProtocol {

        protected const String NullTerm = "****";

        //Structures
        public List<Row> rows = new List<Row>();
        public String[] columns { get; set; }
        private IndexMap<string> columnNameMap {
            get {
                return columns.generateIndexMap();
            }
        }

        //Optimization
        private Queue<int> emptyRows = new Queue<int>();

        protected String filePath;

        //Public properties

        public int rowCount {
            get {
                return rows.Count;
            }
        }

        public int Count => ((ICollection<Row>)rows).Count;

        public bool IsReadOnly => ((ICollection<Row>)rows).IsReadOnly;

        private void initialize(String[] columns, Row[] rows) {
            this.fileType = AuroraResourceType.TwoDA;
            this.columns = columns;
            this.rows = new List<Row>(rows);
        }

        public Array2D() {
            initialize(new string[0], new Row[0]);
        }

        public Array2D(String[] columns, String[][] rowValuesArray) {
            initialize(columns, rowValuesArray.Select(rowValues => {
                return new Row(columns, rowValues);
            }).ToArray());
        }

        public Array2D(String[] columns, Row[] rows) {
            initialize(columns, rows);
        }

        public Array2D(String[] columns) {
            initialize(columns, new Row[] { });
        }

        public Row rowWhereLabelValueIs(string columnName, string value) {
            Row[] foundRows = rows.Where(row => row[columnName] == value).Where(row => row != null).ToArray();
            if (foundRows.Length > 0) {
                return foundRows.First();
            } else {
                return null;
            }
        }

        public Row this[int key] {
            get {
                return rows.ElementAt(key);
            }
        }

        public Row addNewRow() {
            rows.Add(new Row(columns));
            return rows.Last();
        }

        public int getLastRowIndex() {
            return rows.Count - 1;
        }

        public void clearRowAtIndex(int index) {
            Row row = this[index];
            row.clear();
            emptyRows.Enqueue(index);
        }

        public void Add(Row item) {
            ((ICollection<Row>)rows).Add(item);
        }

        public void Clear() {
            ((ICollection<Row>)rows).Clear();
        }

        public bool Contains(Row item) {
            return ((ICollection<Row>)rows).Contains(item);
        }

        public void CopyTo(Row[] array, int arrayIndex) {
            ((ICollection<Row>)rows).CopyTo(array, arrayIndex);
        }

        public bool Remove(Row item) {
            return ((ICollection<Row>)rows).Remove(item);
        }

        public IEnumerator<Row> GetEnumerator() {
            return ((ICollection<Row>)rows).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((ICollection<Row>)rows).GetEnumerator();
        }

        public override byte[] toBytes() {
            var coder = new _2daCoder();
            return coder.encode(this);
        }

        public class Row {
            private IndexMap<String> columnMap;
            public String[] rowValues { get; private set; }

            internal Row(IndexMap<String> indexMap) {
                initialize(indexMap, new String[0]);
            }

            public Row(String[] columnNames) {
                initialize(columnNames.generateIndexMap(), new String[columnNames.Length]);
            }

            public Row(String[] columnNames, String[] rowData) {
                initialize(columnNames.generateIndexMap(), rowData);
            }

            private void initialize(IndexMap<String> columnMap,
                                    String[] rowValues) {
                this.columnMap = columnMap;
                this.rowValues = rowValues;
            }

            public String this[String key] {
                get {
                    return getValueForColumn(key);
                } set {
                    setValueForColumn(key, value);
                }
            }

            public String this[int index] {
                get {
                    return getValueAtIndex(index);
                } set {
                    setValueAtIndex(index, value);
                }
            }

            [Obsolete("deprecated, please use [] instead.")]
            public void setValueForColumn(string columnName, string value) {
                if (columnMap.Contains(columnName)) {
                    setValueAtIndex(columnMap[columnName], value);
                }
            }

            [Obsolete("deprecated, please use [] instead.")]
            public void setValueAtIndex(int index, String s) {
                if (s.Length > 0) {
                    rowValues[index] = s;
                } else {
                    rowValues[index] = NullTerm;
                }
            }

            [Obsolete("deprecated, please use [] instead.")]
            public String getValueForColumn(string columnName) {
                if (columnMap.Contains(columnName)) {
                    return getValueAtIndex(columnMap[columnName]);
                } else {
                    return null;
                }
            }

            [Obsolete("deprecated, please use [] instead.")]
            public String getValueAtIndex(int index) {
                return rowValues[index];
            }

            public int length() {
                return rowValues.Length;
            }

            public void clear() {
                for (int i = 0; i < rowValues.Length; i++) {
                    rowValues[i] = NullTerm;
                }
            }

            public Row copyRow() {
                Row newRow = new Row(columnMap);
                Row oldRow = this;

                for (int i = 0; i < newRow.length(); i++) {
                    newRow[i] = oldRow[i];
                }

                return newRow;
            }

            override public String ToString() {
                return String.Join("|", rowValues);
            }
        }

        public string asciiEncoding() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Join("\t", columns));
            sb.Append("\n");
            sb.Append(String.Join("\n", this.Select(r => {
                return String.Join("\t", r.rowValues.Select(value => value).ToArray());
            }).ToArray()));
            return sb.ToString();
        }
    }
}
