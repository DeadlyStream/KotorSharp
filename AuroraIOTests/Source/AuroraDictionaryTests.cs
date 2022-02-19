using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Base;
using AuroraIO.Source.Models.Dictionary;
using AuroraIOTests.Source.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuroraIOTests.Source
{
    [TestClass]
    public class AuroraDictionaryTests
    {

        [TestMethod]
        public void testPopulateDictionary()
        {
            var dict = AuroraDictionary.make(dict =>
            {
                dict["field_byte"] = (byte)0;
                dict["field_char"] = 'c';
                dict["field_word"] = (ushort)0;
                dict["field_short"] = (short)0;
                dict["field_dword"] = (uint)0;
                dict["field_int"] = (int)0;
                dict["field_dword64"] = (ulong)0;
                dict["field_int64"] = (long)0;
                dict["field_float"] = 0.0f;
                dict["field_double"] = 0.0;
                dict["field_cexoString"] = "cexostring";
                dict["field_resref"] = AuroraResref.make("resref");
                dict["field_cexolocstring"] = AuroraLocalizedString.make(dict => {
                    dict[CExoLanguage.EnglishFemale] = "female";
                });
                dict["void"] = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                dict["field_struct"] = AuroraStruct.make(dict => {
                    dict["field_struct.int"] = 0;
                    dict["field_struct.byte"] = (byte)0;
                });
                dict["field_list"] = new AuroraStruct[]
                {
                    AuroraStruct.make(0, (dict) =>
                    {
                        dict["sub1"] = 0;
                        dict["sub2"] = "testString";
                    }),
                    AuroraStruct.make(1, (dict) =>
                    {
                        dict["sub1"] = 0;
                        dict["sub2"] = "testString";
                    }),
                    AuroraStruct.make(2, (dict) =>
                    {
                        dict["sub1"] = 0;
                        dict["sub2"] = "testString";
                    })
                };
                dict["field_quaternion"] = (0.0f, 0.0f, 0.0f, 0.0f);
                dict["field_vector"] = (0.0f, 0.0f, 0.0f);
                dict["field_strref"] = AuroraStrRef.make(0);
            });

            Snapshot.Verify(dict);
        }

        [TestMethod]
        public void testNestedListDictionary()
        {
            var dict = AuroraDictionary.make(dict =>
            {
                dict["baseList"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["list1"] = new AuroraStruct[] {
                            AuroraStruct.make(0)
                        };
                    }),
                    AuroraStruct.make(0, dict => {
                        dict["list2"] = new AuroraStruct[] {
                            AuroraStruct.make(0)
                        };
                    }),
                    AuroraStruct.make(0, dict => {
                        dict["list3"] = new AuroraStruct[] {
                            AuroraStruct.make(0)
                        };
                    }),
                };
            });
            Snapshot.Verify(dict);
        }

        [TestMethod]
        public void testNestedStructDictionary()
        {
            var dict = AuroraDictionary.make(dict =>
            {
                dict["struct"] = AuroraStruct.make(dict =>
                {
                    dict["int"] = 0;
                    dict["float"] = 0.0f;
                    dict["struct"] = AuroraStruct.make(dict =>
                    {
                        dict["int"] = 0;
                        dict["float"] = 0.0f;
                    });
                });
            });
            Snapshot.Verify(dict);
        }
    }
}
