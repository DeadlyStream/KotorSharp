using AuroraIO.Source.Models.Dictionary;
using KPatcher.Source.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KPatcher.Source.Patcher.Patcher;

namespace KPatcher.Source.Patcher {
    internal static class GFFPatcher {

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
            arStruct.setValueForKey(keyPath, value);
        }
    }
}
