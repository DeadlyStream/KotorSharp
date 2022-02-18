using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using AuroraIO.Source.Models.Table;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AuroraIOTests.Source.Asserts {
    public static class Snapshot {

        static string testFileBaseName(string className, string methodName) {
            return Path.Combine(Bundle.SnapshotDirectory,
                "Artifacts",
                String.Format("{0}\\{1}", Path.GetFileNameWithoutExtension(className), methodName));
        }

        private static string resourceFileName(string className, string methodName) {
            return Path.Combine(Bundle.SnapshotDirectory,
                "Resources",
                String.Format("{0}\\{1}", Path.GetFileNameWithoutExtension(className), methodName));
        }

        static string testFileOutputName(string className,  string methodName) {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Artifacts",
                String.Format("{0}\\{1}_actual", Path.GetFileNameWithoutExtension(className), methodName));
        }

        public static byte[] DataResource([CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
            return File.ReadAllBytes(resourceFileName(className, methodName));
        }

        public static string TextResource([CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
            return File.ReadAllText(resourceFileName(className, methodName));
        }

        public static void VerifyEncoding(AuroraTable actual, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "", bool record = false) {
            Verify(new _2DACoder().encode(actual), className, methodName, record);
        }

        public static void VerifyEncoding(AuroraDictionary actual, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "", bool record = false) {
            Verify(new GFFCoder().encode(actual), className, methodName, record);
        }

        public static void Verify(ASCIIOutputProtocol actual, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "", bool record = false) {
            Verify(new ASCIICoder().encode(actual), className, methodName, record);
        }

        public static void Verify(byte[] actual, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "", bool record = false) {
            var expectedPath = testFileBaseName(className, methodName);
            var actualPath = testFileOutputName(className, methodName);

            if (record) {
                if (!Directory.Exists(Path.GetDirectoryName(expectedPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(expectedPath));
                }
                File.WriteAllBytes(expectedPath, actual);
            } else {
                if (!Directory.Exists(Path.GetDirectoryName(actualPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(actualPath));
                }
            }
            File.WriteAllBytes(actualPath, actual);
            var expected = File.ReadAllBytes(expectedPath);
            Assert.AreEqual(Encoding.ASCII.GetString(expected), Encoding.ASCII.GetString(actual));
        }

        public static void Verify(String actual, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "", bool record = false) {
            var expectedPath = testFileBaseName(className, methodName);
            var actualPath = testFileOutputName(className, methodName);

            if (record) {
                if (!Directory.Exists(Path.GetDirectoryName(expectedPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(expectedPath));
                }
                File.WriteAllText(expectedPath, actual);
            } else {
                if (!Directory.Exists(Path.GetDirectoryName(actualPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(actualPath));
                }
            }
            File.WriteAllText(actualPath, actual);
            var expected = File.ReadAllText(expectedPath);
            Assert.AreEqual(expected, actual);
        }
    }
}
