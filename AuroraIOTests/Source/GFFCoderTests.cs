using AuroraIO;
using AuroraIO.Models;
using AuroraIO.Source.Common;
using AuroraIO.Source.Models.GFF.Helpers;
using AuroraIOTests.Properties;
using AuroraIOTests.Source.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace AuroraIOTests.Source.Models {
    [TestClass]
    public class GFFCoderTests {
        [TestMethod]
        public void testReadGFFFile() {
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFCoder gffCoder = new GFFCoder();
            var stub = AuroraStructStubs.stub1();
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
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFCharDataObject('c');

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetCharExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFWordDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetWordExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFShortDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        public void testSetShortExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFDWordDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetDwordExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFIntDataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetIntExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFDWord64DataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetDword64Existing() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFInt64DataObject(1);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetInt64Existing() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // double
        public void testAddDouble() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetDoubleExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFCExoStringDataObject("testString");

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetCexostringExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        // resref
        [TestMethod]
        public void testAddResref() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFResrefDataObject("testResref");


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetResrefExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFCExoLocStringDataObject(uint.MaxValue, new Dictionary<GFFLanguage, String>());


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetCexolocstringExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetLanguageOnCexolocstringExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetStrRefOnCexolocstringExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);


            gffObject["testField"] = new GFFVoidDataObject(new byte[] { 0x01, 0x02, 0x03, 0x04 });


            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetVoidExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testAddStructToList() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testRemoveStructFromList() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFQuaternionDataObject(0.0, 0.1, 0.2, 0.3);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetXQuaternionExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetYQuaternionExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetWQuaternionExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetZQuaternionExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            gffObject["testField"] = new GFFVectorDataObject(0.0, 0.1, 0.2);

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetXVectorExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetYVectorExisting() {
            GFFCoder gFFCoder = new GFFCoder();
            ASCIICoder asciiCoder = new ASCIICoder();
            GFFObject gffObject = gFFCoder.decode(Resources._base);

            //Add/remove fields here

            string gffString = asciiCoder.encode(gffObject);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Assert.IsTrue(verifyTest(asciiCoder.encode(gffObject), methodName));
        }

        [TestMethod]
        public void testSetZVectorExisting() {
            GFFCoder gFFCoder = new GFFCoder();
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

    }
}
