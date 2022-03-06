using AuroraIO.Source.Models.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Source.Coders {
    public class _2DACoder {
        //Char parsing constants
        protected const char TabCharacter = (char)9;
        protected const char NullCharacter = (char)0;

        //Constants
        protected const String HeaderValue = "2DA V2.b\n";
        protected const String NullTerm = "****";
        protected const int HeaderLength = 9;

        public AuroraTable decode(byte[] byteArray) {

            int i = HeaderLength;
            char c = TabCharacter;

            ///
            /// Read Columns
            ///
            List<String> columnNames = new List<String>();
            StringBuilder s = new StringBuilder();

            while (c != NullCharacter)
            {
                c = (char)byteArray[i++];
                //Break the loop
                if (c == NullCharacter)
                {
                    break;
                    //End of string, create new
                }
                else if (c == TabCharacter)
                {
                    columnNames.Add(s.ToString());
                    s.Clear();
                    //Append character to stringbuilder
                }
                else
                {
                    s.Append(c.ToString());
                }
            }

            int numberOfColumns = columnNames.Count;

            ///
            /// Read Rows
            ///

            //Get the length
            int numberOfRows = (int)BitConverter.ToUInt32(byteArray, i);

            string[][] rows = new string[numberOfRows][];
            i += 4;
            c = TabCharacter;
            int tabCount = 0;
            while (tabCount < numberOfRows)
            {
                c = (char)byteArray[i++];
                //The strings here are always row numbers, I'm essentially bypassing all of this info
                if (c == TabCharacter)
                {
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
            while (i < tableDataLength)
            {
                int columnIndex = (i / 2) % (numberOfColumns);
                int rowIndex = (i / 2) / numberOfColumns;

                //If we've read enough data to complete a row
                //start a new row
                if (columnIndex == 0)
                {
                    rowValues = new String[numberOfColumns];
                }

                UInt16 offsetToString = BitConverter.ToUInt16(byteArray, i + RowPointerOffset);
                int offsetInArray = offsetToString + StringDataOffset;
                String stringValue = Encoding.ASCII.GetNullTerminatedString(byteArray, offsetInArray);
                if (stringValue.Length == 0)
                {
                    stringValue = NullTerm;
                }
                //Set the string value in the row
                rowValues[columnIndex] = stringValue;

                if (columnIndex == numberOfColumns - 1)
                {
                    rows[rowIndex] = rowValues;
                }
                i += 2;
            }

            return new AuroraTable(columnNames.ToArray(), rows);
        }

        public byte[] encode(AuroraTable obj)
        {
            Data newFileArray = new Data();

            //AddHeader
            newFileArray.AddRange(Encoding.ASCII.GetBytes(HeaderValue));

            //Write Column Names
            String columnNameList = String.Join("\t", obj.columns) + "\t";
            newFileArray.AddRange(Encoding.ASCII.GetBytes(columnNameList));

            //Add null char
            newFileArray.Add(0);

            //Write Rows
            newFileArray.AddRange(BitConverter.GetBytes((UInt32)obj.rowList.Count));

            StringBuilder s = new StringBuilder();
            for (int i = 0; i < obj.rowList.Count; i++)
            {
                s.Append(i);
                s.Append("\t");
            }

            newFileArray.AddRange(Encoding.ASCII.GetBytes(s.ToString()));

            Dictionary<string, int> stringMap = new Dictionary<string, int>();
            int currentOffset = 0;
            foreach (string[] rowData in obj.rowList)
            {
                foreach(String value in rowData)
                {
                    String stringValue = value ?? "";

                    //if stringValue is any sequence of asterisks (*) replace with ""
                    var nullRegex = new Regex("\\**");
                    stringValue = nullRegex.Replace(stringValue, "");
                    
                    //If string is already map then find offset and write to fileArray
                    if (stringMap.ContainsKey(stringValue))
                    {
                        int offset = stringMap[stringValue];
                        newFileArray.AddRange(BitConverter.GetBytes((UInt16)offset));
                    } else
                    {
                        //If string is not in map, then add it to map and advance the currentOffset
                        int offset = currentOffset;
                        stringMap[stringValue] = offset;
                        newFileArray.AddRange(BitConverter.GetBytes((UInt16)offset));
                        currentOffset += stringValue.Length + 1;
                    }
                }
            }

            //Two null chars
            newFileArray.Add(0);
            newFileArray.Add(0);

            //Sort the dictionary keys by the values and convert to a string

            var flattenedStringArray = stringMap.OrderBy(pair => pair.Value)
                .Select(pair => pair.Key).ToArray();

            newFileArray.AddRange(Encoding.ASCII.GetBytes(String.Join("\0", flattenedStringArray) + "\0"));
            return newFileArray.ToArray();
        }
    }
}
