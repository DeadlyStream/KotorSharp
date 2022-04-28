using AuroraIO.Source.Models.Dictionary;
using KPatcher.Source.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;

namespace KPatcher.Source.Patcher {
    internal static class GFFPatcher {

        public static void ProcessGFF(AuroraDictionary dict, Dictionary<string, string> pairs, PatchInfo patchInfo) {
            foreach (var pair in pairs) {
                if (Regex.IsMatch(pair.Key, @"AddField")) {
                    ProcessAddField(dict, patchInfo.changesIni[pair.Value], patchInfo.tokenRegistry);
                } else if (!Regex.IsMatch(pair.Key, @"!")) {
                    ProcessSetKeyPath(dict, pair.Key, pair.Value, patchInfo.tokenRegistry);
                }
            }
        }

        public static void ProcessAddField(AuroraStructType arStruct, Dictionary<string, string> values, TokenRegistry tokenRegistry) {
            switch (values["FieldType"]) {
                case "Struct":
                    int typeId = int.Parse(values["TypeId"]);
                    string path = values["Path"];
                    break;
                default:
                    
                    break;
            }
        }

        public static void ProcessSetKeyPath(AuroraStructType arStruct, String keyPath, String value, TokenRegistry tokenRegistry) {
            if (tokenRegistry.ContainsKey(value)) {
                arStruct.setValueForKey(keyPath, tokenRegistry[value]);
            } else {
                arStruct.setValueForKey(keyPath, value);
            }
        }
    }
}
