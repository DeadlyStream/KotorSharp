using AuroraIO.Models.Base;
using AuroraIO.Source.Archives;
using AuroraIO.Source.Archives.ERFRIM;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders {
    public class ERFRIMCoder {
        public enum Format {
            Auto,
            ERF,
            RIM
        }

        public AuroraArchive decode(Data data) {
            AuroraArchive.Format fileType = Encoding.ASCII.GetString(data, 0, 4).Trim();
            if (fileType == AuroraArchive.Format.RIM) {
                return decodeRIM(data);
            } else {
                return decodeERF(data);
            }
        }

        private AuroraArchive decodeRIM(Data data) {

            AuroraArchive.Format fileType = Encoding.ASCII.GetString(data, 0, 4).Trim();

            //file version at 8

            //resource count
            int resourceCount = (int)BitConverter.ToUInt32(data, 12);

            //resource offset
            int resourceOffset = (int)BitConverter.ToUInt32(data, 16);

            //100 reserved bytes

            //Parse Key List
            Dictionary<AuroraResourceName, byte[]> fileMap = new Dictionary<AuroraResourceName, byte[]>();

            for (int i = 0; i < resourceCount; i++) {
                int offset = resourceOffset + 32 * i;
                CResRef resref = Encoding.ASCII.GetString(data, offset, 16);
                AuroraResourceID resourceType = (AuroraResourceID)BitConverter.ToUInt16(data, offset + 16);
                
                //We don't care about Res ID
                AuroraResourceName name = new AuroraResourceName(resref, resourceType);
                int fileOffset = (int)BitConverter.ToUInt32(data, offset + 24);
                int fileSize = (int)BitConverter.ToUInt32(data, offset + 28);

                byte[] fileData = new byte[fileSize];

                Buffer.BlockCopy(data, fileOffset, fileData, 0, fileSize);
                fileMap[name] = fileData;
            }

            return new AuroraArchive(AuroraArchive.Format.RIM, fileMap);
        }

        private AuroraArchive decodeERF(Data data) { 
            //FileType
            AuroraArchive.Format fileType = Encoding.ASCII.GetString(data, 0, 4).Trim();

            ////FileVersion
            //string fileVersion = Encoding.ASCII.GetString(data, 4, 4);

            //language count
            int localizedStringCount = (int)BitConverter.ToUInt32(data, 8);
            //Localized string size
            int localizedStringHeapSize = (int)BitConverter.ToUInt32(data, 12);

            //Entry count
            int entryCount = (int)BitConverter.ToUInt32(data, 16);

            //offset to localized string
            int localizedStringOffset = (int)BitConverter.ToUInt32(data, 20);

            //offset to Key list
            int keylistOffset = (int)BitConverter.ToUInt32(data, 24);

            //offset to resource list
            int resourceListOffset = (int)BitConverter.ToUInt32(data, 28);

            ////build year
            //int buildYear = (int)BitConverter.ToUInt32(data, 32);

            ////build day
            //int buildDay = (int)BitConverter.ToUInt32(data, 36);

            //description str ref
            uint descriptionStrRef = BitConverter.ToUInt32(data, 40);

            CExoLocString localizedString = new CExoLocString();

            //Parse localized strings
            int stringOffset = localizedStringOffset;
            for (int i = 0; i < localizedStringCount; i++) {
                CExoLanguage languageID = (CExoLanguage)(int)(BitConverter.ToUInt32(data, stringOffset));
                int stringSize = (int)BitConverter.ToUInt32(data, stringOffset + 4);
                String stringValue = Encoding.ASCII.GetString(data, stringOffset + 8, stringSize);
                localizedString[languageID] = stringValue;

                stringOffset += 8 + stringSize;
            }

            //Parse Key List

            Dictionary<AuroraResourceName, byte[]> fileMap = new Dictionary<AuroraResourceName,byte[]>();
            for (int i = 0; i < entryCount; i++) {
                int offset = keylistOffset + 24 * i;
                CResRef resref = Encoding.ASCII.GetString(data, offset, 16).ToLower();
                //We don't care about Res ID

                int resourceID = (int)BitConverter.ToUInt32(data, offset + 16);

                AuroraResourceID id = (AuroraResourceID)BitConverter.ToUInt16(data, offset + 20);
                //keyList.Add(new ERFKeyListElement
                AuroraResourceName name = new AuroraResourceName(resref, id);

                int resourceEntryOffset = resourceListOffset + i * 8;
                int offsetToResourceData = (int)BitConverter.ToUInt32(data, resourceEntryOffset);
                int resourceDataSize = (int)BitConverter.ToUInt32(data, resourceEntryOffset + 4);

                byte[] fileData = new byte[resourceDataSize];
                Buffer.BlockCopy(data, offsetToResourceData, fileData, 0, resourceDataSize);
                fileMap[name] = fileData;
            }

            return new AuroraArchive(fileType, fileMap, descriptionStrRef, localizedString);
        }

        public Data encode(AuroraArchive archive, Format format = Format.Auto) {
            if (archive.format == AuroraArchive.Format.RIM) {
                return encodeRIM(archive);
            } else {
                return encodeERF(archive);
            }
        }

        private byte[] encodeRIM(AuroraArchive archive) {
            Data data = new Data();

            data.AddRange(Encoding.ASCII.GetBytes(archive.format.ToString().PadRight(4)));

            data.AddRange(Encoding.ASCII.GetBytes("V1.0"));

            data.AddRange(BitConverter.GetBytes((uint)0));

            //resource count
            int resourceCount = archive.Count();
            data.AddRange(BitConverter.GetBytes((uint)resourceCount));

            //resource offset
            int resourceOffset = 120;
            data.AddRange(BitConverter.GetBytes((uint)resourceOffset));

            //100 reserved bytes
            data.AddRange(Enumerable.Repeat((byte)0, 100));

            Data fileHeap = new Data();
            int index = 0;
            foreach (AuroraFile file in archive) {
                data.AddRange(AuroraBitConverter.GetBytes(file.name.resref));
                data.AddRange(BitConverter.GetBytes((uint)file.name.resourceType.id));
                data.AddRange(BitConverter.GetBytes((uint)index++));
                data.AddRange(BitConverter.GetBytes((uint)(fileHeap.Count + resourceOffset + 32 * resourceCount)));
                fileHeap.AddRange(file.data);
                data.AddRange(BitConverter.GetBytes((uint)file.data.Length));
            }

            data.AddRange(fileHeap);

            return data;
        }

        private byte[] encodeERF(AuroraArchive archive) { 

            Data data = new Data();

            //BuildHeader
            string fileType = archive.format;
            data.AddRange(Encoding.ASCII.GetBytes(fileType.PadRight(4)));
            data.AddRange(Encoding.ASCII.GetBytes("v1.0".PadRight(4)));
            //Language count

            int languageCount = archive.localizedString.languageCount;
            data.AddRange(BitConverter.GetBytes((uint)languageCount));

            Data stringData = new Data();
            foreach(KeyValuePair<CExoLanguage, String> pair in archive.localizedString) {
                stringData.AddRange(BitConverter.GetBytes((uint)pair.Key));
                stringData.AddRange(BitConverter.GetBytes((uint)pair.Value.Length));
                stringData.AddRange(Encoding.ASCII.GetBytes(pair.Value));
            }
            //localized String size
            data.AddRange(BitConverter.GetBytes((uint)stringData.Count));

            //Entry size
            int entryCount = archive.fileCount;
            data.AddRange(BitConverter.GetBytes((uint)entryCount));
            //Offset to localizedString
            int localizedStringOffset = 160;
            data.AddRange(BitConverter.GetBytes((uint)localizedStringOffset));

            //Offset to keyList
            int keyListOffset = localizedStringOffset + stringData.Count;

            //TODO: localized string support: When supporting localized strings, this should be 160 + localizedStringHeap size
            data.AddRange(BitConverter.GetBytes((uint)keyListOffset));
            //Offset to resourceList
            int resourceListOffset = entryCount * 24 + keyListOffset;
            data.AddRange(BitConverter.GetBytes((uint)resourceListOffset));

            var date = DateTime.Today;
            //Build year
            data.AddRange(BitConverter.GetBytes((uint)date.Year - 1900));
            //Build Day
            data.AddRange(BitConverter.GetBytes((uint)date.DayOfYear));
            //Description strref
            uint descriptionStrRef = archive.descriptionStrRef;
            data.AddRange(BitConverter.GetBytes((uint)descriptionStrRef));
            //write 116 bytes of reserved space
            for (int i = 0; i < 116; i++) {
                data.Add(0);
            }

            //localized string support: Support localized string writing

            data.AddRange(stringData);

            //Write Key List

            uint resID = 0;
            foreach (AuroraFile file in archive) {
                data.AddRange(AuroraBitConverter.GetBytes(file.name.resref));
                data.AddRange(BitConverter.GetBytes(resID++));
                data.AddRange(BitConverter.GetBytes((UInt16)file.name.resourceType.id));
                data.AddRange(BitConverter.GetBytes((UInt16)0));
            }

            //Write Resource List
            Data resourceHeap = new Data();
            int startingResHeapOffset = (entryCount * 8) + resourceListOffset;
            foreach (AuroraFile file in archive) {
                int offset = resourceHeap.Count + startingResHeapOffset;
                data.AddRange(BitConverter.GetBytes((uint)offset));
                data.AddRange(BitConverter.GetBytes((uint)file.data.Length));
                resourceHeap.AddRange(file.data);
            }

            data.AddRange(resourceHeap);

            return data;
        }
    }
}
