using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AuroraIOTests.Source {
    [TestClass]
    public class GFFCoderEncodingTests {

        GFFCoder coder = new GFFCoder();

        [TestMethod]
        public void testEncodeGameFile() {
            var table = coder.decode(Snapshot.DataResource());

            var newTable = coder.decode(coder.encode(table));
            Snapshot.Verify(newTable);
        }

        [TestMethod]
        public void testEncodeEmpty() {
            var dict = AuroraDictionary.make("GFF", dict => {

            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeByte() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (byte)100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeChar() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = 'c';
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeWord() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (ushort)100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeShort() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (short)100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeInt() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = 100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeDword64() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (ulong)100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeInt64() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (long)100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeFloat() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = 100f;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeDouble() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = 100.0;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeCExoString() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = "stringValue";
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeCResref() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = AuroraResref.make("xxxxxxxxxxxxxxxx");
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeCExoLocString() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = AuroraLocalizedString.make(uint.MaxValue);
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeVoid() {
            var dict = AuroraDictionary.make("GFF", dict => {
                byte[] byteArray = new byte[byte.MaxValue];
                for (int i = 0; i < byte.MaxValue; i++) {
                    byteArray[i] = (byte)i;
                }
                dict["value"] = byteArray;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeStruct() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = AuroraStruct.make(0);
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeList() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["value"] = 100;
                    })
                };
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeQuaternion() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (0.1f, 0.2f, 0.3f, 0.4f);
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeVector() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = (0.1f, 0.2f, 0.3f);
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeStrref() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["value"] = AuroraStrRef.make(uint.MaxValue);
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncode4SimpleField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = (byte)0;
                dict["field1"] = (byte)1;
                dict["field2"] = (byte)2;
                dict["field3"] = (byte)3;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncode4ComplexField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = "string0";
                dict["field1"] = "string1";
                dict["field2"] = "string2";
                dict["field3"] = "string3";
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeMixedSimpleComplexField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = 0;
                dict["field1"] = 1;
                dict["field2"] = "string2";
                dict["field3"] = "string3";
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }


        [TestMethod]
        public void testEncode4ListField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["field0-item"] = (byte)0;
                    })
                };
                dict["field1"] = new AuroraStruct[] {
                    AuroraStruct.make(1, dict => {
                        dict["field1-item"] = (byte)1;
                    })
                };
                dict["field2"] = new AuroraStruct[] {
                    AuroraStruct.make(2, dict => {
                        dict["field2-item"] = (byte)2;
                    })
                };
                dict["field3"] = new AuroraStruct[] {
                    AuroraStruct.make(3, dict => {
                        dict["field3-item"] = (byte)3;
                    })
                };
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncode4StructField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = AuroraStruct.make(0, dict => {
                    dict["field0-item"] = (byte)0;
                });
                dict["field1"] = AuroraStruct.make(1, dict => {
                    dict["field1-item"] = (byte)1;
                });
                dict["field2"] = AuroraStruct.make(2, dict => {
                    dict["field2-item"] = (byte)2;
                });
                dict["field3"] = AuroraStruct.make(3, dict => {
                    dict["field3-item"] = (byte)3;
                });
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeMixedListStructField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["field0-item"] = (byte)0;
                    })
                };
                dict["field1"] = new AuroraStruct[] {
                    AuroraStruct.make(1, dict => {
                        dict["field1-item"] = (byte)1;
                    })
                };
                dict["field2"] = AuroraStruct.make(2, dict => {
                    dict["field2-item"] = (byte)2;
                });
                dict["field3"] = AuroraStruct.make(3, dict => {
                    dict["field3-item"] = (byte)3;
                });
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }

        [TestMethod]
        public void testEncodeNestedListField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["field1"] = new AuroraStruct[] {
                            AuroraStruct.make(1, dict => {
                                dict["field2"] = new AuroraStruct[] {
                                    AuroraStruct.make(2, dict => {
                                        dict["field3"] = 100;
                                    })
                                };
                            })
                        };
                    })
                };
                dict["field4"] = 100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict, true);
        }

        [TestMethod]
        public void testEncodeNestedStructField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field0"] = AuroraStruct.make(0, dict => {
                    dict["field1"] = 100;
                });
                dict["field2"] = 100;
            });

            var newDict = coder.decode(coder.encode(dict));
            Snapshot.Verify(newDict);
        }
    }
}
