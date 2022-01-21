using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public enum GFFFieldType {
        UNDEFINED = -1,
        BYTE = 0,
        CHAR = 1,
        WORD = 2,
        SHORT = 3,
        DWORD = 4,
        INT = 5,
        DWORD64 = 6,
        INT64 = 7,
        FLOAT = 8,
        DOUBLE = 9,
        CEXOSTRING = 10,
        RESREF = 11,
        CEXOLOCSTRING = 12,
        VOID = 13,
        STRUCT = 14,
        LIST = 15,
        QUATERNION = 16,
        VECTOR = 17,
        STRREF = 18
    }

    public static class GFFFieldTypeExtensions {
        public static string stringValue(this GFFFieldType fieldType) {
            switch (fieldType) {
                case GFFFieldType.BYTE: return "byte";
                case GFFFieldType.CHAR: return "char";
                case GFFFieldType.WORD: return "word";
                case GFFFieldType.SHORT: return "short";
                case GFFFieldType.DWORD: return "dword";
                case GFFFieldType.INT: return "int";
                case GFFFieldType.DWORD64: return "dword64";
                case GFFFieldType.INT64: return "int64";
                case GFFFieldType.FLOAT: return "float";
                case GFFFieldType.DOUBLE: return "double";
                case GFFFieldType.CEXOSTRING: return "cexostring";
                case GFFFieldType.RESREF: return "resref";
                case GFFFieldType.CEXOLOCSTRING: return "cexolocstring";
                case GFFFieldType.VOID: return "void";
                case GFFFieldType.STRUCT: return "struct";
                case GFFFieldType.LIST: return "list";
                case GFFFieldType.QUATERNION: return "quaterntion";
                case GFFFieldType.VECTOR: return "vector";
                case GFFFieldType.STRREF: return "strref";
                default: return "invalid";
            }
        }

        public static GFFFieldType toGFFFieldType(this string s) {
            string modifiedString = s.ToLower();
            if (GFFFieldType.BYTE.stringValue() == modifiedString) {
                return GFFFieldType.BYTE;
            } else if (GFFFieldType.CHAR.stringValue() == modifiedString) {
                return GFFFieldType.CHAR;
            } else if (GFFFieldType.WORD.stringValue() == modifiedString) {
                return GFFFieldType.WORD;
            } else if (GFFFieldType.SHORT.stringValue() == modifiedString) {
                return GFFFieldType.SHORT;
            } else if (GFFFieldType.DWORD.stringValue() == modifiedString) {
                return GFFFieldType.DWORD;
            } else if (GFFFieldType.INT.stringValue() == modifiedString) {
                return GFFFieldType.INT;
            } else if (GFFFieldType.DWORD64.stringValue() == modifiedString) {
                return GFFFieldType.DWORD64;
            } else if (GFFFieldType.INT64.stringValue() == modifiedString) {
                return GFFFieldType.INT64;
            } else if (GFFFieldType.FLOAT.stringValue() == modifiedString) {
                return GFFFieldType.FLOAT;
            } else if (GFFFieldType.DOUBLE.stringValue() == modifiedString) {
                return GFFFieldType.DOUBLE;
            } else if (GFFFieldType.CEXOSTRING.stringValue() == modifiedString
                || "exostring" == modifiedString)
            {
                return GFFFieldType.CEXOSTRING;
            } else if (GFFFieldType.RESREF.stringValue() == modifiedString) {
                return GFFFieldType.RESREF;
            } else if (GFFFieldType.CEXOLOCSTRING.stringValue() == modifiedString
                || "exolocstring" == modifiedString)
            {
                return GFFFieldType.CEXOLOCSTRING;
            } else if (GFFFieldType.VOID.stringValue() == modifiedString) {
                return GFFFieldType.VOID;
            } else if (GFFFieldType.STRUCT.stringValue() == modifiedString) {
                return GFFFieldType.STRUCT;
            } else if (GFFFieldType.LIST.stringValue() == modifiedString) {
                return GFFFieldType.LIST;
            } else if (GFFFieldType.QUATERNION.stringValue() == modifiedString) {
                return GFFFieldType.QUATERNION;
            } else if (GFFFieldType.VECTOR.stringValue() == modifiedString) {
                return GFFFieldType.VECTOR;
            } else {
                return GFFFieldType.UNDEFINED;
            }
        }
    }
}
