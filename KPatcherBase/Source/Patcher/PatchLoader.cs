using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Patcher {
    public class PatchLoader {
        public NamespaceCollection readNamespaces(String filePath) {
            String fileText = File.ReadAllText(filePath);
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);
            Dictionary<String, NamespaceInfo> namespaces = data.Sections["Namespaces"].ToDictionary(
                item => item.Value, 
                item => {
                    KeyDataCollection namespaceData = data[item.Value];
                    return new NamespaceInfo(namespaceData["IniName"],
                        namespaceData["InfoName"],
                        namespaceData["DataPath"],
                        namespaceData["Name"],
                        namespaceData["Description"]);
                }
            );
            return new NamespaceCollection(namespaces);
        }

        public ChangesCollection readChanges(String filePath) {
            String fileText = File.ReadAllText(filePath);
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);
            KeyDataCollection settingsSection = data["Settings"];

            LogLevel logLevel = (LogLevel)int.Parse(settingsSection["LogLevel"]);

            Dictionary<String, int> tlkList = data["TLKList"].ToDictionary(
                item => item.KeyName,
                item => int.Parse(item.Value));

            Dictionary<String, String[]> installList = data["InstallList"].ToDictionary(
                item => item.Value,
                item => {
                    return data[item.KeyName].Select(subItem => subItem.Value).ToArray();
                });

            Dictionary<String, InstructionSet[]> twoDAList = data["2DAList"].ToDictionary(
                item => item.Value,
                item => {
                    return parseInstructions(data, item.Value).instructions.ToArray();
                }
            );

            Dictionary<String, InstructionSet> gffList = data["GFFList"].ToDictionary(
                item => item.Value,
                item => {
                    return parseInstructions(data, item.Value);
                }
            );

            Dictionary<String, InstructionSet> compileList = data["CompileList"].ToDictionary(
                item => item.Value,
                item => {
                    return parseInstructions(data, item.Value);
                });

            Dictionary<String, InstructionSet> ssfList = data["SSFList"].ToDictionary(
                item => item.Value,
                item => {
                    return parseInstructions(data, item.Value);
                });

            return new ChangesCollection(logLevel,
                                         tlkList,
                                         installList,
                                         twoDAList,
                                         gffList,
                                         compileList,
                                         ssfList
                );
        }

        private InstructionSet parseInstructions(IniData data, String key) {
            Dictionary<String, String> keyValueSet = new Dictionary<string, string>();
            List<InstructionSet> instructions = new List<InstructionSet>();

            foreach (KeyData keyData in data[key]) {
                if (keyData.KeyName.Contains("AddField")
                    || keyData.KeyName.Contains("AddRow")
                )
                {
                    instructions.Add(parseInstructions(data, keyData.Value));
                } else {
                    keyValueSet[keyData.KeyName] = keyData.Value;
                }
            }

            return new InstructionSet(keyValueSet, instructions.ToArray());
        }
    }
}
