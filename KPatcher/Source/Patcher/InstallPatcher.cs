using AuroraIO.Source.Archives;
using AuroraIO.Source.Archives.ERFRIM;
using AuroraIO.Source.Coders;
using KPatcher.Source.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;

namespace KPatcher.Source.Patcher {
    public static class InstallPatcher {

        public static void ProcessArchiveInstall(AuroraArchive archive, string sourceDirectory, Dictionary<string, string> values, FileInterface fileInterface) {
            foreach (var pair in values) {
                try {
                    bool overwrite = pair.Key.Contains("Replace") ? true : false;
                    var sourceFilePath = Path.Combine(sourceDirectory, pair.Value);

                    archive.Add(
                        new AuroraFileEntry(
                            pair.Value,
                            fileInterface.Read(sourceFilePath)
                        )
                    );
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void ProcessDirectoryInstall(String directory, string sourceDirectory, Dictionary<string, string> values, FileInterface fileInterface) {
            foreach (var pair in values) {
                try {
                    bool overwrite = pair.Key.Contains("Replace") ? true : false;
                    var sourceFilePath = Path.Combine(sourceDirectory, pair.Value);
                    var destinationFilePath = Path.Combine(directory, pair.Value);
                    fileInterface.Copy(sourceFilePath, destinationFilePath, overwrite);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
