﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AuroraIO.Source.Archives;
using AuroraIO.Source.Archives.ERFRIM;
using AuroraIO.Source.Coders;
using AuroraIO.Source.Models.Dictionary;
using AuroraIO.Source.Models.TLK;
using KPatcher.Source.Extensions;
using KPatcher.Source.Ini;

namespace KPatcher.Source.Patcher {
    public class Patcher {
        public struct PatchInfo {
            internal string patchDataPath;
            internal string gameRootDirectory;
            internal string backupDirectory;
            internal TokenRegistry tokenRegistry;
            internal IniObject changesIni;
            internal DateTime date;
            internal PatchInfo(
                string patchDataPath,
                string gameRootDirectory,
                TokenRegistry tokenRegistry,
                IniObject changesIni,
                DateTime date
            ) {
                this.patchDataPath = patchDataPath;
                this.gameRootDirectory = gameRootDirectory;
                this.backupDirectory = Path.Combine(gameRootDirectory, "backup", date.ToString("MMddyyyyHmmss"));
                this.tokenRegistry = tokenRegistry;
                this.changesIni = changesIni;
                this.date = date;
            }
        }

        public static void Run(string changesPath,
            string gameDirectory,
            FileInterface fileInterface,
            int logLevel = 0) {
            Run(changesPath, gameDirectory, fileInterface, DateTime.Now, logLevel);
        }

        public static void Run(string changesPath,
            string gameDirectory,
            FileInterface fileInterface,
            DateTime date,
            int logLevel = 0) {

            var changesIni = new IniParser().parse(changesPath);

            var patchInfo = new PatchInfo(
                Path.GetDirectoryName(changesPath),
                gameDirectory,
                new TokenRegistry(),
                changesIni,
                date);
            if (patchInfo.changesIni["TLKList"].Count > 1)
                ProcessTLKList(patchInfo, fileInterface);
            if (patchInfo.changesIni["2DAList"].Count > 1)
                Process2DAList(patchInfo, fileInterface);
            if (patchInfo.changesIni["InstallList"].Count > 1)
                ProcessInstallList(patchInfo, fileInterface);
            if (patchInfo.changesIni["GFFList"].Count > 1)
                ProcessGFFList(patchInfo, fileInterface);
            if (patchInfo.changesIni["CompileList"].Count > 1)
                ProcessCompileList(patchInfo, fileInterface);
            if (patchInfo.changesIni["SSFList"].Count > 1)
                ProcessSSFList(patchInfo, fileInterface);
            return;
        }

        public static void ProcessTLKList(PatchInfo patchInfo, FileInterface fileInterface) {
            var appendTLK = fileInterface.ReadTLK(Path.Combine(patchInfo.patchDataPath, "append.tlk"));
            var dialogTLKPath = Path.Combine(patchInfo.gameRootDirectory, "dialog.tlk");
            var dialogTLK = fileInterface.ReadTLK(dialogTLKPath);
            var backupDialogTLKPath = Path.Combine(
                patchInfo.backupDirectory,
                "dialog.tlk");
            fileInterface.Copy(dialogTLKPath, backupDialogTLKPath);

            Console.WriteLine("Appending dialog.tlk");
            TLKPatcher.Process(patchInfo.changesIni["TLKList"], dialogTLK, appendTLK, patchInfo.tokenRegistry);

            fileInterface.WriteTLK(dialogTLKPath, dialogTLK);
        }

        public static void ProcessInstallList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["InstallList"]) {
                string directory = pair.Value;
                Console.WriteLine(String.Format("Installing {0}", pair.Value));

                if (isArchive(directory)) {
                    var archiveFilePath = Path.Combine(patchInfo.gameRootDirectory, directory);
                    var archiveBackupFilePath = Path.Combine(patchInfo.backupDirectory, directory);
                    fileInterface.Copy(archiveFilePath, archiveBackupFilePath);

                    var archive = fileInterface.ReadArchive(archiveFilePath);

                    InstallPatcher.ProcessArchiveInstall(archive,
                        patchInfo.patchDataPath,
                        patchInfo.changesIni[pair.Key],
                        fileInterface);

                    fileInterface.WriteArchive(archiveFilePath, archive);
                } else {
                    InstallPatcher.ProcessDirectoryInstall(
                        Path.Combine(patchInfo.gameRootDirectory, directory),
                        patchInfo.patchDataPath,
                        patchInfo.changesIni[pair.Key],
                        fileInterface);
                }
            }

            //convert 
        }

        public static void Process2DAList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var _2daFilePair in patchInfo.changesIni["2DAList"]) {
                Console.WriteLine(String.Format("Patching {0}", _2daFilePair.Value));
                var table = fileInterface.Read2DA(Path.Combine(patchInfo.patchDataPath, _2daFilePair.Value));

                foreach (var rowInstructionPair in patchInfo.changesIni[_2daFilePair.Value]) {
                    var rowValuePairs = patchInfo.changesIni[rowInstructionPair.Value];
                    if (Regex.IsMatch(rowInstructionPair.Key, @"ChangeRow")) {
                        _2DAPatcher.ProcessChangeRow(table, rowValuePairs, patchInfo);
                    } else if (Regex.IsMatch(rowInstructionPair.Key, @"AddRow")) {
                        _2DAPatcher.ProcessAddRow(table, rowValuePairs, patchInfo);
                    }
                }

                fileInterface.Write2DA(Path.Combine(patchInfo.gameRootDirectory, "override", _2daFilePair.Value), table);
            }
        }

        public static void ProcessGFFList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["GFFList"]) {

                Console.WriteLine(String.Format("Patching {0}", pair.Value));
                try {
                    var changePairs = patchInfo.changesIni[pair.Value];
                    var destination = changePairs.ContainsKey("!Destination") ? changePairs["!Destination"] : "override";
                    var replaceFile = changePairs.ContainsKey("!ReplaceFile") ? changePairs["!ReplaceFile"] == "1" : false;

                    if (isArchive(destination)) {
                        var archiveFilePath = Path.Combine(patchInfo.gameRootDirectory, destination);
                        var archive = fileInterface.ReadArchive(archiveFilePath);
                        var dict = new GFFCoder().decode(archive.Get(pair.Value).file.data);

                        GFFPatcher.ProcessGFF(dict, changePairs, patchInfo);

                        fileInterface.WriteArchive(archiveFilePath, archive);
                    } else {

                        var filePath = Path.Combine(patchInfo.gameRootDirectory, destination, pair.Value);
                        var sourceFilePath = Path.Combine(patchInfo.patchDataPath, pair.Value);
                        if (!fileInterface.FileExists(filePath)) {
                            fileInterface.Copy(sourceFilePath, filePath);
                        }

                        AuroraDictionary dict = fileInterface.ReadGFF(filePath);

                        GFFPatcher.ProcessGFF(dict, changePairs, patchInfo);

                        fileInterface.WriteGFF(filePath, dict);
                    }
                 
                    //Write file
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void ProcessCompileList(PatchInfo patchInfo, FileInterface fileInterface) {
            foreach (var pair in patchInfo.changesIni["CompileList"]) {
                Console.WriteLine(String.Format("Compiling {0}", pair.Value));
                bool overwrite = pair.Key.Contains("Replace") ? true : false;
                var nssScriptSource = fileInterface.ReadText(Path.Combine(patchInfo.patchDataPath, pair.Value));

                NSSPatcher.ProcessScriptSource(nssScriptSource, patchInfo);

                var destination = "Override";
                patchInfo.changesIni.SafeGetKey(pair.Value, (fileSection) => {
                    fileSection.SafeGetKey("!Destination", (value) => {
                        destination = value;
                    });
                });

                if (isArchive(destination)) {
                    var archiveFilePath = Path.Combine(patchInfo.gameRootDirectory, destination);
                    var archive = fileInterface.ReadArchive(archiveFilePath);
                    archive.Add(new AuroraFileEntry(pair.Value, Encoding.ASCII.GetBytes(nssScriptSource)));

                    fileInterface.WriteArchive(archiveFilePath, archive);
                } else {
                    var directoryFilePath = Path.Combine(patchInfo.gameRootDirectory, destination, pair.Value);

                    fileInterface.WriteText(directoryFilePath, nssScriptSource, overwrite);
                }
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

        public static bool isArchive(string directory) {
            return Regex.IsMatch(directory, @"\.(mod|hak|rim|erf|sav)$");
        }
    }
}
