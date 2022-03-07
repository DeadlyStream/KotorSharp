using AppToolbox.Classes;
using AuroraIO;
using AuroraIO.Models;
using AuroraIO.Source.Models.GFF;
using KotorManifest.Paths;
using KPatcherBase.Source.Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Source.Managers {
    class GFFProcessor {
        public static void process(ChangesCollection changes, KeyValueManifest manifest) {
            foreach (KeyValuePair<String, InstructionSet> pair in changes.gffList) {
                GFFObject gffObject = new GFFObject(AuroraResourceType.GFF);
                try {
                    gffObject = PatchFileManager.shared.loadGFF(pair.Key);
                } catch (Exception e) {
                    Log.warnLine(e.Message);
                }

                if (gffObject == null) {
                    continue;
                }

                try {
                    InstructionSet instruction = pair.Value;
                    Dictionary<String, String> keyValueSet = instruction.keyValueSet;

                    //Filter out keys with ! - theses are not keys to set in the gff file itself
                    var keysToAdd = instruction.keyValueSet.Where(kvPair => {
                        return !kvPair.Key.StartsWith("!");
                    });

                    var fieldsToAdd = pair.Value.instructions.Select(subInstruction => {
                        return processAddFieldInstruction(gffObject, subInstruction, "", manifest);
                    });

                    keysToAdd.ToList().ForEach(kvPair => {
                        gffObject[kvPair.Key] = manifest[kvPair.Value] ?? kvPair.Value;
                        Log.debugLine(String.Format("Set {0} for field {1}", kvPair.Value, kvPair.Key));
                    });

                    fieldsToAdd.ToList().ForEach(kvPair => {
                        gffObject[kvPair.Key] = kvPair.Value;
                        Log.debugLine(String.Format("Set {0} for field {1}", kvPair.Value, kvPair.Key));
                    });

                    KotorRootPath temp = PathManager.shared.temp;
                    ApplicationPath filePath;
                    if (keyValueSet.ContainsKey("!Destination")) {
                        filePath = temp + keyValueSet["!Destination"];
                    } else {
                        filePath = temp.Override + pair.Key;
                    }

                    PatchFileManager.shared.writeGFFFile(gffObject, filePath);
                } catch (Exception e) {
                    Log.warnLine(e.Message);
                }
            }
        }

        public static KeyValuePair<String, Object> processAddFieldInstruction(GFFObject gffObject, InstructionSet instruction, GFFPath rootPath, KeyValueManifest manifest) {
            //this is likely a new field
            GFFFieldType fieldType = instruction.keyValueSet["FieldType"].toGFFFieldType();
            String typeId = instruction.keyValueSet.ContainsKey("TypeId") ? instruction.keyValueSet["TypeId"] : null;
            String path = instruction.keyValueSet.ContainsKey("Path") ? instruction.keyValueSet["Path"] : "";
            String label = instruction.keyValueSet.ContainsKey("Label") ? instruction.keyValueSet["Label"] : null;
            String value = instruction.keyValueSet.ContainsKey("Value") ? instruction.keyValueSet["Value"] : null;
            GFFPath fullPath = path + label;
            String strRefKey = instruction.keyValueSet.ContainsKey("StrRef") ? instruction.keyValueSet["StrRef"] : "";
            Object strRefValue = manifest[strRefKey];

            if (fieldType == GFFFieldType.STRUCT) {
                Dictionary<String, Object> subFields = instruction.instructions.Select(
                    subInstruction => {
                    return processAddFieldInstruction(gffObject, subInstruction, rootPath + fullPath, manifest);
                }).ToDictionary(field => field.Key, field => field.Value);
                GFFStruct structInfo = new GFFStruct(uint.Parse(typeId));
                GFFStructDataObject dataObject = new GFFStructDataObject(structInfo);
                
                foreach(KeyValuePair<String, Object> pair in subFields) {
                    structInfo[pair.Key] = pair.Value;
                }
                return new KeyValuePair<String, Object>(fullPath, dataObject);
            } else {
                GFFFieldDataObject dataObject = null;
                switch (fieldType) {
                    case GFFFieldType.UNDEFINED:
                    break;
                    case GFFFieldType.BYTE:
                    dataObject = new GFFByteDataObject(Convert.ToByte(value));
                    break;
                    case GFFFieldType.CHAR:
                    dataObject = new GFFCharDataObject(Convert.ToChar(value));
                    break;
                    case GFFFieldType.WORD:
                    dataObject = new GFFWordDataObject(Convert.ToUInt16(value));
                    break;
                    case GFFFieldType.SHORT:
                    dataObject = new GFFShortDataObject(Convert.ToInt16(value));
                    break;
                    case GFFFieldType.DWORD:
                    dataObject = new GFFDWordDataObject(Convert.ToUInt32(value));
                    break;
                    case GFFFieldType.INT:
                    dataObject = new GFFIntDataObject(Convert.ToInt32(value));
                    break;
                    case GFFFieldType.DWORD64:
                    dataObject = new GFFDWord64DataObject(Convert.ToUInt64(value));
                    break;
                    case GFFFieldType.INT64:
                    dataObject = new GFFInt64DataObject(Convert.ToInt64(value));
                    break;
                    case GFFFieldType.FLOAT:
                    dataObject = new GFFFloatDataObject(Convert.ToSingle(value));
                    break;
                    case GFFFieldType.DOUBLE:
                    dataObject = new GFFDoubleDataObject(Convert.ToDouble(value));
                    break;
                    case GFFFieldType.CEXOSTRING:
                    dataObject = new GFFCExoStringDataObject(value);
                    break;
                    case GFFFieldType.RESREF:
                    dataObject = new GFFResrefDataObject(value);
                    break;
                    case GFFFieldType.CEXOLOCSTRING:
                    dataObject = new GFFCExoLocStringDataObject(Convert.ToUInt32(strRefValue), new Dictionary<GFFLanguage, String>());
                    break;
                    case GFFFieldType.VOID:
                    dataObject = new GFFVoidDataObject(Encoding.ASCII.GetBytes(value));
                    break;
                    case GFFFieldType.STRUCT:
                    break;
                    case GFFFieldType.LIST:
                    dataObject = new GFFListDataObject(new GFFStruct[0]);
                    break;
                    case GFFFieldType.QUATERNION:
                    break;
                    case GFFFieldType.VECTOR:
                    break;
                    default:
                    Log.debugLine(String.Format("Set new {0}: {1} : {2}", fieldType, label, value));
                    break;
                }

                KeyValuePair<string, GFFFieldDataObject> newFieldInfo = new KeyValuePair<string, GFFFieldDataObject>(label, dataObject);
                return new KeyValuePair<String, Object>(label, newFieldInfo);
            }
        }
    }
}
