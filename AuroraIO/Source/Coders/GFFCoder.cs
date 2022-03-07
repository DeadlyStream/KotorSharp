using AuroraIO.Models.Base;
using AuroraIO.Source.Models.Base;
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
            public string label;
            public readonly uint dataOrDataOffset;

            public GFFFieldInfo(AuroraDataType fieldType, string label, uint dataOrDataOffset) {
                this.fieldType = fieldType;
                this.label = label;
                this.dataOrDataOffset = dataOrDataOffset;
            }
        }

        class GFFBuildInfo {
            public GFFStructInfo[] structInfoArray;
            public GFFFieldInfo[] fieldInfoArray;
            public CResRef[] labels;
            public byte[] complexFieldData;
            public byte[] fieldIndicesArray;
            public byte[] listIndicesArray;

            public GFFBuildInfo(
                GFFStructInfo[] structInfoArray,
                GFFFieldInfo[] fieldInfoArray,
                CResRef[] labels,
                byte[] complexFieldData,
                byte[] fieldIndicesArray,
                byte[] listIndicesArray
            ) {
                this.structInfoArray = structInfoArray;
                this.fieldInfoArray = fieldInfoArray;
                this.labels = labels;
                this.complexFieldData = complexFieldData;
                this.fieldIndicesArray = fieldIndicesArray;
                this.listIndicesArray = listIndicesArray;
            }
        }

        public AuroraDictionary decode(byte[] byteArray) {
            AuroraResourceType fileType = Encoding.ASCII.GetString(byteArray, 0, 4).Replace(" ", "").ToLower();
            var baseStruct = getStructAtOffset(byteArray, 0);
            return AuroraDictionary.make(fileType.stringValue, dict => { 
                foreach(KeyValuePair<CResRef, AuroraDataObject> pair in baseStruct) {
                    dict[pair.Key] = pair.Value;
                }
            });
        }

        public byte[] encode(AuroraDictionary dict) {

            //Processing prep work

            var info = processDictionary(dict);

            Data data = new Data();
            String fileVersion = "V3.2";

            //MARK: - Write Header

            //Write File Type
            string fileType = dict.type.ToUpper().PadRight(4, ' ');
            data.AddRange(Encoding.UTF8.GetBytes(fileType));

            //Write File Version
            data.AddRange(Encoding.ASCII.GetBytes(fileVersion));

            //Write Struct Starting Offset
            //Header size is always 56, and Struct Array Offset always comes directly after the header
            int structStartingOffset = 56;
            data.AddRange(BitConverter.GetBytes(structStartingOffset));

            //Write Struct Count
            //The total count of structs in the file - this includes structs inside of fields that are struct types and list types
            UInt32 structCount = (UInt32)info.structInfoArray.Count();
            data.AddRange(BitConverter.GetBytes(structCount));

            //MARK: - Write Field offset 
            // Structs have 12 bytes of information, thus the field array starting offset will be 12 * stuctCount
            UInt32 fieldOffset = 56 + structCount * 12;
            data.AddRange(BitConverter.GetBytes(fieldOffset));

            //MARK: - Write field count (actual count)
            UInt32 fieldCount = (UInt32)info.fieldInfoArray.Count();
            data.AddRange(BitConverter.GetBytes(fieldCount));

            //MARK: - Write Label offset
            //Fields have 12 bytes of information, thus the label offset will be 12 * fieldCount
            UInt32 labelOffset = fieldOffset + fieldCount * 12;
            data.AddRange(BitConverter.GetBytes(labelOffset));

            //MARK: - Write Label count
            UInt32 labelCount = (UInt32)info.labels.Count();
            data.AddRange(BitConverter.GetBytes(labelCount));

            //MARK: - Field data offset
            //This is the starting offset that points the beginning of the complex data heap
            UInt32 fieldDataOffset = labelOffset + labelCount * 16;
            data.AddRange(BitConverter.GetBytes(fieldDataOffset));

            //MARK: - FieldData length (in total bytes)
            UInt32 fieldDataCount = (UInt32)info.complexFieldData.Length;
            data.AddRange(BitConverter.GetBytes(fieldDataCount));

            //MARK: - FieldIndices offset

            //A Field Index is a DWORD containing the index of the associated Field within the Field array.
            //The Field Indices Array is an array of such DWORDs.
            UInt32 fieldIndicesOffset = fieldDataOffset + fieldDataCount;
            data.AddRange(BitConverter.GetBytes(fieldIndicesOffset));

            //MARK: - Field Indices length (in bytes)
            UInt32 fieldIndicesCount = (UInt32)info.fieldIndicesArray.Length;
            data.AddRange(BitConverter.GetBytes(fieldIndicesCount));

            //MARK: - List indices offset
            UInt32 listIndicesOffset = fieldIndicesOffset + fieldIndicesCount;
            data.AddRange(BitConverter.GetBytes(listIndicesOffset));

            //MARK: - List indices offset
            UInt32 listIndicesCount = (UInt32)info.listIndicesArray.Length;
            data.AddRange(BitConverter.GetBytes(listIndicesCount));

            //Write Struct data
            foreach (GFFStructInfo structInfo in info.structInfoArray) {

                //Convert structInfo to bytes
                data.AddRange(BitConverter.GetBytes((UInt32)structInfo.structType));
                data.AddRange(BitConverter.GetBytes((UInt32)structInfo.dataOrDataOffset));
                data.AddRange(BitConverter.GetBytes(structInfo.fieldCount));
            }

            //Write Field Data

            IndexMap<CResRef> labelMap = info.labels.ToIndexMap();

            foreach (GFFFieldInfo fieldInfo in info.fieldInfoArray) {
                data.AddRange(BitConverter.GetBytes((uint)fieldInfo.fieldType));
                data.AddRange(BitConverter.GetBytes(labelMap[fieldInfo.label]));
                data.AddRange(BitConverter.GetBytes(fieldInfo.dataOrDataOffset));
            }   

            //Write Label Array
            foreach(CResRef label in info.labels) {
                data.AddRange(AuroraBitConverter.GetBytes(label));
            }

            //Write Complex Field Data Block
            data.AddRange(info.complexFieldData.ToArray());

            //Write FieldIndices Array
            data.AddRange(info.fieldIndicesArray.ToArray());

            //Write ListIndices Array
            data.AddRange(info.listIndicesArray.ToArray());

            return data;
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
                KeyValuePair<CResRef, AuroraDataObject> field = getFieldAtOffset(fileArray, (int)dataOrDataOffset * 12);
                return AuroraStruct.make(structType, dict => {
                    dict[field.Key] = field.Value;
                });
            } else {
                //Look into field indices array
                return AuroraStruct.make(structType, dict => {
                    UInt32 fieldIndexOffset = dataOrDataOffset;
                    for (int j = 0; j < structFieldCount; j++) {
                        UInt32 startingIndex = BitConverter.ToUInt32(fileArray, (int)fieldIndexOffset + (int)fieldIndicesOffset);
                        KeyValuePair<CResRef, AuroraDataObject> fieldPair = getFieldAtOffset(fileArray, (int)startingIndex * 12);
                        dict[fieldPair.Key] = fieldPair.Value;
                        fieldIndexOffset += 4;
                    }
                });
            }
        }

        KeyValuePair<CResRef, AuroraDataObject> getFieldAtOffset(byte[] fileArray, int offset) {
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
            CResRef label = Encoding.ASCII.GetString(fileArray, (int)labelOffset + (int)(labelIndex * 16), 16);
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
                    dataObject = AuroraResref.make(Encoding.ASCII.GetString(fileArray, complexOffset + 1, resrefLength));
                    break;
                case AuroraDataType.CExoLocString:
                    UInt32 cexoLocStringSize = BitConverter.ToUInt32(fileArray, complexOffset);
                    UInt32 stringResRef = BitConverter.ToUInt32(fileArray, complexOffset + 4);
                    UInt32 totalSubstrings = BitConverter.ToUInt32(fileArray, complexOffset + 8);

                    dataObject = AuroraLocalizedString.make(stringResRef, dict =>{
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
                    byte[] byteArray = new byte[dataLength];
                    Array.ConstrainedCopy(fileArray, complexOffset + 4, byteArray, 0, (int)dataLength);
                    dataObject = byteArray;
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
                   // Unlike most of the complex Field data types, a Struct Field's data is located not in the Field Data Block,
                   // but in the Struct Array.
                   // Normally, a Field's DataOrDataOffset value would be a byte offset into the Field Data Block, but for a
                   // Struct, it is an index into the Struct Array
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
            return new KeyValuePair<CResRef, AuroraDataObject>(label, dataObject);
        }

        private GFFBuildInfo processDictionary(AuroraDictionary dictionary) {
           
            List<GFFFieldInfo> fieldArray = new List<GFFFieldInfo>();
            List<GFFStructInfo> structArray = new List<GFFStructInfo>();
            HashSet<CResRef> labels = new HashSet<CResRef>();
            Data complexFieldData = new Data();
            Data fieldIndicesArray = new Data();
            Data listIndicesArray = new Data();

            processStruct(dictionary,
                structArray,
                fieldArray,
                labels, 
                complexFieldData,
                fieldIndicesArray, 
                listIndicesArray);

            return new GFFBuildInfo(structArray.ToArray(),
                fieldArray.ToArray(),
                labels.ToArray(),
                complexFieldData.ToArray(),
                fieldIndicesArray.ToArray(),
                listIndicesArray.ToArray()
            );
        }

        private void processStruct(
            AuroraStructType baseStruct,
            List<GFFStructInfo> structArray,
            List<GFFFieldInfo> fieldArray,
            HashSet<CResRef> labels,
            Data complexFieldData,
            Data fieldIndicesArray,
            Data listIndicesArray
         ) {

            int fieldIndicesArrayStartingOffset = fieldIndicesArray.Count;

            GFFStructInfo structInfo;
            int localFieldCount = baseStruct.Count();
            if (localFieldCount > 1) {
                structInfo = new GFFStructInfo(baseStruct.structType, fieldIndicesArrayStartingOffset, localFieldCount);
            } else if (localFieldCount == 1) {
                structInfo = new GFFStructInfo(baseStruct.structType, fieldArray.Count, localFieldCount);
            } else {
                structInfo = new GFFStructInfo(baseStruct.structType, -1, 0);
            }

            structArray.Add(structInfo);

            //Reserve the bytes in the fieldIndicesArray, then backfill with localFieldIndicesArray
            fieldIndicesArray.AddRange(Enumerable.Repeat((byte)0, localFieldCount * 4));
            
            List<int> localFieldIndices = new List<int>();

            foreach (KeyValuePair<CResRef, AuroraDataObject> pair in baseStruct.OrderBy(pair => pair.Key)) {
                //The field's index only has to be added to the fieldIndicesArray if it's part of a struct that has > 1 field

                labels.Add(pair.Key);

                var dataObject = pair.Value;

                if (localFieldCount > 1) {
                    localFieldIndices.Add(fieldArray.Count);
                }

                switch (dataObject.dataType) {
                    case AuroraDataType.Undefined:
                        break;
                    case AuroraDataType.Byte: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraByte), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Char: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraChar), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Word: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraWord), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Short: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraShort), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Dword: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraDWord), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Int: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraInt), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Float: {
                            fieldArray.Add(new GFFFieldInfo(
                                    dataObject.dataType,
                                    pair.Key,
                                    BitConverter.ToUInt32(AuroraBitConverter.GetBytes(dataObject as AuroraFloat), 0)
                                ));
                        }
                        break;
                    case AuroraDataType.Dword64: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraDWord64));
                        }
                        break;
                    case AuroraDataType.Int64: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraInt64));
                        }
                        break;
                    case AuroraDataType.Double: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraDouble));
                        }
                        break;
                    case AuroraDataType.CExoString: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraString));
                        }
                        break;
                    case AuroraDataType.CResref: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraResref));
                        }
                        break;
                    case AuroraDataType.CExoLocString: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraLocalizedString));
                        }
                        break;
                    case AuroraDataType.Void: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraVoid));
                        }
                        break;
                    case AuroraDataType.Struct: {
                            // Unlike most of the complex Field data types, a Struct Field's data is located not in the Field Data Block,
                            // but in the Struct Array.
                            // Normally, a Field's DataOrDataOffset value would be a byte offset into the Field Data Block, but for a
                            // Struct, it is an index into the Struct Array

                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(structArray.Count)
                            ));

                            //process this struct here
                            processStruct(dataObject as AuroraStruct,
                                structArray,
                                fieldArray,
                                labels,
                                complexFieldData,
                                fieldIndicesArray,
                                listIndicesArray
                            );
                        }
                        break;
                    case AuroraDataType.List: {
                            // Unlike most of the complex Field data types, a List Field's data is located not in the Field Data Block,
                            // but in the Field Indices Array.
                            // The starting address of a List is specified in its Field's DataOrDataOffset value as a byte offset into the
                            // Field Indices Array, at which is located a List element. Section 3.8 describes the structure a List element

                            var listDataObject = dataObject as AuroraList;

                            int listIndicesArrayStartingOffset = listIndicesArray.Count();
                            int localStructCount = listDataObject.Count();

                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(listIndicesArray.Count())
                            ));

                            //Reserve the bytes in the listIndicesArray, then backfill with localListIndicesArray
                            listIndicesArray.AddRange(Enumerable.Repeat((byte)0, (localStructCount + 1) * 4));

                            List<int> localListIndices = new List<int>();

                            foreach (AuroraStruct arStruct in listDataObject) {
                                localListIndices.Add(structArray.Count());
                                processStruct(arStruct,
                                    structArray,
                                    fieldArray,
                                    labels,
                                    complexFieldData,
                                    fieldIndicesArray,
                                    listIndicesArray);
                            }

                            listIndicesArray.copyBytesToOffset(BitConverter.GetBytes((uint)localStructCount), listIndicesArrayStartingOffset);
                            listIndicesArray.copyBytesToOffset(localListIndices.SelectMany(b => BitConverter.GetBytes((uint)b)).ToArray(),
                                listIndicesArrayStartingOffset + 4);
                        }
                        break;
                    case AuroraDataType.Quaternion: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraQuaternion));
                        }
                        break;
                    case AuroraDataType.Vector: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraVector));
                        }
                        break;
                    case AuroraDataType.StrRef: {
                            fieldArray.Add(new GFFFieldInfo(
                                dataObject.dataType,
                                pair.Key,
                                Convert.ToUInt32(complexFieldData.Count)
                            ));

                            complexFieldData.AddRange(AuroraBitConverter.GetBytes(dataObject as AuroraStrRef));
                        }
                        break;
                    default:
                        break;
                }
            }

            fieldIndicesArray.copyBytesToOffset(localFieldIndices.SelectMany(b => BitConverter.GetBytes(b)).ToArray(), fieldIndicesArrayStartingOffset);
        }
    }
}
