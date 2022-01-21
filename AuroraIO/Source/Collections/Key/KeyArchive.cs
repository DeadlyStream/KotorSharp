using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Collections {
    public class KeyArchive: AuroraResourceCollection {
        private string fileVersion;
        private string fileDirectory;

        public string[] bifFilePaths {
            get {
                return bifTable.Keys.ToArray();
            }
        }

        private Dictionary<string, KeyFileEntryInfo> bifTable = new Dictionary<string, KeyFileEntryInfo>();
        private Dictionary<string, BiffArchive> loadedBifs = new Dictionary<string, BiffArchive>();

        public KeyArchive(string filePath) {
            byte[] fileArray = File.ReadAllBytes(filePath);

            this.fileDirectory = Regex.Replace(filePath, "[^\\\\]*$", "");
            String fullFileName = Regex.Match(filePath, "[^\\\\]*$").Value.ToLower();
            this.fileName = Regex.Match(fullFileName, "^[^.]*").Value.ToLower();
            this.fileType = Encoding.ASCII.GetString(fileArray, 0, 4).Replace(" ", "").ToLower().toAuroraResourceType();
            this.fileVersion = Encoding.ASCII.GetString(fileArray, 4, 4);

            int bifCount = (int)BitConverter.ToUInt32(fileArray, 8);
            int keyCount = (int)BitConverter.ToUInt32(fileArray, 12);

            int offsetToFileTable = (int)BitConverter.ToUInt32(fileArray, 16);
            int offsetToKeyTable = (int)BitConverter.ToUInt32(fileArray, 20);

            int buildYear = (int)BitConverter.ToUInt32(fileArray, 24);
            int buildDay = (int)BitConverter.ToUInt32(fileArray, 28);
            //Next 32 bytes reserved

            //Parse File Table
            int readingOffset = offsetToFileTable;

            Dictionary<uint, string> fileIndexMap = new Dictionary<uint, string>();
            for (uint i = 0; i < bifCount; i++) {
                int fileSize = (int)BitConverter.ToUInt32(fileArray, readingOffset);
                int fileNameOffset = (int)BitConverter.ToUInt32(fileArray, readingOffset + 4);
                int filenameSize = BitConverter.ToUInt16(fileArray, readingOffset + 8);
                string fileName = Encoding.ASCII.GetString(fileArray, fileNameOffset, filenameSize).Replace("\0", "");
                UInt16 drives = BitConverter.ToUInt16(fileArray, readingOffset + 10);
                KeyFileEntryInfo fileEntry = new KeyFileEntryInfo(fileSize, drives);
                bifTable[fileName] = fileEntry;
                fileIndexMap[i] = fileName;
                readingOffset += 12;
            }

            readingOffset = offsetToKeyTable;

            for (int i = 0; i < keyCount; i++) {
                string resref = Encoding.ASCII.GetString(fileArray, readingOffset, 16).Replace("\0", "");
                AuroraResourceType resourceType = (AuroraResourceType)BitConverter.ToUInt16(fileArray, readingOffset + 16);
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

                String bifName = fileIndexMap[fieldIndex];
                AuroraResourceInfo resInfo = new AuroraResourceInfo(resref, resourceType);
                bifTable[bifName].fileMap[resInfo] = resID;
                readingOffset += 22;
            }

            return;
        }

        public BiffArchive loadBifArchive(String bifPath) {
            if (bifTable.ContainsKey(bifPath)) {
                BiffArchive bifArchive;
                if (loadedBifs.ContainsKey(bifPath)) {
                    bifArchive = loadedBifs[bifPath];
                } else {
                    Dictionary<uint, AuroraResourceInfo> resIDMap = bifTable[bifPath].fileMap.ToDictionary(pair => pair.Value, pair => pair.Key);
                    bifArchive = new BiffArchive(bifPath, fileDirectory + bifPath, resIDMap);
                    loadedBifs[bifPath] = bifArchive;
                }
                return bifArchive;
            } else {
                return null;
            }
        }

        public void releaseBifArchive(String bifPath) {
            loadedBifs[bifPath] = null;
        }

        public void commit(BiffArchive bifArchive) {
            //Write bif to disk
            int index = bifTable.Keys.ToList().IndexOf(bifArchive.fileID);
            byte[] bifArchiveArray = bifArchive.toBytes(index);
            //File.WriteAllBytes(fileDirectory + bifArchive.fileID + ".new", bifArchiveArray);
            //update key
            bifTable[bifArchive.fileID].fileSize = bifArchiveArray.Length;
        }

        public override byte[] toBytes() {
            ByteArray byteArray = new ByteArray();

            //Write FileType
            byteArray.AddRange(Encoding.ASCII.GetBytes(fileType.stringValue().ToUpper().PadRight(4, ' ')));

            //Write FileVersion
            byteArray.AddRange(Encoding.ASCII.GetBytes(fileVersion.ToUpper().PadRight(4, ' ')));

            //Write the number of bifs
            int bifCount = bifTable.Keys.Count;
            byteArray.AddRange(BitConverter.GetBytes((uint)bifCount));

            //Write the number of resources
            int resourceCount = bifTable.Values.Select(keyInfo => keyInfo.fileMap.Count).Sum();
            byteArray.AddRange(BitConverter.GetBytes((uint)resourceCount));

            //Write fileTable offset
            int fileTableOffset = 64;
            byteArray.AddRange(BitConverter.GetBytes((uint)fileTableOffset));

            String fileNames = String.Join("\0", bifTable.Keys) + "\0";

            int offsetToFileNames = fileTableOffset + bifTable.Count * 12;

            //Write resourceTable offset
            int offsetToResources = offsetToFileNames + fileNames.Length;
            byteArray.AddRange(BitConverter.GetBytes((uint)offsetToResources));

            //Write Build Year
            byteArray.AddRange(BitConverter.GetBytes((uint)0));

            //Write Build Day
            byteArray.AddRange(BitConverter.GetBytes((uint)0));

            //Write 32 bytes of reserved space
            for (int i = 0; i < 32; i++) {
                byteArray.Add(0);
            }
  
            foreach (KeyValuePair<string, KeyFileEntryInfo> pair in bifTable) {
                byteArray.AddRange(pair.Value.toBytes(pair.Key, fileNames, offsetToFileNames));
            }

            byteArray.AddRange(Encoding.ASCII.GetBytes(fileNames));


            /*
            A unique ID number.  It is generated as follows: 

            Variable: ID = (x << 20) + y
            Fixed: ID = (x << 20) + (y << 14)

            x = [Index into File Table to specify a BIF]
            y = [Index into Variable or Fixed Resource Table in BIF](<< means bit shift left)
            */
            
            int x = 0;
            foreach (KeyValuePair<string, KeyFileEntryInfo> fileEntryInfoPair in bifTable) {
                int y = 0;
                foreach (KeyValuePair<AuroraResourceInfo, uint> pair in fileEntryInfoPair.Value.fileMap) {
                    uint resId = ((uint)x << 20) + (uint)y;
                    string resref = pair.Key.resref.Split(".".ToCharArray()).First();
                    byteArray.AddRange(Encoding.ASCII.GetBytes(resref.PadRight(16, '\0')));
                    byteArray.AddRange(BitConverter.GetBytes((UInt16)pair.Key.resourceType));
                    byteArray.AddRange(BitConverter.GetBytes((uint)resId));
                    y++;
                }
                x++;
            }
            
            return byteArray.ToArray();
        }

        public override void extractAll(string filePath) {
            throw new NotImplementedException();
        }
    }
}
