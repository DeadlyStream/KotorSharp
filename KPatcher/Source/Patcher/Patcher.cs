using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AuroraIO.Source.Models.TLK;
using KPatcher.Source.Extensions;
using KPatcher.Source.Ini;

namespace KPatcher.Source.Patcher {
    internal class Patcher {
        public struct PatchInfo {
            public string patchDataPath;
            public string gameRootDirectory;
            public TokenRegistry tokenRegistry;
            public IniObject changesIni;
            public PatchInfo(string patchDataPath,
                string gameRootDirectory,
                TokenRegistry tokenRegistry,
                IniObject changesIni
                ) {
                this.patchDataPath = patchDataPath;
                this.gameRootDirectory = gameRootDirectory;
                this.tokenRegistry = tokenRegistry;
                this.changesIni = changesIni;
            }
        }

        public static void Run(string changesPath, string gameDirectory, int logLevel, bool dryRun) {

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var changesIni = new IniParser().parse(changesPath);

            var patchInfo = new PatchInfo(
                Path.GetDirectoryName(changesPath),
                gameDirectory,
                new TokenRegistry(),
                changesIni);

            FileInterface fileInterface = new VirtualFileInterface();

            ProcessTLKList(patchInfo, fileInterface);
            ProcessInstallList(patchInfo, fileInterface);
            Process2DAList(patchInfo, fileInterface);
            ProcessGFFList(patchInfo, fileInterface);
            ProcessCompileList(patchInfo, fileInterface);
            ProcessSSFList(patchInfo, fileInterface);
            return;
        }

        private static void ProcessTLKList(PatchInfo patchInfo, FileInterface fileInterface) {
            var appendTLK = fileInterface.ReadTLK(Path.Combine(patchInfo.patchDataPath, "append.tlk"));
            var dialogTLK = fileInterface.ReadTLK(Path.Combine(patchInfo.gameRootDirectory, "dialog.tlk"));

            Console.WriteLine("Appending dialog.tlk");
            TLKPatcher.Process(patchInfo.changesIni["TLKList"], dialogTLK, appendTLK, patchInfo.tokenRegistry);

            fileInterface.WriteTLK(Path.Combine(patchInfo.gameRootDirectory, "dialog.tlk"), dialogTLK);
        }

        public static void ProcessInstallList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["InstallList"]) {
                string directory = pair.Value;
                Console.WriteLine(String.Format("Installing {0}", pair.Value));
                if (Regex.IsMatch(directory, @"\.(mod|hak|rim|erf|sav)$")) {
                    var archiveFilePath = Path.Combine(patchInfo.gameRootDirectory, directory);
                    var archive = fileInterface.ReadArchive(archiveFilePath);
                    InstallPatcher.ProcessArchiveInstall(archive, patchInfo.changesIni[pair.Key], patchInfo);
                    fileInterface.WriteArchive(archiveFilePath, archive);
                } else {
                    foreach(var fileCommandPair in patchInfo.changesIni[pair.Key]) {
                        bool overwrite = pair.Key.Contains("Replace") ? true : false;
                        fileInterface.Copy(
                            Path.Combine(patchInfo.patchDataPath, fileCommandPair.Value),
                            Path.Combine(patchInfo.gameRootDirectory, directory, fileCommandPair.Value),
                            overwrite
                        );
                    }
                }
            }
        }

        public static void Process2DAList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["2DAList"]) {
                Console.WriteLine(String.Format("Patching {0}", pair.Value));
                var table = fileInterface.Read2DA(Path.Combine(patchInfo.patchDataPath, pair.Value));
                foreach (var rowPair in patchInfo.changesIni[pair.Value]) {
                    if (Regex.IsMatch(rowPair.Key, @"ChangeRow")) {
                        _2DAPatcher.ProcessChangeRow(table, patchInfo.changesIni[rowPair.Value], patchInfo);
                    } else if (Regex.IsMatch(rowPair.Key, @"AddRow")) {
                        _2DAPatcher.ProcessAddRow(table, patchInfo.changesIni[rowPair.Value], patchInfo);
                    }
                } 
            }
        }

        public static void ProcessGFFList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["GFFList"]) {

                Console.WriteLine(String.Format("Patching {0}", pair.Value));
                var dict = fileInterface.ReadGFF(Path.Combine(patchInfo.patchDataPath, pair.Value));

                var changePairs = patchInfo.changesIni[pair.Value];

                foreach (var rowPair in changePairs) {
                    if (Regex.IsMatch(rowPair.Key, @"AddField")) {
                        GFFPatcher.ProcessAddField(dict, patchInfo.changesIni[rowPair.Value], patchInfo.tokenRegistry);
                    } else if (Regex.IsMatch(rowPair.Key, @"!Destination")) {
                        GFFPatcher.ProcessSetKeyPath(dict, rowPair.Key, rowPair.Value, patchInfo.tokenRegistry);
                    }   
                }

                var destination = changePairs.ContainsKey("!Destination") ? changePairs["!Destination"] : "override";
                //Write file
            }
        }

        public static void ProcessCompileList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["CompileList"]) {
                Console.WriteLine(String.Format("Compiling {0}", pair.Value));
                bool overwrite = pair.Key.Contains("Replace") ? true : false;
                var nssScriptSource = fileInterface.ReadText(Path.Combine(patchInfo.patchDataPath, pair.Value));

                NSSPatcher.ProcessScriptSource(nssScriptSource, patchInfo);

                var destinationDirectory = "Override";
                patchInfo.changesIni.SafeGetKey(pair.Value, (fileSection) => {
                    fileSection.SafeGetKey("!Destination", (value) => {
                        destinationDirectory = value;
                    });
                });
                fileInterface.WriteText(Path.Combine(patchInfo.gameRootDirectory, destinationDirectory, pair.Value), nssScriptSource, overwrite);
            }
        }

        public static void ProcessSSFList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["SSFList"]) {
                Console.WriteLine(String.Format("Patching {0}", pair.Value));
                bool overwrite = pair.Key.Contains("Replace") ? true : false;
                var soundSet = fileInterface.ReadSSF(Path.Combine(patchInfo.patchDataPath, pair.Value));

                patchInfo.changesIni.SafeGetKey(pair.Value, (values) => {
                    SSFPatcher.Process(soundSet, values, patchInfo.tokenRegistry);
                });

                fileInterface.WriteSSF(Path.Combine(patchInfo.gameRootDirectory, pair.Value), soundSet, overwrite);
            }
        }
    }
}
