using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Collections {
    public class ERFArchive: AuroraResourceCollection {
        
        private int buildDay;
        private int buildYear;
        private int descriptionStrRef;
        private string fileVersion;
        private string filePath;

        Dictionary<CExoLanguage, string> localizedStrings = new Dictionary<CExoLanguage, string>();
        Dictionary<AuroraResourceInfo, ERFResourceInfo> keyResourceMap = new Dictionary<AuroraResourceInfo, ERFResourceInfo>();
        Dictionary<AuroraResourceInfo, AuroraResource> pendingResources = new Dictionary<AuroraResourceInfo, AuroraResource>();

        public ERFArchive(AuroraResourceType resourceType) {
            this.fileType = resourceType;
            this.fileVersion = "V1.0";
        }

        public ERFArchive(string filePath) : this(File.ReadAllBytes(filePath) ) {
            this.filePath = filePath;
        }

        public ERFArchive(byte[] fileArray) {
            //FileType
            this.fileType = Encoding.ASCII.GetString(fileArray, 0, 4).Replace(" ", "").ToLower().toAuroraResourceType();

            //FileVersion
            this.fileVersion = Encoding.ASCII.GetString(fileArray, 4, 4);

            //language count
            int localizedStringCount = (int)BitConverter.ToUInt32(fileArray, 8);
            //Localized string size
            int localizedStringHeapSize = (int)BitConverter.ToUInt32(fileArray, 12);

            //Entry count
            int entryCount = (int)BitConverter.ToUInt32(fileArray, 16);

            //offset to localized string
            int localizedStringOffset = (int)BitConverter.ToUInt32(fileArray, 20);

            //offset to Key list
            int keylistOffset = (int)BitConverter.ToUInt32(fileArray, 24);

            //offset to resource list
            int resourceListOffset = (int)BitConverter.ToUInt32(fileArray, 28);

            //build year
            this.buildYear = (int)BitConverter.ToUInt32(fileArray, 32);

            //build day
            this.buildDay = (int)BitConverter.ToUInt32(fileArray, 36);

            //description str ref
            this.descriptionStrRef = (int)BitConverter.ToUInt32(fileArray, 40);

            //Parse localized strings
            int readingOffset = 160;
            for (int i = 0; i < localizedStringCount; i++) {
                CExoLanguage languageID = (CExoLanguage)(int)(BitConverter.ToUInt32(fileArray, readingOffset));
                int stringSize = (int)BitConverter.ToUInt32(fileArray, readingOffset + 4);
                String localizedString = Encoding.ASCII.GetString(fileArray, readingOffset + 8, stringSize);
                localizedStrings[languageID] = localizedString;
                readingOffset += 12;
            }

            //Parse Key List
            readingOffset = keylistOffset;
            int resourceReadingOffset = resourceListOffset;
            for (int i = 0; i < entryCount; i++) {;
                String resref = Encoding.ASCII.GetString(fileArray, readingOffset, 16).Replace(" ", "").Replace("\0", "").ToLower();
                //We don't care about Res ID
                AuroraResourceType resourceType = (AuroraResourceType)BitConverter.ToUInt16(fileArray, readingOffset + 20);
                //keyList.Add(new ERFKeyListElement
                AuroraResourceInfo key = new AuroraResourceInfo(resref, resourceType);


                int offsetToResourceData = (int)BitConverter.ToUInt32(fileArray, resourceReadingOffset);
                int resourceDataSize = (int)BitConverter.ToUInt32(fileArray, resourceReadingOffset + 4);

                ERFResourceInfo resourceInfo = new ERFResourceInfo(resourceDataSize, offsetToResourceData);
                keyResourceMap[key] = resourceInfo;
                readingOffset += 24;
                resourceReadingOffset += 8;
            }
        }

        public void addResource(AuroraResource resource) {
            AuroraResourceInfo key = new AuroraResourceInfo(resource.fileName, resource.fileType);
            pendingResources[key] = resource;
            keyResourceMap[key] = new ERFResourceInfo(resource.toBytes().Length, 0);
        }

        public AuroraResource extractResource(string resourceID) {
            string fileResourceName = Regex.Match(resourceID, "^[^.]*").Value.ToLower();
            string fileExtension = Regex.Match(resourceID, "[^.]*$").Value;
            AuroraResourceInfo resInfo = new AuroraResourceInfo(fileResourceName, fileExtension.toAuroraResourceType());
            byte[] fileArray = File.ReadAllBytes(this.filePath);
            ERFResourceInfo erfResInfo = keyResourceMap[resInfo];
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
            //Language count
            int languageCount = localizedStrings.Count;
            byteArray.AddRange(BitConverter.GetBytes((uint)languageCount));
            //localized String size
            //TODO: localized string support: Fill in with non-zero later
            byteArray.AddRange(BitConverter.GetBytes((uint)0));
            //Entry size
            int entryCount = keyResourceMap.Count;
            byteArray.AddRange(BitConverter.GetBytes((uint)entryCount));
            //Offset to localizedString
            int localizedStringOffset = 160;
            byteArray.AddRange(BitConverter.GetBytes((uint)localizedStringOffset));
            //Offset to keyList
            int keyListOffset = localizedStringOffset + languageCount * 12;
            //TODO: localized string support: When supporting localized strings, this should be 160 + localizedStringHeap size
            byteArray.AddRange(BitConverter.GetBytes((uint)160));
            //Offset to resourceList
            int resourceListOffset = entryCount * 24 + keyListOffset;
            byteArray.AddRange(BitConverter.GetBytes((uint)resourceListOffset));
            //Build year
            byteArray.AddRange(BitConverter.GetBytes((uint)0));
            //Build Day
            byteArray.AddRange(BitConverter.GetBytes((uint)0));
            //Description strref
            byteArray.AddRange(BitConverter.GetBytes((uint)descriptionStrRef));
            //write 116 bytes of reserved space
            for (int i = 0; i < 116; i++) {
                byteArray.Add(0);
            }

            //TODO: localized string support: Support localized string writing

            //Write Key List

            uint resID = 0;
            foreach (KeyValuePair<AuroraResourceInfo, ERFResourceInfo> pair in keyResourceMap) {
                byteArray.AddRange(Encoding.ASCII.GetBytes(pair.Key.resref.PadRight(16, '\0').Substring(0, 16)));
                byteArray.AddRange(BitConverter.GetBytes(resID++));
                byteArray.AddRange(BitConverter.GetBytes((UInt16)pair.Key.resourceType));
                byteArray.AddRange(BitConverter.GetBytes((UInt16)0));
            }

            //Write Resource List
            int startingResHeapOffset = (entryCount * 8) + resourceListOffset;
            int currentResHeapOffset = startingResHeapOffset;
            foreach (KeyValuePair<AuroraResourceInfo, ERFResourceInfo> pair in keyResourceMap) {
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

            foreach (KeyValuePair<AuroraResourceInfo, ERFResourceInfo> pair in keyResourceMap) {
                if (pendingResources.ContainsKey(pair.Key)) {
                    //Add external file as new item
                    byteArray.AddRange(pendingResources[pair.Key].toBytes());
                    //remove the key
                    //pendingResources[pair.Key] = null;
                } else {
                    //slice the array out of the segment
                    int offset = (int)keyResourceMap[pair.Key].fileOffset - startingResHeapOffset;
                    int resourceSize = (int)pair.Value.fileSize;
                    byte[] individualFileArray = new byte[pair.Value.fileSize];
                    Buffer.BlockCopy(heapArray, (int)offset, individualFileArray, 0, resourceSize);
                    byteArray.AddRange(individualFileArray);
                }
            }

            return byteArray.ToArray();
        }

        public override void extractAll(string filePath) {
            foreach (KeyValuePair<AuroraResourceInfo, ERFResourceInfo> pair in keyResourceMap) {
                AuroraResource resource = extractResource(pair.Key.ToString());
                File.WriteAllBytes(filePath + Path.DirectorySeparatorChar + pair.Key.resref, resource.toBytes());
            }
        }
    }
}
