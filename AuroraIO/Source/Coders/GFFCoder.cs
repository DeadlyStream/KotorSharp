using AuroraIO.Source.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Coders
{
    public class GFFCoder {

         struct GFFStructInfo {
            public readonly uint structType;
            public readonly uint dataOrDataOffset;
            public readonly uint fieldCount;

            public GFFStructInfo(uint structType, int dataOrDataOffset, int fieldCount) {
                this.structType = structType;         
                this.dataOrDataOffset = (uint)dataOrDataOffset;
                this.fieldCount = (uint)fieldCount;
            }
        }

        struct GFFFieldInfo {
            public readonly AuroraDataType fieldType;
            public readonly uint labelIndex;
            public readonly uint dataOrDataOffset;

            public GFFFieldInfo(AuroraDataType fieldType, uint labelIndex, uint dataOrDataOffset) {
                this.fieldType = fieldType;
                this.labelIndex = labelIndex;
                this.dataOrDataOffset = dataOrDataOffset;
            }
        }

        public AuroraDictionary decode(byte[] byteArray) {
            AuroraResourceType fileType = Encoding.ASCII.GetString(byteArray, 0, 4).Replace(" ", "").ToLower().toAuroraResourceType();
            var baseStruct = getStructAtOffset(byteArray, 0);
            return AuroraDictionary.make(fileType.stringValue(), dict => { 
                foreach(KeyValuePair<string, AuroraDataObject> pair in baseStruct) {
                    dict[pair.Key] = pair.Value;
                }
            });
        }

        public byte[] encode(AuroraDictionary dict) {

            //Processing prep work

            ByteArray complexFieldData = new ByteArray();
            List<GFFFieldInfo> allFields = new List<GFFFieldInfo>();
            List<GFFStructInfo> allStructs = new List<GFFStructInfo>();
            IndexMap<String> labelMap = new IndexMap<String>();
            List<int> listIndicesArray = new List<int>();
            List<int> fieldIndicesArray = new List<int>();

            processStruct(dict,
                allFields,
                allStructs,
                labelMap,
                complexFieldData,
                fieldIndicesArray,
                listIndicesArray);

            ByteArray newByteArray = new ByteArray();
            String fileVersion = "V3.2";

            //MARK: - Write Header

            //Write File Type
            string fileType = dict.type.ToUpper().PadRight(4, ' ');
            newByteArray.AddRange(Encoding.UTF8.GetBytes(fileType));

            //Write File Version
            newByteArray.AddRange(Encoding.ASCII.GetBytes(fileVersion));

            //Write Struct Starting Offset
            //Header size is always 56, and Struct Array Offset always comes directly after the header
            int structStartingOffset = 56;
            newByteArray.AddRange(BitConverter.GetBytes(structStartingOffset));

            //Write Struct Count
            //The total count of structs in the file - this includes structs inside of fields that are struct types and list types
            UInt32 structCount = (UInt32)allStructs.Count();
            newByteArray.AddRange(BitConverter.GetBytes(structCount));

            //MARK: - Write Field offset 
            // Structs have 12 bytes of information, thus the field array starting offset will be 12 * stuctCount
            UInt32 fieldOffset = 56 + structCount * 12;
            newByteArray.AddRange(BitConverter.GetBytes(fieldOffset));

            //MARK: - Write field count (actual count)
            UInt32 fieldCount = (UInt32)allFields.Count();
            newByteArray.AddRange(BitConverter.GetBytes(fieldCount));

            //MARK: - Write Label offset
            //Fields have 12 bytes of information, thus the label offset will be 12 * fieldCount
            UInt32 labelOffset = fieldOffset + fieldCount * 12;
            newByteArray.AddRange(BitConverter.GetBytes(labelOffset));

            //MARK: - Write Label count
            UInt32 labelCount = (UInt32)labelMap.Count();
            newByteArray.AddRange(BitConverter.GetBytes(labelCount));

            //MARK: - Field data offset
            //This is the starting offset that points the beginning of the complex data heap
            UInt32 fieldDataOffset = labelOffset + labelCount * 16;
            newByteArray.AddRange(BitConverter.GetBytes(fieldDataOffset));

            //MARK: - FieldData length (in total bytes)
            UInt32 fieldDataCount = (UInt32)complexFieldData.Count;
            newByteArray.AddRange(BitConverter.GetBytes(fieldDataCount));

            //MARK: - FieldIndices offset

            //A Field Index is a DWORD containing the index of the associated Field within the Field array.
            //The Field Indices Array is an array of such DWORDs.
            UInt32 fieldIndicesOffset = fieldDataOffset + fieldDataCount;
            newByteArray.AddRange(BitConverter.GetBytes(fieldIndicesOffset));

            //MARK: - Field Indices length (in bytes)
            UInt32 fieldIndicesCount = (UInt32)fieldIndicesArray.Count * 4;
            newByteArray.AddRange(BitConverter.GetBytes(fieldIndicesCount));

            //MARK: - List indices offset
            UInt32 listIndicesOffset = fieldIndicesOffset + fieldIndicesCount;
            newByteArray.AddRange(BitConverter.GetBytes(listIndicesOffset));

            //MARK: - List indices offset
            UInt32 listIndicesCount = (UInt32)listIndicesArray.Count * 4;
            newByteArray.AddRange(BitConverter.GetBytes(listIndicesCount));

            Debug.Assert(newByteArray.Count == 56);

            //Write Struct data
            HashSet<int> verifiedFields = new HashSet<int>();
            
            foreach (GFFStructInfo structInfo in allStructs) {

                //Convert structInfo to bytes
                newByteArray.AddRange(BitConverter.GetBytes((UInt32)structInfo.structType));
                newByteArray.AddRange(BitConverter.GetBytes((UInt32)structInfo.dataOrDataOffset));
                newByteArray.AddRange(BitConverter.GetBytes(structInfo.fieldCount));

                if (structInfo.fieldCount > 1) {
                    int startingIndex = (int)structInfo.dataOrDataOffset / 4;
                    for (int i = 0; i < structInfo.fieldCount; i++) {
                        verifiedFields.Add(fieldIndicesArray[startingIndex + i]);
                    }
                } else if (structInfo.fieldCount == 1) {
                    verifiedFields.Add((int)structInfo.dataOrDataOffset);
                }
            }

            Debug.Assert(fieldCount == verifiedFields.Count);

            //Write Field Data

            HashSet<string> verifiedLabels = new HashSet<string>();
            ByteArray verifiedComplexDataBytes = new ByteArray();
            HashSet<int> verifiedStructIndices = new HashSet<int>();

            foreach (GFFFieldInfo fieldInfo in allFields) {

                newByteArray.AddRange(BitConverter.GetBytes((uint)fieldInfo.fieldType));
                newByteArray.AddRange(BitConverter.GetBytes(fieldInfo.labelIndex));
                newByteArray.AddRange(BitConverter.GetBytes(fieldInfo.dataOrDataOffset));

                //Add to verified labels
                verifiedLabels.Add(labelMap[(int)fieldInfo.labelIndex]);
            }   

            Debug.Assert(labelMap.Count == verifiedLabels.Count);

            //Write Label Array
            foreach(String label in labelMap) {
                //Field label values must be padded to 16 length with 0's
                String modifiedValue = label.Substring(0, Math.Min(label.Length, 16));
                newByteArray.AddRange(ASCIIEncoding.ASCII.GetBytes(modifiedValue));

                for (int i = modifiedValue.Length; i < 16; i++) {
                    newByteArray.Add(0);
                }     
            }

            //Write Complex Field Data Block
            newByteArray.AddRange(complexFieldData.ToArray());

            //Write FieldIndices Array
            newByteArray.AddRange(fieldIndicesArray.SelectMany(index => BitConverter.GetBytes((uint)index).ToArray()));

            //Write ListIndices Array
            newByteArray.AddRange(listIndicesArray.SelectMany(index => BitConverter.GetBytes((uint)index).ToArray()));

            return newByteArray.ToArray();
        }

        AuroraStruct getStructAtOffset(byte[] fileArray, int offset) {
            UInt32 fieldIndicesOffset = BitConverter.ToUInt32(fileArray, 40);
            //Parse base struct
            int startingOffset = (int)BitConverter.ToUInt32(fileArray, 8) + offset;
            UInt32 structType = BitConverter.ToUInt32(fileArray, startingOffset);
            UInt32 dataOrDataOffset = BitConverter.ToUInt32(fileArray, startingOffset + 4);
            UInt32 structFieldCount = BitConverter.ToUInt32(fileArray, startingOffset + 8);
            if (structFieldCount == 1) {
                //Look into field Array
                KeyValuePair<String, AuroraDataObject> field = getFieldAtOffset(fileArray, (int)dataOrDataOffset * 12);
                return AuroraStruct.make(structType, dict => {
                    dict[field.Key] = field.Value;
                });
            } else {
                //Look into field indices array
                return AuroraStruct.make(structType, dict => {
                    UInt32 fieldIndexOffset = dataOrDataOffset;
                    for (int j = 0; j < structFieldCount; j++) {
                        UInt32 startingIndex = BitConverter.ToUInt32(fileArray, (int)fieldIndexOffset + (int)fieldIndicesOffset);
                        KeyValuePair<String, AuroraDataObject> fieldPair = getFieldAtOffset(fileArray, (int)startingIndex * 12);
                        dict[fieldPair.Key] = fieldPair.Value;
                        fieldIndexOffset += 4;
                    }
                });
            }
        }

        KeyValuePair<String, AuroraDataObject> getFieldAtOffset(byte[] fileArray, int offset) {
            UInt32 fieldOffset = BitConverter.ToUInt32(fileArray, 16);
            UInt32 labelOffset = BitConverter.ToUInt32(fileArray, 24);
            UInt32 labelCount = BitConverter.ToUInt32(fileArray, 28);
            UInt32 fieldDataOffset = BitConverter.ToUInt32(fileArray, 32);
            UInt32 fieldDataCount = BitConverter.ToUInt32(fileArray, 36);
            UInt32 fieldIndicesOffset = BitConverter.ToUInt32(fileArray, 40);
            UInt32 fieldIndicesCount = BitConverter.ToUInt32(fileArray, 44) / 4;
            UInt32 listIndicesOffset = BitConverter.ToUInt32(fileArray, 48);
            UInt32 listIndicesCount = BitConverter.ToUInt32(fileArray, 52);

            int startOffset = (int)fieldOffset + offset;
            AuroraDataType fieldType = (AuroraDataType)BitConverter.ToUInt32(fileArray, startOffset);
            UInt32 labelIndex = BitConverter.ToUInt32(fileArray, startOffset + 4);
            String label = Encoding.ASCII.GetString(fileArray, (int)labelOffset + (int)(labelIndex * 16), 16).Replace("\0", "");
            UInt32 dataOrDataOffset = BitConverter.ToUInt32(fileArray, startOffset + 8);

            //Complex only which is an offset into the field data array
            int complexOffset = (int)fieldDataOffset + (int)dataOrDataOffset;
            //Simple only which is the offset of the current field + 8
            int simpleOffset = startOffset + 8;

            AuroraDataObject dataObject = null;

            switch (fieldType) {
                //Simple data parses from an offset that lies right with the field data
                case AuroraDataType.Byte:
                    dataObject = fileArray[simpleOffset];
                    break;
                case AuroraDataType.Char:
                    dataObject = (char)fileArray[simpleOffset];
                    break;
                case AuroraDataType.Word:
                    dataObject = BitConverter.ToUInt16(fileArray, simpleOffset);
                    break;
                case AuroraDataType.Short:
                    dataObject = BitConverter.ToInt16(fileArray, simpleOffset);
                    break;
                case AuroraDataType.Dword:
                    dataObject = BitConverter.ToUInt32(fileArray, simpleOffset);
                    break;
                case AuroraDataType.Int:
                    dataObject = BitConverter.ToInt32(fileArray, simpleOffset);
                    break;
                case AuroraDataType.Float:
                    dataObject = BitConverter.ToSingle(fileArray, simpleOffset);
                    break;
                //Complex data parses data in the field data heap
                case AuroraDataType.Dword64:
                    dataObject = BitConverter.ToUInt64(fileArray, complexOffset);
                    break;
                case AuroraDataType.Int64:
                    dataObject = BitConverter.ToInt64(fileArray, complexOffset);
                    break;
                case AuroraDataType.Double:
                    dataObject = BitConverter.ToDouble(fileArray, complexOffset);
                    break;
                case AuroraDataType.CExoString:
                    UInt32 stringLength = BitConverter.ToUInt32(fileArray, complexOffset);
                    dataObject = Encoding.ASCII.GetString(fileArray, complexOffset + 4, (int)stringLength);
                    break;
                case AuroraDataType.CResref:
                    byte resrefLength = fileArray[complexOffset];
                    dataObject = Encoding.ASCII.GetString(fileArray, complexOffset + 1, resrefLength);
                    break;
                case AuroraDataType.CExoLocString:
                    UInt32 cexoLocStringSize = BitConverter.ToUInt32(fileArray, complexOffset);
                    UInt32 stringResRef = BitConverter.ToUInt32(fileArray, complexOffset + 4);
                    UInt32 totalSubstrings = BitConverter.ToUInt32(fileArray, complexOffset + 8);

                    dataObject = AuroraCExoLocString.make(stringResRef, dict =>{
                        int substringStartingOffset = complexOffset + 12;
                        for (int i = 0; i < (int)totalSubstrings; i++) {
                            CExoLanguage stringID = (CExoLanguage)BitConverter.ToUInt32(fileArray, substringStartingOffset);
                            UInt32 subStringLength = BitConverter.ToUInt32(fileArray, substringStartingOffset + 4);
                            String cexoSubstring = Encoding.ASCII.GetString(fileArray, substringStartingOffset + 8, (int)subStringLength);
                            dict[stringID] = cexoSubstring;
                            substringStartingOffset += (int)subStringLength + 8;
                        }
                    });
                    break;
                case AuroraDataType.Void:
                    UInt32 dataLength = BitConverter.ToUInt32(fileArray, complexOffset);
                    dataObject = new ArraySegment<byte>(fileArray, complexOffset + 4, (int)dataLength).Array;
                    break;
                case AuroraDataType.Quaternion:
                    float qW = BitConverter.ToSingle(fileArray, complexOffset);
                    float qX = BitConverter.ToSingle(fileArray, complexOffset + 4);
                    float qY = BitConverter.ToSingle(fileArray, complexOffset + 8);
                    float qZ = BitConverter.ToSingle(fileArray, complexOffset + 12);
                    dataObject = (qW, qX, qY, qZ);
                    break;
                case AuroraDataType.Vector:
                    float vX = BitConverter.ToSingle(fileArray, complexOffset);
                    float vY = BitConverter.ToSingle(fileArray, complexOffset + 4);
                    float vZ = BitConverter.ToSingle(fileArray, complexOffset + 8);
                    dataObject = (vX, vY, vZ);
                    break;
                case AuroraDataType.StrRef:
                    //Even in xoreos, this is hardcoded to 4, not sure why
                    int unknownHeader = BitConverter.ToInt32(fileArray, complexOffset);
                    dataObject = AuroraStrRef.make(BitConverter.ToUInt32(fileArray, complexOffset + 4));
                    break;
                //Structs are different and get an offset from struct array
                case AuroraDataType.Struct:
                    int structOffset = (int)dataOrDataOffset * 12;
                    dataObject = getStructAtOffset(fileArray, structOffset);
                    break;
                    
                // Unlike most of the complex Field data types, a List Field's data is located not in the Field Data Block, 
                // but in the Field Indices Array.
                // The starting address of a List is specified in its Field's DataOrDataOffset value as a byte offset into the
                // Field Indices Array, at which is located a List element.
                case AuroraDataType.List:
                    int offsetToList = (int)dataOrDataOffset + (int)listIndicesOffset;
                    int structArraySize = (int)BitConverter.ToUInt32(fileArray, offsetToList);
                    if (dataOrDataOffset == UInt32.MaxValue) {
                        structArraySize = 0;
                    }
                    AuroraStruct[] childStructs = new AuroraStruct[structArraySize];
                    for (int j = 0; j < structArraySize; j++) {
                        int childStructIndex = (int)BitConverter.ToUInt32(fileArray, offsetToList + (j + 1) * 4);
                        childStructs[j] = getStructAtOffset(fileArray, childStructIndex * 12);
                    }

                    dataObject = childStructs;
                    break;
            }
            return new KeyValuePair<string, AuroraDataObject>(label, dataObject);
        }

        AuroraStructType[] findStructs(AuroraStructType auroraStruct) {
            var list = auroraStruct.SelectMany(pair =>
            {
                var dataObject = pair.Value;
                switch (dataObject.dataType)
                {
                    case AuroraDataType.List:
                        var subList = dataObject as AuroraList;
                        return subList.ToList().SelectMany(item => findStructs(item)).ToArray();
                    case AuroraDataType.Struct:
                        return new AuroraStruct[] { dataObject as AuroraStruct };
                    default:
                        return new AuroraStruct[] { };
                }
            }).ToList();

            list.Add(auroraStruct);
            return list.ToArray();
        }

        void processStruct(AuroraStructType baseStruct,
            List<GFFFieldInfo> allFields,
            List<GFFStructInfo> allStructs,
            IndexMap<String> labelMap,
            ByteArray complexFieldData,
            List<int> fieldIndicesArray,
            List<int> listIndicesArray)
        {
            List<GFFStructInfo> localStructs = new List<GFFStructInfo>();
            List<GFFFieldInfo> localFields = new List<GFFFieldInfo>();
            List<int> localFieldIndicesArray = new List<int>();

            KeyValuePair<String, AuroraDataObject>[] orderedPairs = baseStruct.Select(pair => pair)
                .OrderBy(pair => pair.Key)
                .ToArray();

            int fieldCount = baseStruct.Count();

            foreach (KeyValuePair<string, AuroraDataObject> pair in orderedPairs) {
                var dataObject = pair.Value;
                var label = pair.Key;
                var fieldType = dataObject.dataType;

                labelMap.Add(label);

                uint dataOrDataOffset = uint.MaxValue;

                switch (fieldType) {
                    case AuroraDataType.Undefined:
                        break;
                    case AuroraDataType.Byte: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraByte);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Char: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraChar);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Word: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraWord);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Short: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraShort);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Dword: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraDWord);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Int: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraInt);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Float: {
                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraFloat);
                            dataOrDataOffset = BitConverter.ToUInt32(value, 0);
                        }
                        break;
                    case AuroraDataType.Dword64: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraDWord64);
                            complexFieldData.AddRange(value);
                        }
                        break;
                    case AuroraDataType.Int64: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraInt64);
                            complexFieldData.AddRange(value);
                        }
                        break;
                    case AuroraDataType.Double: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            byte[] value = AuroraBitConverter.GetBytes(dataObject as AuroraDouble);
                            complexFieldData.AddRange(value);
                        }
                        break;
                    case AuroraDataType.CExoString: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraCExoString));
                        }
                        break;
                    case AuroraDataType.CResref: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraResref));
                        }
                        break;
                    case AuroraDataType.CExoLocString: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraCExoLocString));
                        }
                        break;
                    case AuroraDataType.Void: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraVoid));
                        }
                        break;
                    case AuroraDataType.Struct: {
                            //Not sure if this should go before processing the struct or after
                            dataOrDataOffset = Convert.ToUInt32(allStructs.Count);

                            processStruct(dataObject as AuroraStruct, allFields, localStructs, labelMap, complexFieldData, localFieldIndicesArray, listIndicesArray);
                        }
                        break;
                    case AuroraDataType.List: {
                            var listDataObject = dataObject as AuroraList;

                            //Add size of struct array to account for these structs;
                            List<int> localListIndicesArray = new List<int>();

                            //Ofset into listIndicesArray
                            int offset = listIndicesArray.Count * 4;
                            dataOrDataOffset = Convert.ToUInt32(offset);
                            int size = listDataObject.Count;
                            localListIndicesArray.Add(size);

                            foreach (AuroraStruct arStruct in listDataObject) {
                                processStruct(arStruct, allFields, localStructs, labelMap, complexFieldData, fieldIndicesArray, listIndicesArray);
                                localListIndicesArray.Add(allStructs.Count);
                            }

                            //Add to the total list indices
                            listIndicesArray.AddRange(localListIndicesArray.ToArray());
                        }
                        break;
                    case AuroraDataType.Quaternion: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraQuaternion));
                        }
                        break;
                    case AuroraDataType.Vector: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraVector));
                        }
                        break;
                    case AuroraDataType.StrRef: {
                            dataOrDataOffset = Convert.ToUInt32(complexFieldData.Count);

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraStrRef));
                        }
                        break;
                    default:
                        break;
                }

                //The field's index only has to be added to the fieldIndicesArray if it's part of a struct that has > 1 field
                if (fieldCount > 1) {
                    localFieldIndicesArray.Add(localFields.Count);
                }

                localFields.Add(new GFFFieldInfo(fieldType, (uint)labelMap[label], dataOrDataOffset));
            }

            GFFStructInfo structInfo;
            int currentFieldIndex = allFields.Count;
            if (localFields.Count > 1) {
                //Offset into fieldIndicesArray
                int fieldIndicesArrayOffset = fieldIndicesArray.Count * 4;
                structInfo = new GFFStructInfo(baseStruct.structType, fieldIndicesArrayOffset, localFields.Count);
                int[] modifiedListIndices = localFieldIndicesArray.Select(index => index + currentFieldIndex).ToArray();
                fieldIndicesArray.AddRange(modifiedListIndices);
            } else if (localFields.Count == 1) {
                structInfo = new GFFStructInfo(baseStruct.structType, currentFieldIndex, 1);
            } else {
                structInfo = new GFFStructInfo(baseStruct.structType, -1, 0);
            }

            allStructs.Add(structInfo);
            allStructs.AddRange(localStructs);
            allFields.AddRange(localFields);
        }

        /*
        private static GFFStruct[] allStructs(GFFStruct baseStruct)
        {
            List<GFFStruct> structs = new List<GFFStruct>();
            structs.Add(baseStruct);
            structs.AddRange(childStructs(baseStruct));
            return structs.ToArray();
        }
        private static GFFStruct[] childStructs(GFFStruct structInfo)
        {
            List<GFFStruct> structs = new List<GFFStruct>();

            foreach (KeyValuePair<string, GFFFieldDataObject> pair in structInfo.fields)
            {
                switch (pair.Value.fieldType())
                {
                    case GFFFieldType.STRUCT:
                        {
                            var structObject = pair.Value as GFFStructDataObject;
                            structs.Add(structObject.structInfo);
                            break;
                        }
                    case GFFFieldType.LIST:
                        {
                            var listDataObject = pair.Value as GFFListDataObject;
                            structs.AddRange(listDataObject.structInfoArray);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            GFFStruct[] baseLevelStructs = structs.ToArray();
            foreach (GFFStruct subStruct in baseLevelStructs)
            {
                structs.AddRange(childStructs(subStruct));
            }

            return structs.ToArray();
        }

        private static Dictionary<GFFStruct, UInt32> structMap(GFFStruct startingStruct)
        {
            GFFStruct[] allStructs = childStructs(startingStruct);

            Dictionary<GFFStruct, UInt32> structMap = new Dictionary<GFFStruct, uint>();
            UInt32 structIndex = 0;
            foreach (GFFStruct structInfo in allStructs)
            {
                structMap[structInfo] = structIndex++;
            }
            return structMap;
        }
        */
    }
}
