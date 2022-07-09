﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using YAMLEncoding;

namespace KSnapshot {

    public class Bundle {

        internal readonly string ProjectDirectory;
        internal readonly string SnapshotDirectory;

        public static Bundle current([CallerFilePath] string className = "") {
            return new Bundle(className);
        }

        Bundle([CallerFilePath] string className = "") {
            ProjectDirectory = Path.GetDirectoryName(className);
            SnapshotDirectory = Path.Combine(ProjectDirectory, "Snapshots");
        } 
    }

    public class KSEnvironment {
        public static Bundle Bundle = Bundle.current();
    }

    public static class Snapshot {

        static string testFileBaseName(string className, string methodName) {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Artifacts",
                String.Format("{0}\\{1}", Path.GetFileNameWithoutExtension(className), methodName));
        }

        static string testFileOutputName(string className, string methodName) {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Artifacts",
                String.Format("{0}\\{1}_actual", Path.GetFileNameWithoutExtension(className), methodName));
        }

        public static string OutputPath(bool recorded = false, [CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            if (recorded) {
                return testFileBaseName(className, methodName);
            } else {
                return testFileOutputName(className, methodName);
            }
        }

        public static string PatchDataDirectory([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                String.Format("{0}\\{1}_dir", Path.GetFileNameWithoutExtension(className), methodName),
                "tslpatchdata");
        }

        public static string RootGameDirectory([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                "gameRoot");
        }

        public static string ResourceDirectory([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                String.Format("{0}\\{1}_dir", Path.GetFileNameWithoutExtension(className), methodName));
        }

        public static string ResourcePath([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                String.Format("{0}\\{1}", Path.GetFileNameWithoutExtension(className), methodName));
        }

        public static byte[] DataResource([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return File.ReadAllBytes(ResourcePath(methodName, className));
        }

        public static string TextResource([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return File.ReadAllText(ResourcePath(methodName, className));
        }

        public static void Verify(YAMLEncodingProtocol actual, bool record = false, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
            Verify(new YAMLCoder().encode(actual), record, className, methodName);
        }

        public static void Verify(YAMLEncodingProtocol[] actual, bool record = false, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
            Verify(new YAMLCoder().encode(actual), record, className, methodName);
        }

        public static void VerifyDirectory(string path, bool record = false, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
            string[] allFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(filePath => {
                return Path.GetFullPath(filePath);
            }).ToArray();

            Verify(String.Join("\n", allFiles), record, className, methodName);
        }

        public static void Verify(String actual, bool record = false, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
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
                File.WriteAllText(actualPath, actual);
            }

            var expected = File.ReadAllText(expectedPath);
            Assert.AreEqual(expected, actual);
        }

        public static void Verify(byte[] actual, bool record = false, [CallerFilePath] string className = "", [CallerMemberName] string methodName = "") {
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
                File.WriteAllBytes(actualPath, actual);
            }

            var expected = File.ReadAllBytes(expectedPath);
            Assert.AreEqual(Encoding.ASCII.GetString(expected), Encoding.ASCII.GetString(actual));
        }
    }
}