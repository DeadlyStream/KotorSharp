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
    public class GFFCoderDecodingTests {

        GFFCoder coder = new GFFCoder();

        [TestMethod]
        public void testDecodeEmpty() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeByte() {
            var file = Snapshot.DataResource();
  
            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeChar() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeWord() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeShort() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeInt() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeDword64() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeInt64() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeFloat() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeDouble() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeCExoString() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeCResref() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeCExoLocString() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeVoid() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeStruct() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeList() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeQuaternion() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeVector() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeStrref() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecode4SimpleField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecode4ComplexField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeMixedSimpleComplexField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }


        [TestMethod]
        public void testDecode4ListField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecode4StructField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeMixedListStructField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeNestedListField() {
            var file = Snapshot.DataResource();

            Snapshot.Verify(coder.decode(file));
        }        
    }
}
