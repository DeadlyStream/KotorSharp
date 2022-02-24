using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Archives {
    public class BiffArchive {
        private Dictionary<AuroraResourceInfo, BiffVariableResourceEntry> entries = new Dictionary<AuroraResourceInfo, BiffVariableResourceEntry>();
        private Dictionary<AuroraResourceInfo, AuroraResource> pendingEntries = new Dictionary<AuroraResourceInfo, AuroraResource>();
        private string fileType;
        private string fileVersion;
        private string filePath;
        public string fileID;
        private byte[] fileArray;

        public BiffArchive(string fileID, string filePath, Dictionary<uint, AuroraResourceInfo> resEntries) {
            byte[] fileArray = File.ReadAllBytes(filePath);

            this.fileID = fileID;
            this.filePath = filePath;
            this.fileType = Encoding.ASCII.GetString(fileArray, 0, 4).Replace(" ", "").ToLower();
            this.fileVersion = Encoding.ASCII.GetString(fileArray, 4, 4);

            int variableResourceCount = (int)BitConverter.ToUInt32(fileArray, 8);
            int fixedResourceCount = (int)BitConverter.ToUInt32(fileArray, 12);
            int variableTableOffset = (int)BitConverter.ToUInt32(fileArray, 16);

            int readingOffset = variableTableOffset;
            for (int i = 0; i < variableResourceCount; i++) {
                uint id = BitConverter.ToUInt32(fileArray, readingOffset);
                string resref = resEntries[id].resref;
                uint offset = BitConverter.ToUInt32(fileArray, readingOffset + 4);
                uint fileSize = BitConverter.ToUInt32(fileArray, readingOffset + 8);
                AuroraResourceType resourceType = (AuroraResourceType)BitConverter.ToUInt32(fileArray, readingOffset + 12);    
                entries[new AuroraResourceInfo(resref, resourceType)] = new BiffVariableResourceEntry((int)offset, (int)fileSize);
                readingOffset += 16;
            }
        }

        public void preloadResources() {

        }

        public void unloadResources() {

        }

        public void addResource(AuroraResource resource) {
            AuroraResourceInfo resInfo = new AuroraResourceInfo(resource.fileName, resource.fileType);
            byte[] fileBytes = resource.toBytes();
            entries[resInfo] = new BiffVariableResourceEntry((int)-1, fileBytes.Length);
            pendingEntries[resInfo] = resource;
        }

        public AuroraResource extractResource(string resourceID) {
            string resref = Regex.Match(resourceID, "^[^.]*").Value.ToLower();
            AuroraResourceType resourceType = Regex.Match(resourceID, "[^.]*$").Value.toAuroraResourceType();
            AuroraResourceInfo resInfo = new AuroraResourceInfo(resref, resourceType);
            BiffVariableResourceEntry entry = entries[resInfo];
            if (fileArray == null) {
                fileArray = File.ReadAllBytes(this.filePath);
            }
            byte[] individualFileArray = new byte[entry.fileSize];
            int offset = (int)entry.fileOffset;
            Buffer.BlockCopy(fileArray, (int)offset, individualFileArray, 0, (int)entry.fileSize);
            return AuroraResourceLoader.loadFile(new AuroraResourceInfo(resref, resourceType), individualFileArray);
        }

        internal byte[] toBytes(int x) {
            ByteArray byteArray = new ByteArray();

            //Write FileType
            byteArray.AddRange(Encoding.ASCII.GetBytes(fileType.PadRight(4).ToUpper()));

            //Write FileVersion
            byteArray.AddRange(Encoding.ASCII.GetBytes(fileVersion.PadRight(4)));

            //Write resource count
            int resEntryCount = entries.Count;
            byteArray.AddRange(BitConverter.GetBytes((uint)resEntryCount));

            byteArray.AddRange(BitConverter.GetBytes(0));

            //VariableTable offset
            byteArray.AddRange(BitConverter.GetBytes((uint)20));

            int startingResHeapOffset = resEntryCount * 16 + 20;
            uint currentResHeapOffset = (uint)startingResHeapOffset;
            int y = 0;

            foreach (KeyValuePair<AuroraResourceInfo, BiffVariableResourceEntry> pair in entries) {
                uint resId = ((uint)x << 20) + (uint)y;

                byteArray.AddRange(BitConverter.GetBytes((uint)resId));
                byteArray.AddRange(BitConverter.GetBytes(currentResHeapOffset));
                byteArray.AddRange(BitConverter.GetBytes((uint)pair.Value.fileSize));
                byteArray.AddRange(BitConverter.GetBytes((uint)pair.Key.resourceType));
                currentResHeapOffset += (uint)pair.Value.fileSize;
                y++;
            }

            int fileSize = (int)(new FileInfo(this.filePath).Length);
            byte[] fileArray = File.ReadAllBytes(this.filePath);
            byte[] heapArray = new byte[fileSize - startingResHeapOffset];
            Buffer.BlockCopy(fileArray, startingResHeapOffset, heapArray, 0, heapArray.Length);
            fileArray = null;

            foreach (KeyValuePair<AuroraResourceInfo, BiffVariableResourceEntry> pair in entries) {
                if (pendingEntries.ContainsKey(pair.Key)) {
                    //Add external file as new item
                    byteArray.AddRange(pendingEntries[pair.Key].toBytes());
                    //remove the key
                    //pendingEntries[pair.Key] = null;
                } else {
                    //slice the array out of the segment
                    int offset = (int)entries[pair.Key].fileOffset - startingResHeapOffset;
                    int resourceSize = (int)pair.Value.fileSize;
                    byte[] individualFileArray = new byte[pair.Value.fileSize];
                    Buffer.BlockCopy(heapArray, (int)offset, individualFileArray, 0, resourceSize);
                    byteArray.AddRange(individualFileArray);
                }
            }
            return byteArray.ToArray();
        }
    }
}
