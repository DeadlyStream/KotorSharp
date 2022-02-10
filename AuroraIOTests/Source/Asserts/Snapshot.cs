using AuroraIO.Source.Coders;
using AuroraIO.Source.Models._2da;
using AuroraIO.Source.Models.Dictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AuroraIOTests.Source.Asserts {
    public static class Snapshot {

        static string testFileBaseName(MethodBase methodName) {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Artifacts",
                String.Format("{0}\\{1}", methodName.DeclaringType.Name, methodName.Name));
        }

        static string testFileActualName(MethodBase methodName) {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Artifacts",
                String.Format("{0}\\{1}_actual", methodName.DeclaringType.Name, methodName.Name));
        }

        public static void VerifyEncoding(_2DAObject actual, MethodBase methodName, bool record) {
            Verify(new _2DACoder().encode(actual), methodName, record);
        }

        public static void VerifyEncoding(AuroraDictionary actual, MethodBase methodName, bool record) {
            Verify(new GFFCoder().encode(actual), methodName, record);
        }

        public static void Verify(ASCIIOutputProtocol actual, MethodBase methodName, bool record) {
            Verify(new ASCIICoder().encode(actual), methodName, record);
        }

        public static void Verify(byte[] actual, MethodBase methodName, bool record) {
            var path = testFileBaseName(methodName);

            if (record) {
                if (!Directory.Exists(Path.GetDirectoryName(path))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                File.WriteAllBytes(path, actual);
            }
            File.WriteAllBytes(testFileActualName(methodName), actual);
            var expected = File.ReadAllBytes(path);
            Assert.AreEqual(Encoding.ASCII.GetString(actual), Encoding.ASCII.GetString(expected));
        }

        public static void Verify(String actual, MethodBase methodName, bool record) {
            var path = testFileBaseName(methodName);

            if (record) {
                if (!Directory.Exists(Path.GetDirectoryName(path))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                File.WriteAllText(path, actual);
            }
            File.WriteAllText(testFileActualName(methodName), actual);
            var expected = File.ReadAllText(path);
            Assert.AreEqual(actual, expected);
        }
    }
}
