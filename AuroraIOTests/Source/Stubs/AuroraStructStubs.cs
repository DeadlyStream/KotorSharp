using AuroraIO.Models;
using AuroraIO.Source.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraIOTests.Source.Stubs
{
    public static class AuroraStructStubs {
        public static AuroraStruct stub1() {
            var arStruct = new AuroraStruct();

            arStruct["byte"] = (byte)0;
            arStruct["char"] = 'c';
            arStruct["word"] = (ushort)0;
            arStruct["short"] = (short)0;
            arStruct["dword"] = (uint)0;
            arStruct["int"] = (int)0;
            arStruct["dword64"] = (ulong)0;
            arStruct["int64"] = (long)0;
            arStruct["float"] = 0.0f;
            arStruct["double"] = 0.0;
            arStruct["CexoString"] = "cexostring";
            arStruct["resref"] = AuroraResref.make("resref");
            arStruct["cexolocstring"] = AuroraCExoLocString.make(dict => {
                dict[CExoLanguage.EnglishFemale] = "female";
            });
            arStruct["void"] = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            arStruct["struct"] = AuroraStruct.make(dict => {
                dict["struct.int"] = 0;
                dict["struct.byte"] = (byte)0;
            });
            arStruct["list"] = new AuroraStruct[]
            {
                new AuroraStruct(0),
                new AuroraStruct(1),
                new AuroraStruct(2)
            };
            arStruct["quaternion"] = (0.0f, 0.0f, 0.0f, 0.0f);
            arStruct["vector"] = (0.0f, 0.0f, 0.0f);
            arStruct["strref"] = AuroraStrRef.make(0);
            return arStruct;
        }
    }
}
