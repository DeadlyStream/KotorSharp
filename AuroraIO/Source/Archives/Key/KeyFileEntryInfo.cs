using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Archives {
    public class KeyFileEntryInfo {
        private UInt16 drives;
        public int fileSize;
        public Dictionary<AuroraResourceInfo, uint> fileMap;

        public KeyFileEntryInfo(int fileSize, UInt16 drives) {
            this.fileSize = fileSize;
            this.drives = drives;
            this.fileMap = new Dictionary<AuroraResourceInfo, uint>();
        }

        public byte[] toBytes(string fileName, String fileNames, int offsetToFileNames) {
            ByteArray byteArray = new ByteArray();
            byteArray.AddRange(BitConverter.GetBytes((uint)fileSize));
            int fileNameOffset = fileNames.IndexOf(fileName) + offsetToFileNames;
            byteArray.AddRange(BitConverter.GetBytes((uint)fileNameOffset));
            //Add 1 because I guess \0 are counted as part of the string
            byteArray.AddRange(BitConverter.GetBytes((UInt16)(fileName.Length + 1)));
            byteArray.AddRange(BitConverter.GetBytes((UInt16)drives));
            return byteArray.ToArray();
        }
    }
}
