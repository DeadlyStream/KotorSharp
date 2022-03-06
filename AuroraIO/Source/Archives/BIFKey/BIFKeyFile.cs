using AuroraIO.Models.Base;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Archives.BIFKey {
    public static class BIFKeyFile {
        public static BIFKeyTable Read(string path) {

            byte[] fileArray = File.ReadAllBytes(path);

            AuroraResourceType fileType = Encoding.ASCII.GetString(fileArray, 0, 4).Trim();
            string fileVersion = Encoding.ASCII.GetString(fileArray, 4, 4).Trim();

            int bifCount = (int)BitConverter.ToUInt32(fileArray, 8);
            int keyCount = (int)BitConverter.ToUInt32(fileArray, 12);

            int offsetToFileTable = (int)BitConverter.ToUInt32(fileArray, 16);
            int offsetToKeyTable = (int)BitConverter.ToUInt32(fileArray, 20);

            int buildYear = (int)BitConverter.ToUInt32(fileArray, 24);
            int buildDay = (int)BitConverter.ToUInt32(fileArray, 28);
            //Next 32 bytes reserved

            int readingOffset = 0;

            //Parse File Table

            readingOffset = offsetToKeyTable;

            Dictionary<string, Dictionary<uint, AuroraResourceName>> fileMap = new Dictionary<string, Dictionary<uint, AuroraResourceName>>();

            for (int i = 0; i < keyCount; i++) {
                string resref = Encoding.ASCII.GetString(fileArray, readingOffset, 16).Replace("\0", "");
                AuroraResourceID resourceType = (AuroraResourceID)BitConverter.ToUInt16(fileArray, readingOffset + 16);
                /*
                A unique ID number.  It is generated as follows: 

                Variable: ID = (x << 20) + y
                Fixed: ID = (x << 20) + (y << 14)

                x = [Index into File Table to specify a BIF]
                y = [Index into Variable or Fixed Resource Table in BIF](<< means bit shift left)
                */
                uint resID = BitConverter.ToUInt32(fileArray, readingOffset + 18);
                uint fieldIndex = resID >> 20;
                uint resourceIndex = fieldIndex << 20 ^ resID;


                int fileEntryOffset = offsetToFileTable + (int)fieldIndex * 12;
                int fileSize = (int)BitConverter.ToUInt32(fileArray, fileEntryOffset);
                int fileNameOffset = (int)BitConverter.ToUInt32(fileArray, fileEntryOffset + 4);
                int filenameSize = BitConverter.ToUInt16(fileArray, fileEntryOffset + 8);
                string fileName = Encoding.ASCII.GetString(fileArray, fileNameOffset, filenameSize).Trim(new char[] { '\0' });
                UInt16 drives = BitConverter.ToUInt16(fileArray, fileEntryOffset + 10);

                var dict = fileMap.ContainsKey(fileName) ? fileMap[fileName] : new Dictionary<uint, AuroraResourceName>();
                dict[resourceIndex] = new AuroraResourceName(resref, resourceType);
                fileMap[fileName] = dict;
                readingOffset += 22;
            }

            return new BIFKeyTable(fileMap.ToDictionary(bifPair => bifPair.Key,
                bifPair => {
                    return new BIFArchive(bifPair.Value.ToDictionary(pair => pair.Value, pair => pair.Key),
                        Path.Combine(Path.GetDirectoryName(path), bifPair.Key)
                    );;
                }
            ));
        }
    }
}
