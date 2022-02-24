using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Dictionary
{
    public enum AuroraDataType {
        Undefined = -1,
        Byte = 0,
        Char = 1,
        Word = 2,
        Short = 3,
        Dword = 4,
        Int = 5,
        Dword64 = 6,
        Int64 = 7,
        Float = 8,
        Double = 9,
        CExoString = 10,
        CResref = 11,
        CExoLocString = 12,
        Void = 13,
        Struct = 14,
        List = 15,
        Quaternion = 16,
        Vector = 17,
        StrRef = 18
    }
}
