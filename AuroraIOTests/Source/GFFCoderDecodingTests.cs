using AuroraIO;
using AuroraIO.Models;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using AuroraIOTests.Properties;
using AuroraIOTests.Source.Stubs;
using KSnapshot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AuroraIOTests.Source {
    [TestClass]
    public class GFFCoderDecodingTests {

        ResourceBundle resources = ResourceBundle.GetCurrent();

        GFFCoder coder = new GFFCoder();

        [TestMethod]
        public void testDecodeGameFile() {
            var file = resources.GetFileBytes("kreia.dlg");
            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeEmpty() {
            var file = resources.GetFileBytes("empty.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeByte() {
            var file = resources.GetFileBytes("byte.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeChar() {
            var file = resources.GetFileBytes("char.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeWord() {
            var file = resources.GetFileBytes("word.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeShort() {
            var file = resources.GetFileBytes("short.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeInt() {
            var file = resources.GetFileBytes("int.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeDword64() {
            var file = resources.GetFileBytes("dword64.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeInt64() {
            var file = resources.GetFileBytes("int64.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeFloat() {
            var file = resources.GetFileBytes("float.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeDouble() {
            var file = resources.GetFileBytes("double.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeCExoString() {
            var file = resources.GetFileBytes("CExoString.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeCResref() {
            var file = resources.GetFileBytes("CResref.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeCExoLocString() {
            var file = resources.GetFileBytes("CExoLocString.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeVoid() {
            var file = resources.GetFileBytes("void.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeStruct() {
            var file = resources.GetFileBytes("struct.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeList() {
            var file = resources.GetFileBytes("list.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeQuaternion() {
            var file = resources.GetFileBytes("quaternion.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeVector() {
            var file = resources.GetFileBytes("vector.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeStrref() {
            var file = resources.GetFileBytes("strref.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecode4SimpleField() {
            var file = resources.GetFileBytes("4Simples.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecode4ComplexField() {
            var file = resources.GetFileBytes("4Complexs.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeMixedSimpleComplexField() {
            var file = resources.GetFileBytes("mixedSimpleComplex.gff");

            Snapshot.Verify(coder.decode(file));
        }


        [TestMethod]
        public void testDecode4ListField() {
            var file = resources.GetFileBytes("4Lists.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecode4StructField() {
            var file = resources.GetFileBytes("4Structs.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeMixedListStructField() {
            var file = resources.GetFileBytes("mixedListStruct.gff");

            Snapshot.Verify(coder.decode(file));
        }

        [TestMethod]
        public void testDecodeNestedListField() {
            var file = resources.GetFileBytes("nestedLists.gff");

            Snapshot.Verify(coder.decode(file));
        }        
    }
}
