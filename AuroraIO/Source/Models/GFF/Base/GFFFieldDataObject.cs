using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AuroraIO.Source.Models.GFF;
using AuroraIO.Source.Coders;

namespace AuroraIO {

    public interface GFFFieldDataObject: ASCIIOutputProtocol {
        GFFFieldType fieldType();
        int dataLength();
        byte[] toBytes();
        void setValueForPath(object value, GFFPath path);
        Object getValueAtPath(GFFPath path);
        int subItemCount();
    }

    static class GFFFieldTypeParsingExtensions {
        public static bool isComplex(this GFFFieldDataObject dataObject) {
            return dataObject.fieldType().isComplex();
        }

        public static bool isComplex(this GFFFieldType fieldType) {
            switch (fieldType) {
                case GFFFieldType.BYTE:
                case GFFFieldType.CHAR:
                case GFFFieldType.WORD:
                case GFFFieldType.SHORT:
                case GFFFieldType.DWORD:
                case GFFFieldType.INT:
                case GFFFieldType.FLOAT:
                case GFFFieldType.UNDEFINED:
                    return false;
                case GFFFieldType.DWORD64:
                case GFFFieldType.INT64:
                case GFFFieldType.DOUBLE:
                case GFFFieldType.CEXOSTRING:
                case GFFFieldType.RESREF:
                case GFFFieldType.CEXOLOCSTRING:
                case GFFFieldType.VOID:
                case GFFFieldType.STRUCT:
                case GFFFieldType.LIST:
                case GFFFieldType.QUATERNION:
                case GFFFieldType.VECTOR:
                case GFFFieldType.STRREF:
                return true;
                default:
                    return false;
            }
        }

        public static GFFFieldDataObject parseData(this GFFFieldType type, byte[] fileArray, int offset) {
            switch (type) {
                //Simple data parses from an offset that lies right with the field data
                case GFFFieldType.BYTE:
                    return new GFFByteDataObject(fileArray[offset]);
                case GFFFieldType.CHAR:
                    return new GFFCharDataObject((char)fileArray[offset]);
                case GFFFieldType.WORD:
                    return new GFFWordDataObject(BitConverter.ToUInt16(fileArray, offset));
                case GFFFieldType.SHORT:
                    return new GFFShortDataObject(BitConverter.ToInt16(fileArray, offset));
                case GFFFieldType.DWORD:
                    return new GFFDWordDataObject(BitConverter.ToUInt32(fileArray, offset));
                case GFFFieldType.INT:
                    return new GFFIntDataObject(BitConverter.ToInt32(fileArray, offset));
                case GFFFieldType.FLOAT:
                    return new GFFFloatDataObject(BitConverter.ToSingle(fileArray, offset));
                case GFFFieldType.UNDEFINED:
                    return null;
                //Complex data parses data in the field data heap
                case GFFFieldType.DWORD64:
                    return new GFFDWord64DataObject(BitConverter.ToUInt64(fileArray, offset));
                case GFFFieldType.INT64:
                    return new GFFInt64DataObject(BitConverter.ToInt64(fileArray, offset));
                case GFFFieldType.DOUBLE:
                    return new GFFDoubleDataObject(BitConverter.ToDouble(fileArray, offset));
                case GFFFieldType.CEXOSTRING:
                    UInt32 stringLength = BitConverter.ToUInt32(fileArray, offset);
                    String cexoString =  Encoding.ASCII.GetString(fileArray, offset + 4, (int)stringLength);
                    return new GFFCExoStringDataObject(cexoString);
                case GFFFieldType.RESREF:
                    byte resrefLength = fileArray[offset];
                    String resrefString = Encoding.ASCII.GetString(fileArray, offset + 1, resrefLength);
                    return new GFFResrefDataObject(resrefString);
                case GFFFieldType.CEXOLOCSTRING:
                    UInt32 dexoLocStringSize = BitConverter.ToUInt32(fileArray, offset);
                    UInt32 stringResRef = BitConverter.ToUInt32(fileArray, offset + 4);
                    UInt32 totalSubstrings = BitConverter.ToUInt32(fileArray, offset + 8);

                    Dictionary<GFFLanguage, string> cexoSubstrings = new Dictionary<GFFLanguage, string>();
                    int substringStartingOffset = offset + 12;
                    for (int i = 0; i < (int)totalSubstrings; i++) {
                        GFFLanguage stringID = (GFFLanguage)BitConverter.ToUInt32(fileArray, substringStartingOffset);
                        UInt32 subStringLength = BitConverter.ToUInt32(fileArray, substringStartingOffset + 4);
                        String cexoSubstring = Encoding.ASCII.GetString(fileArray, substringStartingOffset + 8, (int)subStringLength);
                        cexoSubstrings[stringID] = cexoSubstring;
                        substringStartingOffset += (int)subStringLength + 8;
                    }
                    return new GFFCExoLocStringDataObject(stringResRef, cexoSubstrings);
                case GFFFieldType.VOID:
                    UInt32 dataLength = BitConverter.ToUInt32(fileArray, offset);
                    ArraySegment<byte> arraySlice = new ArraySegment<byte>(fileArray, offset + 4, (int)dataLength);
                    return new GFFVoidDataObject(arraySlice.Array);
                case GFFFieldType.QUATERNION:
                    float qW = BitConverter.ToSingle(fileArray, offset);
                    float qX = BitConverter.ToSingle(fileArray, offset + 4);
                    float qY = BitConverter.ToSingle(fileArray, offset + 8);
                    float qZ = BitConverter.ToSingle(fileArray, offset + 12);
                    return new GFFQuaternionDataObject(qW, qX, qY, qZ);
                case GFFFieldType.VECTOR:
                    float vX = BitConverter.ToSingle(fileArray, offset);
                    float vY = BitConverter.ToSingle(fileArray, offset + 4);
                    float vZ = BitConverter.ToSingle(fileArray, offset + 8);
                    return new GFFVectorDataObject(vX, vY, vZ);
                case GFFFieldType.STRREF:
                    //Even in xoreos, this is hardcoded to 4, not sure why
                    int unknownHeader = BitConverter.ToInt32(fileArray, offset);
                    uint strRefValue = BitConverter.ToUInt32(fileArray, offset + 4);
                    return new GFFStrRefObject(strRefValue);
                //Structs are different and get an offset from struct array
                case GFFFieldType.STRUCT:
                //Lists are different and get an offset from list indices array
                case GFFFieldType.LIST:
                default:
                    return null;
            }
        }
    }
}

