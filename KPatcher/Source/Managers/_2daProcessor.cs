using AppToolbox.Classes;
using AuroraIO.Models;
using KPatcherBase.Source.Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    class _2daProcessor {
        public struct Info {
            public Dictionary<String, int> varMap;

            public Info(Dictionary<string, int> varMap) {
                this.varMap = varMap;
            }
        }
        public static Info process(ChangesCollection changes, KeyValueManifest manifest) {

            foreach (KeyValuePair<String, InstructionSet[]> pair in changes.twoDAList) {
                Array2D table = PatchFileManager.shared.load2daFile(pair.Key);

                Log.infoLine(String.Format("Patching {0}", pair.Key));
                foreach (InstructionSet instruction in pair.Value) {
                    Array2D.Row row;
                    if (instruction.keyValueSet.ContainsKey("RowIndex")) {
                        int rowIndex = int.Parse(instruction.keyValueSet["RowIndex"]);
                        Log.debugLine(String.Format("Patching row {0}", rowIndex));
                        row = table[rowIndex];
                    } else {
                        Log.debugLine(String.Format("Adding new row {0}", table.rowCount));
                        row = table.addNewRow();
                    }

                    foreach (KeyValuePair<String, String> keyPair in instruction.keyValueSet) {
                        if (keyPair.Key.Contains("2DAMEMORY")) {
                            manifest[keyPair.Key] = table.rowCount - 1;
                        } else {
                            Object value = manifest[keyPair.Value] ?? keyPair.Value;
                            row[keyPair.Key] = value.ToString();
                        }
                    }
                }

                PatchFileManager.shared.write2daFileWithName(table, pair.Key);
            }
            
            return new Info();
        } 
    }
}
