using AuroraIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Source.Coders {
    public class Array2DCoder: AuroraCoder<Array2D> {

        //Char parsing constants
        protected const char TabCharacter = (char)9;
        protected const char NullCharacter = (char)0;

        //Constants
        protected const String HeaderValue = "2DA V2.b\n";
        protected const String NullTerm = "****";
        protected const int HeaderLength = 9;

        public override Array2D decode(byte[] byteArray) {

            int i = HeaderLength;
            char c = TabCharacter;

            ///
            /// Read Columns
            ///
            List<String> columnNames = new List<String>();
            StringBuilder s = new StringBuilder();

            while (c != NullCharacter) {
                c = (char)byteArray[i++];
                //Break the loop
                if (c == NullCharacter) {
                    break;
                    //End of string, create new
                } else if (c == TabCharacter) {
                    columnNames.Add(s.ToString());
                    s.Clear();
                    //Append character to stringbuilder
                } else {
                    s.Append(c.ToString());
                }
            }

            int numberOfColumns = columnNames.Count;

            ///
            /// Read Rows
            ///

            //Get the length
            int numberOfRows = (int)BitConverter.ToUInt32(byteArray, i);
            Array2D.Row[] rows = new Array2D.Row[numberOfRows];
            i += 4;
            c = TabCharacter;
            int tabCount = 0;
            while (tabCount < numberOfRows) {
                c = (char)byteArray[i++];
                //The strings here are always row numbers, I'm essentially bypassing all of this info
                if (c == TabCharacter) {
                    tabCount++;
                }
            }

            //The offset start for Row data
            int RowPointerOffset = i;

            //Get the offset for the string data because we're going to need to know
            //where to point to to build up the array data
            int StringDataOffset = (numberOfColumns * numberOfRows) * 2 + RowPointerOffset + 2;

            ///
            /// Read Table Data
            ///
            i = 0;

            int tableDataLength = (numberOfColumns * numberOfRows) * 2;

            String[] rowValues = new String[numberOfColumns];
            while (i < tableDataLength) {
                int columnIndex = (i / 2) % (numberOfColumns);
                int rowIndex = (i / 2) / numberOfColumns;

                //If we've read enough data to complete a row
                //start a new row
                if (columnIndex == 0) {
                    rowValues = new String[numberOfColumns];
                }

                UInt16 offsetToString = BitConverter.ToUInt16(byteArray, i + RowPointerOffset);
                int offsetInArray = offsetToString + StringDataOffset;
                String stringValue = Encoding.ASCII.GetNullTerminatedString(byteArray, offsetInArray);
                if (stringValue.Length == 0) {
                    stringValue = NullTerm;
                }
                //Set the string value in the row
                rowValues[columnIndex] = stringValue;

                if (columnIndex == numberOfColumns - 1) {
                    rows[rowIndex] = new Array2D.Row(columnNames.ToArray(), rowValues);
                }
                i += 2;
            }

            return new Array2D(columnNames.ToArray(), rows.ToArray());
        }

        public override byte[] encode(Array2D obj) {
            ByteArray newFileArray = new ByteArray();

            //AddHeader
            newFileArray.AddRange(Encoding.ASCII.GetBytes(HeaderValue));

            //Write Column Names
            String columnNameList = String.Join("\t", obj.columns) + "\t";
            newFileArray.AddRange(Encoding.ASCII.GetBytes(columnNameList));

            //Add null char
            newFileArray.Add(0);

            //Write Rows
            newFileArray.AddRange(BitConverter.GetBytes((UInt32)obj.rows.Count));

            StringBuilder s = new StringBuilder();
            for (int i = 0; i < obj.rows.Count; i++) {
                s.Append(i);
                s.Append("\t");
            }

            newFileArray.AddRange(Encoding.ASCII.GetBytes(s.ToString()));

            //Okay, now the tricky part

            //Make a set to put all of the string values into
            StringDataSet stringDataSet = new StringDataSet();

            //Grab all of the string values and add them to the set
            foreach (Array2D.Row row in obj) {
                for (int i = 0; i < row.length(); i++) {
                    stringDataSet.addString(row[i]);
                }
            }

            //Build a string from StringDataSet
            String stringList = stringDataSet.stringList();

            foreach (Array2D.Row row in obj) {
                for (int i = 0; i < row.length(); i++) {
                    String stringValue = row[i];
                    int offset;
                    if (NullTerm.Equals(stringValue) || stringValue == null) {
                        offset = 0;
                    } else {
                        stringValue = stringValue.Replace(".", "\\.");
                        Regex regex = new Regex(stringValue + "\0");
                        Match m = regex.Match(stringList);
                        offset = m.Index;
                    }

                    newFileArray.AddRange(BitConverter.GetBytes((UInt16)offset));
                }
            }

            //Two null chars
            newFileArray.Add(0);
            newFileArray.Add(0);

            newFileArray.AddRange(Encoding.ASCII.GetBytes(stringList));
            return newFileArray.ToArray();
        }

        public class StringDataSet : HashSet<String> {

            public void addString(String stringData) {
                if (stringData == NullTerm) {
                    stringData = "";
                }
                Add(stringData);
            }

            public String stringList() {
                return "\0" + String.Join("\0", this.ToArray()).Replace("\n", "").Replace("\r", "") + "\0";
            }
        }
    }
}
