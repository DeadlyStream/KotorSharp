using AuroraIO;
using AuroraIO.Models;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using AuroraIOTests.Properties;
using AuroraIOTests.Source.Asserts;
using AuroraIOTests.Source.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AuroraIOTests.Source {
    [TestClass]
    public class GFFCoderTests {

        bool record = false;

        GFFCoder coder = new GFFCoder();


        [TestMethod]
        public void testEncodeEmptyGFF() {
            var dict = AuroraDictionary.make("GFF", dict => { });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }

        [TestMethod]
        public void testEncodeStructWith1SimpleField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field_byte"] = (byte)100;
            });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }

        [TestMethod]
        public void testEncodeStructWithMoreThan1SimpleField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field_byte"] = (byte)100;
                dict["field_dword"] = (uint)100;
            });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }

        [TestMethod]
        public void testEncodeStructWith1ComplexField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["cexostring"] = "testString";
            });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }

        [TestMethod]
        public void testEncodeStructWithMoreThan1ComplexField() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field_string"] = "test_string";
                dict["field_int64"] = (Int64)100;
            });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }

        [TestMethod]
        public void testEncodeStructWith1List() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field_list"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["id"] = (int)10;
                    }),
                };
            });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                false);
        }

        [TestMethod]
        public void testEncodeStructWithMoreThan1List() {
            var dict = AuroraDictionary.make("GFF", dict => {
                dict["field_list1"] = new AuroraStruct[] {
                    AuroraStruct.make(0, dict => {
                        dict["id"] = (int)10;
                        dict["message"] = "value";
                    }),
                    AuroraStruct.make(1, dict => {
                        dict["id"] = (int)10;
                        dict["message"] = "value";
                    }),
                    AuroraStruct.make(2, dict => {
                        dict["id"] = (int)10;
                        dict["message"] = "value";
                    })
                };
                dict["field_list2"] = new AuroraStruct[] {
                    AuroraStruct.make(3, dict => {
                        dict["id"] = (int)10;
                        dict["message"] = "value";
                    }),
                    AuroraStruct.make(4, dict => {
                        dict["id"] = (int)10;
                        dict["message"] = "value";
                    }),
                    AuroraStruct.make(5, dict => {
                        dict["id"] = (int)10;
                        dict["message"] = "value";
                    })
                };
                dict["field_string"] = "test_string";
                dict["field_int64"] = (Int64)100;
            });

            Snapshot.VerifyEncoding(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }

        /*
        [TestMethod]
        public void testWriteAuroraDictionary() {

            byte[] data = coder.encode(AuroraDictionaryStubs.stub1());

            AuroraDictionary dict = coder.decode(data);
            AIOAssert.VerifyFile(
                dict,
                MethodBase.GetCurrentMethod(),
                record);
        }*/
        /*
        [TestMethod]
        public void testReadGFFFile() {
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObjectCoder gffCoder = new GFFObjectCoder();
            GFFObject gffObject = gffCoder.decode(Resources.TestDataTypes);

            string gffString = asciiCoder.encode(gffObject);
            Console.Write(gffString);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
            return;
        }

        [TestMethod]
        public void testWriteGFFFile() {


        }

        //byte

        [TestMethod]
        public void testAddByte() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here
            gffObject["testField"] = new GFFByteDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetByteExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // char

        [TestMethod]
        public void testAddChar() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFCharDataObject('c');

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetCharExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // word 
        [TestMethod]
        public void testAddWord() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFWordDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetWordExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // short
        [TestMethod]
        public void testAddShort() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFShortDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        public void testSetShortExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        //dword
        [TestMethod]
        public void testAddDword() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFDWordDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetDwordExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // int
        [TestMethod]
        public void testAddInt() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFIntDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetIntExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // dword 64
        [TestMethod]
        public void testAddDword64() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFDWord64DataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetDword64Existing() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // int64
        [TestMethod]
        public void testAddInt64() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFInt64DataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetInt64Existing() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // float
        [TestMethod]
        public void testAddFloat() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFFloatDataObject(1);
            gffObject["testField2"] = new GFFFloatDataObject(2);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetFloatExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // double
        public void testAddDouble() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetDoubleExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // cexostring
        [TestMethod]
        public void testAddCexostring() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFCExoStringDataObject("testString");

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetCexostringExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // resref
        [TestMethod]
        public void testAddResref() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFResrefDataObject("testResref");


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetResrefExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // cexolocstring
        [TestMethod]
        public void testAddCexolocstringStrRef() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFCExoLocStringDataObject(uint.MaxValue, new Dictionary<GFFLanguage, String>());


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetCexolocstringExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetLanguageOnCexolocstringExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetStrRefOnCexolocstringExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // void
        [TestMethod]
        public void testAddVoid() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFVoidDataObject(new byte[] { 0x01, 0x02, 0x03, 0x04 });


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetVoidExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // struct
        [TestMethod]
        public void testAddStruct() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            var structInfo = new GFFStruct(uint.MaxValue);
            gffObject["testField"] = structInfo;


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetStructExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // list
        [TestMethod]
        public void testAddList() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFListDataObject(
                    new GFFStruct[] {
                        new GFFStruct(uint.MaxValue)
                    }
            );


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetListExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testAddStructToList() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testRemoveStructFromList() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        //quaternion

        [TestMethod]
        public void testAddQuaternion() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFQuaternionDataObject(0.0, 0.1, 0.2, 0.3);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetXQuaternionExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetYQuaternionExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetWQuaternionExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetZQuaternionExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // vector
        [TestMethod]
        public void testAddVector() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFVectorDataObject(0.0, 0.1, 0.2);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetXVectorExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetYVectorExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetZVectorExisting() {
            GFFObjectCoder gFFCoder = new GFFObjectCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        private bool verifyTest(string value1, string methodName) {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + methodName;
            if (true) {
                File.WriteAllText(filePath, value1);
                return true;
            } else {
                return value1.Equals(File.ReadAllText(filePath));
            }
        }
        */
    }
}
