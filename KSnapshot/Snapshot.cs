using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        internal readonly string OutputDirectory;

        public static Bundle current([CallerFilePath] string className = "") {
            return new Bundle(className);
        }

        Bundle([CallerFilePath] string className = "") {
            ProjectDirectory = Path.GetDirectoryName(className);
            SnapshotDirectory = Path.Combine(ProjectDirectory, "Snapshots");
            OutputDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
    }

    public class ResourceBundle {

        public readonly string Directory;

        public byte[] GetFileBytes(string fileName) {
            return File.ReadAllBytes(GetFilePath(fileName));
        }

        public string GetFileText(string fileName) {
            return File.ReadAllText(GetFilePath(fileName)); 
        }

        public string GetDirectory(string directory) {
            return Path.Combine(Directory, directory);
        }

        public string GetFilePath(string fileName) {
            return Path.Combine(Directory, fileName);
        }

        public static ResourceBundle GetCurrent([CallerFilePath] string fileName = "") {
            return new ResourceBundle(Path.GetFileNameWithoutExtension(fileName));
        }

        private ResourceBundle(string directory) {
            Directory = Path.Combine(KSEnvironment.Bundle.SnapshotDirectory, "Resources", directory);
        }
    }

    public class KSEnvironment {
        public static Bundle Bundle = Bundle.current();
    }

    public static class Snapshot {

        static string expectedSnapshotFilePath(string className, string methodName) {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Artifacts",
                Path.GetFileNameWithoutExtension(className),
                methodName);
        }

        static string snapshotOutputDIrectory(string className, string methodName) {
            return Path.Combine(KSEnvironment.Bundle.OutputDirectory,
                "Artifacts",
                Path.GetFileNameWithoutExtension(className),
                methodName);
        }

        public static string PatchDataDirectory([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                String.Format("{0}\\{1}", Path.GetFileNameWithoutExtension(className), methodName),
                "tslpatchdata",
                "changes.ini");
        }

        public static string RootGameDirectory([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                "gameRoot");
        }

        public static string ResourceDirectory([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(KSEnvironment.Bundle.SnapshotDirectory,
                "Resources",
                String.Format("{0}\\{1}", Path.GetFileNameWithoutExtension(className), methodName));
        }

        public static string ResourcePath(String fileName, [CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return Path.Combine(ResourceDirectory(methodName, className), fileName);
        }

        public static byte[] DataResource([CallerMemberName] string methodName = "", [CallerFilePath] string className = "") {
            return File.ReadAllBytes(ResourcePath("dataResource", methodName, className));
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
            var expectedPath = expectedSnapshotFilePath(className, methodName);

            if (record) {
                if (!Directory.Exists(Path.GetDirectoryName(expectedPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(expectedPath));
                }
                File.WriteAllText(expectedPath, actual);
            } else {
                var expectedCleaned = File.ReadAllText(expectedPath).ReplaceLineEndings();
                var actualCleaned = actual.ReplaceLineEndings();

                var outputDirectory = snapshotOutputDIrectory(className, methodName);
                var expectedOutputPath = Path.Combine(outputDirectory, "expected");
                var actualOutputPath = Path.Combine(outputDirectory, "actual");

                if (expectedCleaned.Equals(actualCleaned)) {
                    Assert.IsTrue(true);

                    if (Directory.Exists(Path.GetDirectoryName(expectedOutputPath)))
                        Directory.Delete(Path.GetDirectoryName(expectedOutputPath), true);
                    if (Directory.Exists(Path.GetDirectoryName(actualOutputPath)))
                        Directory.Delete(Path.GetDirectoryName(actualOutputPath), true);
                    if (File.Exists(expectedOutputPath))
                        File.Delete(expectedOutputPath);
                } else {
                    if (!Directory.Exists(Path.GetDirectoryName(expectedOutputPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(expectedOutputPath));
                    if (!Directory.Exists(Path.GetDirectoryName(actualOutputPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(actualOutputPath));

                    if (File.Exists(actualOutputPath))
                        File.Delete(actualOutputPath);

                    File.Copy(expectedPath, expectedOutputPath, true);
                    File.WriteAllText(actualOutputPath, actualCleaned);
                    Assert.Fail(String.Format("Snapshots did not match, see expected vs. actual at {0}", Path.GetDirectoryName(outputDirectory)));
                }
            }
        }
    }
}