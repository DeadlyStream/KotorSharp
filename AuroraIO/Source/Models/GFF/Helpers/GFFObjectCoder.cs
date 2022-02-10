using AuroraIO.Models;
using AuroraIO.Source.Models.Dictionary;
using AuroraIO.Source.Models.GFF.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AuroraIO.Source.Coders {
    /*
    class GFFObjectCoder: AuroraCoder<GFFObject> {

        public override GFFObject decode(byte[] byteArray) {
            AuroraResourceType fileType = Encoding.ASCII.GetString(byteArray, 0, 4).Replace(" ", "").ToLower().toAuroraResourceType();
            String fileVersion = Encoding.ASCII.GetString(byteArray, 4, 4);
            GFFStruct structInfo = getStructAtOffset(byteArray, 0);
            return new GFFObject(fileType, structInfo);
        }

        private GFFStruct getStructAtOffset(byte[] fileArray, int offset) {
            UInt32 fieldIndicesOffset = BitConverter.ToUInt32(fileArray, 40);
            //Parse base struct
            int startingOffset = (int)BitConverter.ToUInt32(fileArray, 8) + offset;
            UInt32 structType = BitConverter.ToUInt32(fileArray, startingOffset);
            UInt32 dataOrDataOffset = BitConverter.ToUInt32(fileArray, startingOffset + 4);
            UInt32 structFieldCount = BitConverter.ToUInt32(fileArray, startingOffset + 8);
            if (structFieldCount == 1) {
                //Look into field Array
                KeyValuePair<String, GFFFieldDataObject> field = getFieldAtOffset(fileArray, (int)dataOrDataOffset * 12);
                return new GFFStruct(structType, field);
            } else {
                //Look into field indices array
                UInt32 fieldIndexOffset = dataOrDataOffset;
                Dictionary<String, GFFFieldDataObject> fields = new Dictionary<string, GFFFieldDataObject>();
                for (int j = 0; j < structFieldCount; j++) {
                    UInt32 startingIndex = BitConverter.ToUInt32(fileArray, (int)fieldIndexOffset + (int)fieldIndicesOffset);
                    KeyValuePair <String, GFFFieldDataObject> fieldPair = getFieldAtOffset(fileArray, (int)startingIndex * 12);
                    fields[fieldPair.Key] = fieldPair.Value;
                    fieldIndexOffset += 4;
                }
                return new GFFStruct(structType, fields);
            }
        }

        private KeyValuePair<String, GFFFieldDataObject> getFieldAtOffset(byte[] fileArray, int offset) {
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
            GFFFieldType fieldType = (GFFFieldType)BitConverter.ToUInt32(fileArray, startOffset);
            UInt32 labelIndex = BitConverter.ToUInt32(fileArray, startOffset + 4);
            String label = Encoding.ASCII.GetString(fileArray, (int)labelOffset + (int)(labelIndex * 16), 16).Replace("\0", "");
            UInt32 dataOrDataOffset = BitConverter.ToUInt32(fileArray, startOffset + 8);

            GFFFieldDataObject data = null;

            if (fieldType.isComplex()) {
                if (fieldType == GFFFieldType.STRUCT) {
                    int structOffset = (int)dataOrDataOffset * 12;
                    GFFStruct structInfo = getStructAtOffset(fileArray, structOffset);
                    data = new GFFStructDataObject(structInfo);
                } else if (fieldType == GFFFieldType.LIST) {
                    int offsetToList = (int)dataOrDataOffset + (int)listIndicesOffset;
                    int structArraySize = (int)BitConverter.ToUInt32(fileArray, offsetToList);
                    if (dataOrDataOffset == UInt32.MaxValue) {
                        structArraySize = 0;
                    }
                    GFFStruct[] childStructs = new GFFStruct[structArraySize];
                    for (int j = 0; j < structArraySize; j++) {
                        if (dataOrDataOffset != UInt32.MaxValue) {
                            int childStructIndex = (int)BitConverter.ToUInt32(fileArray, offsetToList + (j + 1) * 4);
                            var structInfo = getStructAtOffset(fileArray, childStructIndex * 12);
                            childStructs[j] = structInfo;
                        }
                    }

                    data = new GFFListDataObject(childStructs);
                } else {
                    //Go into field data
                    int offsetIntoFieldData = (int)fieldDataOffset + (int)dataOrDataOffset;
                    data = fieldType.parseData(fileArray, offsetIntoFieldData);
                }
            } else {
                data = fieldType.parseData(fileArray, startOffset + 8);
                //Parse the data as is
            }
            return new KeyValuePair<string, GFFFieldDataObject>(label, data);
        }

        public override byte[] encode(GFFObject obj) {

            //Processing prep work

            GFFStruct baseStruct = new GFFStruct(uint.MaxValue, obj.fields);

            ByteArray complexFieldData = new ByteArray();
            List<GFFFieldInfo> allFields = new List<GFFFieldInfo>();
            List<GFFStructInfo> allStructs = new List<GFFStructInfo>();
            IndexMap<String> labelMap = new IndexMap<String>();
            List<int> listIndicesArray = new List<int>();
            List<int> fieldIndicesArray = new List<int>();

            GFFObjectCoder.processStruct(baseStruct,
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
            string fileType = obj.fileType.stringValue().ToUpper().PadRight(4, ' ');
            newByteArray.AddRange(Encoding.ASCII.GetBytes(fileType));

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

            //Write Struct data
            HashSet<int> verifiedFields = new HashSet<int>();
            foreach (GFFStructInfo structInfo in allStructs) {
                newByteArray.AddRange(structInfo.toBytes());

                if (structInfo.fieldCount > 1) {
                    int startingIndex = (int)structInfo.dataOrDataOffset / 4;
                    for (int i = 0; i < structInfo.fieldCount; i++) {
                        verifiedFields.Add(fieldIndicesArray[startingIndex + i]);
                    }
                } else if (structInfo.fieldCount == 1) {
                    verifiedFields.Add((int)structInfo.dataOrDataOffset);
                }
            }

            Debug.Assert(allFields.Count == verifiedFields.Count);

            //Write Field Data

            HashSet<string> verifiedLabels = new HashSet<string>();
            ByteArray verifiedComplexDataBytes = new ByteArray();
            HashSet<int> verifiedStructIndices = new HashSet<int>();

            int complexFieldCount = 0;
            int simpleFieldCount = 0;

            foreach (GFFFieldInfo field in allFields) {
                newByteArray.AddRange(field.toBytes());

                //Add to verified labels
                verifiedLabels.Add(labelMap[(int)field.labelIndex]);

                if (field.fieldType.isComplex()) {
                    byte[] bytes = new byte[0];
                    int offset = (int)field.dataOrDataOffset;
                    int size = 0;

                    switch (field.fieldType) {
                        case GFFFieldType.UNDEFINED:
                        Debug.Assert(false, "Field type of undefined should never make it into a file write!");
                        break;
                        case GFFFieldType.DWORD64:
                        //get next 8 bytes
                        bytes = new byte[8];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 8);
                        break;
                        case GFFFieldType.INT64:
                        //get next 8 bytes
                        bytes = new byte[8];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 8);
                        break;
                        case GFFFieldType.DOUBLE:
                        //get next 8 bytes
                        bytes = new byte[8];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 8);
                        break;
                        case GFFFieldType.CEXOSTRING:
                        //get size at offset
                        size = complexFieldData[offset];
                        bytes = new byte[size];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);    
                        //get next n-size bytes
                        break;
                        case GFFFieldType.RESREF:
                        //get size (1 byte)
                        size = (byte)complexFieldData[offset];
                        //get next n-size bytes
                        bytes = new byte[size];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 1, bytes, 0, size);
                        GFFResrefDataObject resref = new GFFResrefDataObject(ASCIIEncoding.ASCII.GetString(bytes));
                        break;
                        case GFFFieldType.CEXOLOCSTRING:
                        //get size
                        size = complexFieldData[offset];
                        //get next n-size bytes
                        bytes = new byte[size];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                        break;
                        case GFFFieldType.VOID:
                        //get size
                        size = complexFieldData[offset];
                        //get next n-size bytes
                        bytes = new byte[size];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                        break;
                        case GFFFieldType.STRUCT:
                        //index into struct array
                        size = complexFieldData[offset];
                        //get struct at index and add to verified structs
                        bytes = new byte[size];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                        break;
                        case GFFFieldType.LIST:
                        size = complexFieldData[offset];
                        //offset into field indices array ( factor of 4)
                        int index = listIndicesArray[offset / 4];
                        //the byte at this offset is the size of the list
                        //get indices in list and verify structs at these indices
                        bytes = new byte[size];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                        break;
                        case GFFFieldType.QUATERNION:
                        //16 bytes
                        bytes = new byte[16];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 16);
                        break;
                        case GFFFieldType.VECTOR:
                        //16 bytes
                        bytes = new byte[16];
                        Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 16);
                        break;
                        default:
                        bytes = new byte[0];
                        break;
                    }
                    verifiedComplexDataBytes.AddRange(bytes);
                    complexFieldCount++;
                } else {
                    simpleFieldCount++;
                }
            }

            Debug.Assert(labelMap.Count == verifiedLabels.Count);

            //Write Label Array

            String[] labels = labelMap.toArray();
            foreach (String label in labels) {
                String modifiedValue = label.Substring(0, Math.Min(label.Length, 16));
                newByteArray.AddRange(ASCIIEncoding.ASCII.GetBytes(modifiedValue));
                newByteArray.Add(0);
            }

            //Write Complex Field Data Block
            newByteArray.AddRange(complexFieldData.ToArray());

            //Write FieldIndices Array
            newByteArray.AddRange(fieldIndicesArray.SelectMany( index => BitConverter.GetBytes((uint)index).ToArray()));

            //Write ListIndices Array
            newByteArray.AddRange(listIndicesArray.SelectMany(index => BitConverter.GetBytes((uint)index).ToArray()));

            return newByteArray.ToArray();
        }

        public byte[] encode(AuroraDictionary dict) {
            //Processing prep work

            ByteArray complexFieldData = new ByteArray();
            List<GFFFieldInfo> allFields = new List<GFFFieldInfo>();
            List<GFFStructInfo> allStructs = new List<GFFStructInfo>();
            IndexMap<String> labelMap = new IndexMap<String>();
            List<int> listIndicesArray = new List<int>();
            List<int> fieldIndicesArray = new List<int>();

            /*
            GFFCoder.processStruct(baseStruct,
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
            newByteArray.AddRange(Encoding.ASCII.GetBytes(fileType));

            //Write File Version
            newByteArray.AddRange(Encoding.ASCII.GetBytes(fileVersion));

            //Write Struct Starting Offset
            //Header size is always 56, and Struct Array Offset always comes directly after the header
            int structStartingOffset = 56;
            newByteArray.AddRange(BitConverter.GetBytes(structStartingOffset));

            //Find all structs in AuroraDictionary (needs + 1 for the base struct)
            var structs = findStructs(dict);

            //Write Struct Count
            //The total count of structs in the file - this includes structs inside of fields that are struct types and list types
            UInt32 structCount = (UInt32)structs.Count();
            newByteArray.AddRange(BitConverter.GetBytes(structCount));

            //MARK: - Write Field offset 
            // Structs have 12 bytes of information, thus the field array starting offset will be 12 * stuctCount
            UInt32 fieldOffset = 56 + structCount * 12;
            newByteArray.AddRange(BitConverter.GetBytes(fieldOffset));

            //Find all fields in the structs
            var fields = structs.SelectMany(pair => pair).ToArray();

            //MARK: - Write field count (actual count)
            UInt32 fieldCount = (UInt32)fields.Count();
            newByteArray.AddRange(BitConverter.GetBytes(fieldCount));

            //MARK: - Write Label offset
            //Fields have 12 bytes of information, thus the label offset will be 12 * fieldCount
            UInt32 labelOffset = fieldOffset + fieldCount * 12;
            newByteArray.AddRange(BitConverter.GetBytes(labelOffset));

            //Put all of the field names into a hash
            var labelSet = new HashSet<string>(fields.Select(pair => pair.Key));

            //MARK: - Write Label count
            UInt32 labelCount = (UInt32)labelSet.Count();
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

            //Write Struct data
            HashSet<int> verifiedFields = new HashSet<int>();
            foreach (GFFStructInfo structInfo in allStructs)
            {
                newByteArray.AddRange(structInfo.toBytes());

                if (structInfo.fieldCount > 1)
                {
                    int startingIndex = (int)structInfo.dataOrDataOffset / 4;
                    for (int i = 0; i < structInfo.fieldCount; i++)
                    {
                        verifiedFields.Add(fieldIndicesArray[startingIndex + i]);
                    }
                }
                else if (structInfo.fieldCount == 1)
                {
                    verifiedFields.Add((int)structInfo.dataOrDataOffset);
                }
            }

            Debug.Assert(allFields.Count == verifiedFields.Count);

            //Write Field Data

            HashSet<string> verifiedLabels = new HashSet<string>();
            ByteArray verifiedComplexDataBytes = new ByteArray();
            HashSet<int> verifiedStructIndices = new HashSet<int>();

            int complexFieldCount = 0;
            int simpleFieldCount = 0;

            foreach (GFFFieldInfo field in allFields)
            {
                newByteArray.AddRange(field.toBytes());

                //Add to verified labels
                verifiedLabels.Add(labelMap[(int)field.labelIndex]);

                if (field.fieldType.isComplex())
                {
                    byte[] bytes = new byte[0];
                    int offset = (int)field.dataOrDataOffset;
                    int size = 0;

                    switch (field.fieldType)
                    {
                        case GFFFieldType.UNDEFINED:
                            Debug.Assert(false, "Field type of undefined should never make it into a file write!");
                            break;
                        case GFFFieldType.DWORD64:
                            //get next 8 bytes
                            bytes = new byte[8];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 8);
                            break;
                        case GFFFieldType.INT64:
                            //get next 8 bytes
                            bytes = new byte[8];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 8);
                            break;
                        case GFFFieldType.DOUBLE:
                            //get next 8 bytes
                            bytes = new byte[8];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 8);
                            break;
                        case GFFFieldType.CEXOSTRING:
                            //get size at offset
                            size = complexFieldData[offset];
                            bytes = new byte[size];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                            //get next n-size bytes
                            break;
                        case GFFFieldType.RESREF:
                            //get size (1 byte)
                            size = (byte)complexFieldData[offset];
                            //get next n-size bytes
                            bytes = new byte[size];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 1, bytes, 0, size);
                            GFFResrefDataObject resref = new GFFResrefDataObject(ASCIIEncoding.ASCII.GetString(bytes));
                            break;
                        case GFFFieldType.CEXOLOCSTRING:
                            //get size
                            size = complexFieldData[offset];
                            //get next n-size bytes
                            bytes = new byte[size];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                            break;
                        case GFFFieldType.VOID:
                            //get size
                            size = complexFieldData[offset];
                            //get next n-size bytes
                            bytes = new byte[size];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                            break;
                        case GFFFieldType.STRUCT:
                            //index into struct array
                            size = complexFieldData[offset];
                            //get struct at index and add to verified structs
                            bytes = new byte[size];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                            break;
                        case GFFFieldType.LIST:
                            size = complexFieldData[offset];
                            //offset into field indices array ( factor of 4)
                            int index = listIndicesArray[offset / 4];
                            //the byte at this offset is the size of the list
                            //get indices in list and verify structs at these indices
                            bytes = new byte[size];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset + 4, bytes, 0, size);
                            break;
                        case GFFFieldType.QUATERNION:
                            //16 bytes
                            bytes = new byte[16];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 16);
                            break;
                        case GFFFieldType.VECTOR:
                            //16 bytes
                            bytes = new byte[16];
                            Array.ConstrainedCopy(complexFieldData.ToArray(), offset, bytes, 0, 16);
                            break;
                        default:
                            bytes = new byte[0];
                            break;
                    }
                    verifiedComplexDataBytes.AddRange(bytes);
                    complexFieldCount++;
                }
                else
                {
                    simpleFieldCount++;
                }
            }

            Debug.Assert(labelMap.Count == verifiedLabels.Count);

            //Write Label Array

            String[] labels = labelMap.toArray();
            foreach (String label in labels)
            {
                String modifiedValue = label.Substring(0, Math.Min(label.Length, 16));
                newByteArray.AddRange(ASCIIEncoding.ASCII.GetBytes(modifiedValue));
                newByteArray.Add(0);
            }

            //Write Complex Field Data Block
            newByteArray.AddRange(complexFieldData.ToArray());

            //Write FieldIndices Array
            newByteArray.AddRange(fieldIndicesArray.SelectMany(index => BitConverter.GetBytes((uint)index).ToArray()));

            //Write ListIndices Array
            newByteArray.AddRange(listIndicesArray.SelectMany(index => BitConverter.GetBytes((uint)index).ToArray()));

            return newByteArray.ToArray();
        }

        private static AuroraStructType[] findStructs(AuroraStructType auroraStruct) {
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

        private static void processStruct(GFFStruct baseStruct,
            List<GFFFieldInfo> allFields,
            List<GFFStructInfo> allStructs,
            IndexMap<String> labelMap,
            ByteArray complexFieldData,
            List<int> fieldIndicesArray,
            List<int> listIndicesArray)
        {
            List<GFFFieldInfo> localFields = new List<GFFFieldInfo>();
            List<int> localFieldIndicesArray = new List<int>();

            KeyValuePair<String, GFFFieldDataObject>[] orderedPairs = baseStruct.fields.Select(pair => pair)
                .OrderByDescending( pair => pair.Value.subItemCount())
                .ToArray();

            int fieldCount = baseStruct.fields.Keys.Count;

            foreach (KeyValuePair<string, GFFFieldDataObject> pair in orderedPairs) {
                var dataObject = pair.Value;
                var label = pair.Key;
                var fieldType = dataObject.fieldType();

                labelMap.Add(label);

                GFFFieldInfo field;
                if (dataObject.isComplex()) {
                    switch (fieldType) {
                        case GFFFieldType.STRUCT: {
                            var structObject = pair.Value as GFFStructDataObject;
                            processStruct(structObject.structInfo, allFields, allStructs, labelMap, complexFieldData, localFieldIndicesArray, listIndicesArray);
                            field = new GFFFieldInfo(fieldType, (uint)labelMap[label], Convert.ToUInt32(allStructs.Count));                        
                        }
                        break;
                        case GFFFieldType.LIST: {
                            var listDataObject = pair.Value as GFFListDataObject;

                            //Add size of struct array to account for these structs;
                            List<int> localListIndicesArray = new List<int>();

                            //Ofset into listIndicesArray
                            int offset = listIndicesArray.Count * 4;
                            field = new GFFFieldInfo(fieldType, (uint)labelMap[label], Convert.ToUInt32(offset));
                            int size = listDataObject.structInfoArray.Count;
                            localListIndicesArray.Add(size);

                            foreach (GFFStruct gffStruct in listDataObject.structInfoArray) {
                                processStruct(gffStruct, allFields, allStructs, labelMap, complexFieldData, fieldIndicesArray, listIndicesArray);
                                localListIndicesArray.Add(allStructs.Count);
                            }

                            //Add to the total list indices
                            listIndicesArray.AddRange(localListIndicesArray.ToArray());
                        }
                        break;
                        default: {
                            field = new GFFFieldInfo(fieldType, (uint)labelMap[label], Convert.ToUInt32(complexFieldData.Count));
                            complexFieldData.AddRange(dataObject.toBytes());
                        }
                        break;
                    }
                } else {
                    field = new GFFFieldInfo(fieldType, (uint)labelMap[label], BitConverter.ToUInt32(dataObject.toBytes(), 0));
                }

                //THe field's index only has to be added to the fieldIndicesArray if it's part of a struct that has > 1 field
                if (fieldCount > 1) {
                    localFieldIndicesArray.Add(localFields.Count);
                }
                                
                localFields.Add(field);
            }

            GFFStructInfo structInfo;
            int currentFieldIndex = allFields.Count;
            if (localFields.Count > 1) {
                //Offset into fieldIndicesArray
                int fieldIndicesArrayOffset = fieldIndicesArray.Count * 4;
                structInfo = new GFFStructInfo(baseStruct.structType, localFields.Count, fieldIndicesArrayOffset);
                int[] modifiedListIndices = localFieldIndicesArray.Select(index => index + currentFieldIndex).ToArray();
                fieldIndicesArray.AddRange(modifiedListIndices);
            } else if (localFields.Count == 1) {
                structInfo = new GFFStructInfo(baseStruct.structType, 1, currentFieldIndex);
            } else {
                structInfo = new GFFStructInfo(baseStruct.structType, 0, -1);
            }
            
            allStructs.Add(structInfo);
            allFields.AddRange(localFields);    
        }

        private static GFFStruct[] allStructs(GFFStruct baseStruct) {
            List<GFFStruct> structs = new List<GFFStruct>();
            structs.Add(baseStruct);
            structs.AddRange(childStructs(baseStruct));
            return structs.ToArray();
        }
        private static GFFStruct[] childStructs(GFFStruct structInfo) {
            List<GFFStruct> structs = new List<GFFStruct>();

            foreach (KeyValuePair<string, GFFFieldDataObject> pair in structInfo.fields) {
                switch (pair.Value.fieldType()) {
                    case GFFFieldType.STRUCT: {
                        var structObject = pair.Value as GFFStructDataObject;
                        structs.Add(structObject.structInfo);
                        break;
                    }
                    case GFFFieldType.LIST: {
                        var listDataObject = pair.Value as GFFListDataObject;
                        structs.AddRange(listDataObject.structInfoArray);
                        break;
                    }
                    default: {
                        break;
                    }
                }
            }

            GFFStruct[] baseLevelStructs = structs.ToArray();
            foreach (GFFStruct subStruct in baseLevelStructs) {
                structs.AddRange(childStructs(subStruct));
            }

            return structs.ToArray();
        }

        private static Dictionary<GFFStruct, UInt32> structMap(GFFStruct startingStruct) {
            GFFStruct[] allStructs = childStructs(startingStruct);

            Dictionary<GFFStruct, UInt32> structMap = new Dictionary<GFFStruct, uint>();
            UInt32 structIndex = 0;
            foreach (GFFStruct structInfo in allStructs) {
                structMap[structInfo] = structIndex++;
            }
            return structMap;
        }
    }*/
}
