using KPatcher.Installation.Paths;
using KPatcher.Patching.Models;
using KPatcher.Patching.Models.InstallCommands;
using KPatcherBase.Models.Install;
using KPatcherBase.Source.Models.Mod;
using KPatcherBase.Source.Patching.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KPatcher.Patching.Parsing {
    public static class TSLPatcherModParsingExtensions {

        static class Constants {
            public const String KPatchFilter = "*.kpatch";
            public const String TSLPatcherIni = "*.ini";
            public const String TSLPatchData = "tslpatchdata";
        }

        public static class ParsingRegex {
            public const String EntryTermPair = "^[^=\\n\\r;]+=[^=\\n\\r;]*";
            public const String Whitespace = "[\\n\\s\\t\\r]*|(;[\\s\\t=]*)";
            public const String TLKList = "\\[TLKList\\]";
            public const String InstallList = "\\[InstallList\\]";
            public const String EntryTermRegex = "^[^\\n\\t\\r\\s=;]*[^\\n\\r=]";
            public const String EntryPairRegex = "^[^\\n\\t\\r\\s;]*=[^\\n\\t\\r\\s;]*";
            public const String EntrySeparatorRegex = "^[\\n\\t\\r\\s=;]*";
        }

        public static ModPackage[] loadTSLPatcherPackages(this ModParser parser) {
            string[] directories = Directory.GetDirectories(PatcherPath.Mods).Where(
                    directory => Directory.GetFiles(directory, Constants.TSLPatcherIni, SearchOption.AllDirectories).Count() > 0
                ).ToArray();
            List<ModPackage> packages = new List<ModPackage>();

            foreach (string directory in directories) {
                String readablePath = directory;
                Console.Write(String.Format("Converting TSLPatcher Data in {0}...", readablePath));
                try {
                    packages.Add(parser.generateTSLPatcherModPackage(directory));
                    Console.Write("Success\n");
                } catch (Exception e) {
                    Console.Write(String.Format("Failed\n {0}", e.Message));
                }
            }

            return packages.ToArray();
        }

        public static ModPackage generateTSLPatcherModPackage(this ModParser parser,
                                                              String directoryPath)
        {
            List<ModPackageComponent> components = new List<ModPackageComponent>();
            string[] fileNames = Directory.GetFiles(directoryPath, Constants.TSLPatcherIni, SearchOption.AllDirectories).ToArray();
            string nameSpacesPath = fileNames.Where(fileName => Path.GetFileName(fileName).Equals("namespaces.ini")).FirstOrDefault();

            if (nameSpacesPath != null) {
                components.Add(parser.generateTSLModPackageComponentFromNamespacesIni(nameSpacesPath));
            } else {
                string changesIniPath = fileNames.Where(fileName => Path.GetFileName(fileName).Equals("changes.ini")).FirstOrDefault();
                components.AddRange(parser.generateTSLModPackageComponentsFromChangesIni(changesIniPath));
            }

            return new ModPackage(directoryPath, Path.GetDirectoryName(directoryPath), "1.0", components.ToArray());
        }

        private static ModPackageComponent[] generateTSLModPackageComponentsFromChangesIni(this ModParser parser, String filePath) {
            String readablePath = Path.GetFileName(filePath);
            ModPackageComponent[] components = new ModPackageComponent[] { };
            Console.Write(String.Format("\n\tReading {0}...", readablePath));
            try {
                //do tsl package parsing here
                components = parser.parseModPackageComponents(filePath);
                Console.Write("Success\n");
            } catch (Exception e) {
                Console.Write(String.Format("Failed\n {0}", e.Message));
            }

            return components;
        }
        private static ModPackageComponent generateTSLModPackageComponentFromNamespacesIni(this ModParser parser, String filePath) {
            NamespacesInfo namespacesInfo = parser.loadNamespacesInfo(File.ReadAllText(filePath));

            ModOption optionVar = new ModOption("namespaces", namespacesInfo.optionMap.Values.ToList());

            List<ModPackageComponent> components = new List<ModPackageComponent>();


            Dictionary<ModIfStatementClause, ModPackageComponent[]> ifMap = new Dictionary<ModIfStatementClause, ModPackageComponent[]>();
            

            foreach (KeyValuePair<String, String> pair in namespacesInfo.optionMap) {
                ModBooleanExpression boolExpression = new ModBooleanExpression("namespaces", pair.Key, ModBooleanOperator.Equals);

                String changesIniPath = PatcherPath.Mods + "\\" + pair.Value;
                ModPackageComponent[] changesComponents = parser.generateTSLModPackageComponentsFromChangesIni(changesIniPath);
            }

            return new ModIfStatement(ifMap);
        }

        private static NamespacesInfo loadNamespacesInfo(this ModParser parser, String fileText) {
            Dictionary<String, String> namespacesDictionary = parser.parseEntriesForTitle("Namespaces", fileText);
            Dictionary<String, String> optionMap = new Dictionary<String, String>();

            foreach (String option in namespacesDictionary.Values) {
                Dictionary<String, String> namespaceInfo = parser.parseEntriesForTitle(option, fileText);
                String iniName = namespaceInfo["IniName"];
                String dataPath = namespaceInfo["DataPath"];
                optionMap[option] = dataPath + "\\" + iniName;
            }
            return new NamespacesInfo(optionMap);
        }

        private static Dictionary<String, String> parseEntriesForTitle(this ModParser modParser,
                                                                       String title,
                                                                       String fileText)
        {
            Match titleMatch = Regex.Match(fileText, String.Format("\\[{0}\\]", title));
            String shortenedText = fileText.Remove(0, titleMatch.Index + titleMatch.Value.Length);
            shortenedText = shortenedText.Remove(0, Regex.Match(shortenedText, ParsingRegex.Whitespace).Value.Length);

            Dictionary<string, string> map = new Dictionary<string, string>();

            Match termMatch = Regex.Match(shortenedText, ParsingRegex.EntryTermPair);

            shortenedText = shortenedText.Remove(0, termMatch.Value.Length);

            while (!shortenedText.StartsWith("[")
                && termMatch.Value.Length > 0) {
                String termString = termMatch.Value;
                String commandID = Regex.Match(termString, "[^=]*").Value;
                String value = Regex.Match(termString, "[^=]*$").Value;
                if (commandID.Contains(' ')) {
                    commandID = String.Format("\"{0}\"", commandID);
                }
                if (value.Contains(' ')) {
                    value = String.Format("\"{0}\"", value);
                }
                map[commandID] = value;
                shortenedText = shortenedText.Remove(0, Regex.Match(shortenedText, ParsingRegex.Whitespace).Value.Length);
                termMatch = Regex.Match(shortenedText, ParsingRegex.EntryTermPair);
                shortenedText = shortenedText.Remove(0, termMatch.Length);
            }
            return map;
        }

        private static ModPackageComponent[] parseModPackageComponents(this ModParser parser, string filePath) {
            String fileText = File.ReadAllText(filePath);

            Dictionary<string, string> tlkEntryMap = parser.parseEntriesForTLKList(fileText);
            ModFileInstallSet[] installCommands = parser.parseEntriesForInstallList(fileText);

            return installCommands;
/*            TP2daCommand[] _2daCommands = parseEntriesFor2daList(fileText);
            TPGFFCommandSet[] gffCommands = parseEntriesForGFFList(fileText);
            TPCompileCommand[] scriptCompileCommands = parseEntriesForScriptList(fileText);
            TPSSFCommand[] ssfCommands = parseEntriesForSSFList(fileText);

            return new TPPatchPackage(modName,
                modPath,
                tlkEntryMap,
                installCommands,
                _2daCommands,
                gffCommands,
                scriptCompileCommands,
                ssfCommands);*/
        }

        private static Dictionary<string, string> parseEntriesForTLKList(this ModParser parser, string fileText) {
            return parser.parseEntriesForTitle("TLKList", fileText);
        }

        private static ModFileInstallSet[] parseEntriesForInstallList(this ModParser parser, String fileText) {
            Dictionary<string, String> installList = parser.parseEntriesForTitle("InstallList", fileText);

            List<ModFileInstallSet> installSets = new List<ModFileInstallSet>();
            foreach (KeyValuePair<string, string> pair in installList) {
                Dictionary<String, String> entriesMap = parser.parseEntriesForTitle(pair.Key, fileText);
                ModFileInstallCommand[] commandEntries = entriesMap.Select(entryPair => new SingleFileCopyInstallCommand(entryPair.Value, entryPair.Value)).ToArray();
                installSets.Add(new ModFileInstallSet(pair.Value, commandEntries));
            }
            return installSets.ToArray();
        }

        private static TP2daCommand[] parseEntriesFor2daList(String fileText) {
            Dictionary<string, string> _2daList = parseEntriesForTitle("2DAList", fileText);

            List<TP2daCommand> _2daCommands = new List<TP2daCommand>();
            foreach (KeyValuePair<string, string> pair in _2daList) {

                Dictionary<String, String> entriesMap = parseEntriesForTitle(pair.Value, fileText);

                List<TP2daAddRowCommand> addRowCommands = new List<TP2daAddRowCommand>();
                foreach (KeyValuePair<string, string> entryPair in entriesMap) {
                    Dictionary<string, string> rowEntries = parseEntriesForTitle(entryPair.Value, fileText);
                    addRowCommands.Add(new TP2daAddRowCommand(rowEntries));
                }

                _2daCommands.Add(new TP2daCommand(pair.Value, addRowCommands.ToArray()));
            }
            return _2daCommands.ToArray();
        }

        private static TPGFFCommandSet[] parseEntriesForGFFList(String fileText) {
            Dictionary<string, string> gffList = parseEntriesForTitle("GFFList", fileText);

            List<TPGFFCommandSet> gffCommands = new List<TPGFFCommandSet>();
            foreach (String gffName in gffList.Values) {
                Dictionary<String, String> entriesMap = parseEntriesForTitle(gffName, fileText);
                //TODO: Check to see override is in fact the backup destination
                String installPath = "override";
                if (entriesMap.ContainsKey("!Destination")) {
                    installPath = entriesMap["!Destination"];
                }
                Boolean replaceFile = false;
                if (entriesMap.ContainsKey("!ReplaceFile")) {
                    replaceFile = entriesMap["!ReplaceFile"] != "0";
                }

                List<UPGFFCommand> gffFieldCommands = new List<UPGFFCommand>();
                foreach (KeyValuePair<string, string> entryPair in entriesMap) {
                    if (!entryPair.Key.StartsWith("!")) {
                        if (entryPair.Key.StartsWith(TPFieldTerm.AddField)) {
                            gffFieldCommands.Add(parseGFFAddFieldCommand(entryPair.Value, fileText));
                        } else {
                            gffFieldCommands.Add(new UPGFFSetFieldCommand(entryPair.Key, entryPair.Value));
                        }
                    }
                }

                gffCommands.Add(new TPGFFCommandSet(gffName, replaceFile, gffFieldCommands.ToArray()));
            }

            return gffCommands.ToArray();
        }

        private static UPGFFFieldObject parseGFFFieldObject(String title, String fileText) {
            return parseGFFAddFieldCommand(title, fileText).dataObject;
        }

        private static UPGFFAddFieldCommand parseGFFAddFieldCommand(String title, String fileText) {
            Dictionary<string, string> gffFieldEntries = parseEntriesForTitle(title, fileText);
            GFFFieldType fieldType = gffFieldEntries[TPFieldTerm.FieldType].toGFFFieldType();
            gffFieldEntries.Remove(TPFieldTerm.FieldType);

            String label = "";

            if (gffFieldEntries.ContainsKey(TPFieldTerm.Label)) {
                label = gffFieldEntries[TPFieldTerm.Label];
            }

            UPGFFFieldObject dataObject = null;
            switch (fieldType) {
                case GFFFieldType.BYTE:
                    byte b;
                    byte.TryParse(gffFieldEntries[TPFieldTerm.Value], out b);
                    dataObject = new UPGFFByteObject(b);
                    break;
                case GFFFieldType.CHAR:
                    char c;
                    char.TryParse(gffFieldEntries[TPFieldTerm.Value], out c);
                    dataObject = new UPGFFCharObject(c);
                    break;
                case GFFFieldType.WORD:
                    ushort uShortValue;
                    ushort.TryParse(gffFieldEntries[TPFieldTerm.Value], out uShortValue);
                    dataObject = new UPGFFDWordObject(uShortValue);
                    break;
                case GFFFieldType.SHORT:
                    short shortValue;
                    short.TryParse(gffFieldEntries[TPFieldTerm.Value], out shortValue);
                    dataObject = new UPGFFShortObject(shortValue);
                    break;
                case GFFFieldType.DWORD:
                    uint uintValue;
                    uint.TryParse(gffFieldEntries[TPFieldTerm.Value], out uintValue);
                    dataObject = new UPGFFDWordObject(uintValue);
                    break;
                case GFFFieldType.INT:
                    int intValue;
                    int.TryParse(gffFieldEntries[TPFieldTerm.Value], out intValue);
                    dataObject = new UPGFFIntObject(intValue);
                    break;
                case GFFFieldType.DWORD64:
                    ulong dword64Value;
                    ulong.TryParse(gffFieldEntries[TPFieldTerm.Value], out dword64Value);
                    dataObject = new UPGFFDWord64Object(dword64Value);
                    break;
                case GFFFieldType.INT64:
                    long int64Value;
                    long.TryParse(gffFieldEntries[TPFieldTerm.Value], out int64Value);
                    dataObject = new UPGFFInt64Object(int64Value);
                    break;
                case GFFFieldType.FLOAT:
                    float floatValue;
                    float.TryParse(gffFieldEntries[TPFieldTerm.Value], out floatValue);
                    dataObject = new UPGFFFloatObject(floatValue);
                    break;
                case GFFFieldType.DOUBLE:
                    double doubleValue;
                    double.TryParse(gffFieldEntries[TPFieldTerm.Value], out doubleValue);
                    dataObject = new UPGFFDoubleObject(doubleValue);
                    break;
                case GFFFieldType.CEXOSTRING:
                    String cexoString = gffFieldEntries[TPFieldTerm.Value];
                    dataObject = new UPGFFCExoStringObject(cexoString);
                    break;
                case GFFFieldType.RESREF:
                    String resref = gffFieldEntries[TPFieldTerm.Value];
                    dataObject = new UPGFFResrefObject(resref);
                    break;
                case GFFFieldType.CEXOLOCSTRING:
                    int strref;
                    int.TryParse(gffFieldEntries[TPFieldTerm.StrRef], out strref);
                    Dictionary<GFFLanguage, string> locStrings = new Dictionary<GFFLanguage, string>();
                    foreach (KeyValuePair<string, string> gffEntry in gffFieldEntries) {
                        if (gffEntry.Key.Contains(TPFieldTerm.Lang)) {
                            String keyInt = gffEntry.Key.Replace(TPFieldTerm.Lang, "");
                            int intID;
                            int.TryParse(keyInt, out intID);
                            GFFLanguage languageID = (GFFLanguage)intID;
                            locStrings[languageID] = gffEntry.Value;
                        }
                    }
                    dataObject = new UPGFFCExoLocStringObject(strref, locStrings);
                    break;
                case GFFFieldType.VOID:
                    dataObject = new UPGFFVoidObject(new byte[] { });
                    break;
                case GFFFieldType.STRUCT:
                    List<UPGFFAddFieldCommand> addFieldCommands = new List<UPGFFAddFieldCommand>();
                    foreach (KeyValuePair<string, string> gffEntry in gffFieldEntries) {
                        if (gffEntry.Key.Contains(TPFieldTerm.AddField)) {
                            addFieldCommands.Add(parseGFFAddFieldCommand(gffEntry.Value, fileText));
                        }
                    }
                    if (gffFieldEntries.ContainsKey(TPFieldTerm.Path)) {
                        label = gffFieldEntries[TPFieldTerm.Path];
                    }
                    int structID;
                    int.TryParse(gffFieldEntries[TPFieldTerm.TypeID], out structID);
                    dataObject = new UPGFFStructObject(structID, addFieldCommands.ToArray());
                    break;
                case GFFFieldType.LIST:
                    List<UPGFFListStructCommand> addFieldListCommands = new List<UPGFFListStructCommand>();
                    foreach (KeyValuePair<string, string> gffEntry in gffFieldEntries) {
                        if (gffEntry.Key.Contains(TPFieldTerm.AddField)) {
                            UPGFFStructObject structObject = parseGFFFieldObject(gffEntry.Value, fileText) as UPGFFStructObject;
                            addFieldListCommands.Add(new UPGFFAddStructCommand(structObject, UPGFFListStructAdditionLocation.First));
                        }
                    }
                    dataObject = new UPGFFListObject(addFieldListCommands.ToArray());
                    break;
                case GFFFieldType.QUATERNION:
                    String orientationString = gffFieldEntries[TPFieldTerm.Value];
                    String[] orientationValues = orientationString.Split('|');
                    float x;
                    float.TryParse(orientationValues[0], out x);
                    float y;
                    float.TryParse(orientationValues[1], out y);
                    float z;
                    float.TryParse(orientationValues[2], out z);
                    float w;
                    float.TryParse(orientationValues[3], out w);
                    dataObject = new UPGFFQuaternionObject(x, y, z, w);
                    break;
                case GFFFieldType.VECTOR:
                    String vectorString = gffFieldEntries[TPFieldTerm.Value];
                    String[] vectorValues = vectorString.Split('|');
                    float xv;
                    float.TryParse(vectorValues[0], out xv);
                    float yv;
                    float.TryParse(vectorValues[1], out yv);
                    float zv;
                    float.TryParse(vectorValues[2], out zv);
                    dataObject = new UPGFFVectorObject(xv, yv, zv);
                    break;
                default:
                    break;
            }
            return new UPGFFAddFieldCommand(label, dataObject);
        }

        private static TPCompileCommand[] parseEntriesForScriptList(String fileText) {
            List<TPCompileCommand> scriptCommands = new List<TPCompileCommand>();
            Dictionary<string, string> scriptList = parseEntriesForTitle("CompileList", fileText);
            foreach (KeyValuePair<string, string> pair in scriptList) {
                scriptCommands.Add(new TPCompileCommand(pair.Key, pair.Value));
            }
            return scriptCommands.ToArray();
        }

        private static TPSSFCommand[] parseEntriesForSSFList(String fileText) {
            List<TPSSFCommand> ssfCommands = new List<TPSSFCommand>();
            Dictionary<string, string> ssfList = parseEntriesForTitle("SSFList", fileText);
            foreach (KeyValuePair<string, string> pair in ssfList) {
                ssfCommands.Add(new TPSSFCommand());
            }
            return ssfCommands.ToArray();
        }

        private static Dictionary<String, String> parseEntriesForTitle(String title, String fileText) {
            Match titleMatch = Regex.Match(fileText, String.Format("\\[{0}\\]", title));
            String shortenedText = fileText.Remove(0, titleMatch.Index + titleMatch.Value.Length);
            shortenedText = shortenedText.Remove(0, Regex.Match(shortenedText, TPRegex.Whitespace).Value.Length);

            Dictionary<string, string> map = new Dictionary<string, string>();

            Match termMatch = Regex.Match(shortenedText, TPRegex.EntryTermPair);

            //Match termMatch = Regex.Match(shortenedText, TPRegex.EntryTermPair);
            shortenedText = shortenedText.Remove(0, termMatch.Value.Length);

            while (!shortenedText.StartsWith("[")
                && termMatch.Value.Length > 0) {
                String termString = termMatch.Value;
                String commandID = Regex.Match(termString, "[^=]*").Value;
                String value = Regex.Match(termString, "[^=]*$").Value;
                if (commandID.Contains(' ')) {
                    commandID = String.Format("\"{0}\"", commandID);
                }
                if (value.Contains(' ')) {
                    value = String.Format("\"{0}\"", value);
                }
                map[commandID] = value;
                shortenedText = shortenedText.Remove(0, Regex.Match(shortenedText, TPRegex.Whitespace).Value.Length);
                termMatch = Regex.Match(shortenedText, TPRegex.EntryTermPair);
                shortenedText = shortenedText.Remove(0, termMatch.Length);
            }
            return map;
        }*/
    }
}
