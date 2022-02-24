using AuroraIO.Source.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AuroraIO.Archives {
    public class RIMArchive: AuroraResourceCollection {
        private string filePath;
        private string fileVersion;

        private Dictionary<AuroraResourceInfo, RIMResourceInfo> resourceMap = new Dictionary<AuroraResourceInfo, RIMResourceInfo>();
        private Dictionary<AuroraResourceInfo, AuroraResource> pendingResources = new Dictionary<AuroraResourceInfo, AuroraResource>();

        public RIMArchive() {
            this.fileType = AuroraResourceType.RIM;
            this.fileVersion = "V1.0";
        }

        public RIMArchive(string filePath) {
            byte[] fileArray = File.ReadAllBytes(filePath);

            this.filePath = filePath;

            //FileType
            this.fileType = Encoding.ASCII.GetString(fileArray, 0, 4).Replace(" ", "").ToLower().toAuroraResourceType();

            //FileVersion
            this.fileVersion = Encoding.ASCII.GetString(fileArray, 4, 4);

            //Unknown byte at 8

            //resource count
            int resourceCount = (int)BitConverter.ToUInt32(fileArray, 12);

            //resource offset
            int resourceOffset = (int)BitConverter.ToUInt32(fileArray, 16);

            //100 reserved bytes

            //Parse Key List
            int readingOffset = resourceOffset;
            for (int i = 0; i < resourceCount; i++) {
                String resref = Encoding.ASCII.GetString(fileArray, readingOffset, 16).Replace(" ", "").Replace("\0", "");
                //We don't care about Res ID
                AuroraResourceType resourceType = (AuroraResourceType)BitConverter.ToUInt16(fileArray, readingOffset + 16);

                AuroraResourceInfo resInfo = new AuroraResourceInfo(resref, resourceType);
                int fileOffset = (int)BitConverter.ToUInt32(fileArray, 24);
                int fileSize = (int)BitConverter.ToUInt32(fileArray, 28);

                resourceMap[resInfo] = new RIMResourceInfo(fileOffset, fileSize);
                readingOffset += 32;
            }

            return;
        }

        public void addResource(AuroraResource resource) {
            AuroraResourceInfo key = new AuroraResourceInfo(resource.fileName, resource.fileType);
            pendingResources[key] = resource;
            resourceMap[key] = new RIMResourceInfo(resource.toBytes().Length, 0);
        }

        public AuroraResource extractResource(string resourceID) {
            AuroraResourceInfo resInfo = resourceID.toAuroraResourceInfo();
            byte[] fileArray = File.ReadAllBytes(this.filePath);
            RIMResourceInfo erfResInfo = resourceMap[resInfo];
            byte[] individualFileArray = new byte[erfResInfo.fileSize];
            int offset = erfResInfo.fileOffset;
            Buffer.BlockCopy(fileArray, (int)offset, individualFileArray, 0, (int)erfResInfo.fileSize);
            return AuroraResourceLoader.loadFile(resInfo, individualFileArray);
        }

        public override byte[] toBytes() {
            ByteArray byteArray = new ByteArray();

            //BuildHeader
            byteArray.AddRange(Encoding.ASCII.GetBytes(this.fileType.stringValue().ToUpper().PadRight(4)));
            byteArray.AddRange(Encoding.ASCII.GetBytes(this.fileVersion.PadRight(4)));
            //Unknown byte
            byteArray.AddRange(BitConverter.GetBytes((uint)0));
            //Resource Count
            int resourceCount = resourceMap.Count;
            byteArray.AddRange(BitConverter.GetBytes((uint)resourceCount));
            //Resource Offset
            int resourceOffset = 120;
            byteArray.AddRange(BitConverter.GetBytes((uint)resourceOffset));
            //100 unreserved bytes

            //write 116 bytes of reserved space
            for (int i = 0; i < 100; i++) {
                byteArray.Add(0);
            }

            //Write Key List

            uint resID = 0;
            int startingResHeapOffset = (resourceCount * 8) + resourceOffset;
            int currentResHeapOffset = startingResHeapOffset;
            foreach (KeyValuePair<AuroraResourceInfo, RIMResourceInfo> pair in resourceMap) {
                byteArray.AddRange(Encoding.ASCII.GetBytes(pair.Key.resref.PadRight(16, '\0').Substring(0, 16)));
                byteArray.AddRange(BitConverter.GetBytes((UInt16)pair.Key.resourceType));
                byteArray.AddRange(BitConverter.GetBytes((UInt16)0));
                byteArray.AddRange(BitConverter.GetBytes(resID++));
                byteArray.AddRange(BitConverter.GetBytes((uint)currentResHeapOffset));
                byteArray.AddRange(BitConverter.GetBytes((uint)pair.Value.fileSize));
                currentResHeapOffset += pair.Value.fileSize;
            }

            byte[] heapArray;
            //Write Resource data
            if (this.filePath != null) {
                int fileSize = (int)(new FileInfo(this.filePath).Length);
                byte[] fileArray = File.ReadAllBytes(this.filePath);
                heapArray = new byte[fileSize - startingResHeapOffset];
                Buffer.BlockCopy(fileArray, startingResHeapOffset, heapArray, 0, heapArray.Length);
                fileArray = null;
            } else {
                heapArray = new byte[0];
            }

            foreach (KeyValuePair<AuroraResourceInfo, RIMResourceInfo> pair in resourceMap) {
                if (pendingResources.ContainsKey(pair.Key)) {
                    //Add external file as new item
                    byteArray.AddRange(pendingResources[pair.Key].toBytes());
                    //remove the key
                    //pendingResources[pair.Key] = null;
                } else {
                    //slice the array out of the segment
                    int offset = (int)resourceMap[pair.Key].fileOffset - startingResHeapOffset;
                    int resourceSize = (int)pair.Value.fileSize;
                    byte[] individualFileArray = new byte[pair.Value.fileSize];
                    Buffer.BlockCopy(heapArray, (int)offset, individualFileArray, 0, resourceSize);
                    byteArray.AddRange(individualFileArray);
                }
            }

            return byteArray.ToArray();
        }

        public override void extractAll(string filePath) {
            throw new NotImplementedException();
        }
    }
}
