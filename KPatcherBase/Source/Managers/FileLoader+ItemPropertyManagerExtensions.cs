using AppToolbox.Source.Managers;
using AuroraIO.Collections;
using AuroraIO.Models;
using KotorManifest.Source.Constants;
using KPatcher.Installation.Paths;
using KPatcherBase.Models;
using KPatcherBase.Models.Install;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace KPatcherBase.Source.Managers {
    public static class FileLoader_ItemPropertyManagerExtensions {

        private static class Constant {
            public static readonly String ChitinKeyPath = GamePath.Root + "\\chitin.key";
            public static readonly String ItemPropertiesJSONPath = PatcherPath.UserInfo + "\\itempropertymanager.json";
        }

        public static Dictionary<int, ItemPropertyInfoModel> loadCachedProperties(this FileLoader fileLoader, String filePath) {
            if (File.Exists(filePath)) {
                return JsonConvert.DeserializeObject<Dictionary<int, ItemPropertyInfoModel>>(File.ReadAllText(filePath));
            } else {
                return null;
            }
        }

        public static Dictionary<int, ItemPropertyInfoModel> loadFromBif(this FileLoader loader) {
            KeyArchive keyArchive = new KeyArchive(Constant.ChitinKeyPath);
            BiffArchive bifArchive = keyArchive.loadBifArchive(KotorConstants.Bifs._2da.BifKey);
            bifArchive.preloadResources();
            Array2D itemProps = (Array2D)bifArchive.extractResource(KotorConstants.Bifs._2da.ItemPropDef2da);
            Array2D costTable = (Array2D)bifArchive.extractResource(KotorConstants.Bifs._2da.CostTable2da);
            Array2D itemParams = (Array2D)bifArchive.extractResource(KotorConstants.Bifs._2da.ParamTable2da);

            Dictionary<int, ItemPropertyInfoModel> itemPropertyMap = new Dictionary<int, ItemPropertyInfoModel>();

            Dictionary<string, Array2D> loadedTables = new Dictionary<string, Array2D>();

            for (int i = 0; i < itemProps.rowCount; i++) {
                Array2D.Row r = itemProps[i];
                String propertyName = r["label"];
                String subtype2da = r["subtyperesref"];
                Dictionary<string, int> subTypeMap = new Dictionary<string, int>();
                if (!subtype2da.Equals("****")) {
                    string subType2daKey = subtype2da + ".2da";

                    Array2D subtypeProps = null;
                    if (loadedTables.ContainsKey(subType2daKey)) {
                        subtypeProps = loadedTables[subType2daKey];
                    } else {
                        subtypeProps = (Array2D)bifArchive.extractResource(subType2daKey);
                        loadedTables[subType2daKey] = subtypeProps;
                    }

                    for (int j = 0; j < subtypeProps.rowCount; j++) {
                        Array2D.Row subR = subtypeProps[j];
                        String subPropertyName = subR["label"];
                        if (subPropertyName == null) {
                            subPropertyName = subR["name"];
                        }
                        subTypeMap[subPropertyName] = j;
                    }
                }
                int costTableIndex = -1;
                try {
                    costTableIndex = int.Parse(r["costtableresref"]);
                } catch { }
                Dictionary<string, int> costOptionMap = new Dictionary<string, int>();
                if (costTableIndex > -1) {
                    for (int j = 0; j < costTable.rowCount; j++) {
                        Array2D.Row subR = costTable[costTableIndex];
                        String costTableSubTable = subR["name"];

                        string costTableSubTableKey = costTableSubTable + ".2da";
                        Array2D costSubTable = null;
                        if (loadedTables.ContainsKey(costTableSubTable)) {
                            costSubTable = loadedTables[costTableSubTableKey];
                        } else {
                            costSubTable = (Array2D)bifArchive.extractResource(costTableSubTableKey);
                            loadedTables[costTableSubTableKey] = costSubTable;
                        }

                        for (int k = 0; k < costSubTable.rowCount; k++) {
                            Array2D.Row costSubR = costSubTable[k];
                            String key = costSubR["label"];
                            costOptionMap[key] = k;
                        }
                    }
                }

                int param1Index = -1;
                try {
                    param1Index = int.Parse(r["param1resref"]);
                } catch { }
                Dictionary<string, int> param1Map = new Dictionary<string, int>();
                if (param1Index > -1) {
                    for (int j = 0; j < itemParams.rowCount; j++) {
                        Array2D.Row subR = itemParams[param1Index];
                        String itemParamsSubtableString = subR["tableresref"];

                        string itemParamsSubTableKey = itemParamsSubtableString + ".2da";
                        Array2D itemParamsSubTable = null;
                        if (loadedTables.ContainsKey(itemParamsSubtableString)) {
                            itemParamsSubTable = loadedTables[itemParamsSubTableKey];
                        } else {
                            itemParamsSubTable = (Array2D)bifArchive.extractResource(itemParamsSubTableKey);
                            loadedTables[itemParamsSubTableKey] = itemParamsSubTable;
                        }

                        for (int k = 0; k < itemParamsSubTable.rowCount; k++) {
                            Array2D.Row paramSubR = itemParamsSubTable[k];
                            String key = paramSubR["label"];
                            param1Map[key] = k;
                        }
                    }
                }

                itemPropertyMap[i] = new ItemPropertyInfoModel(propertyName,
                                                          subTypeMap,
                                                          costTableIndex,
                                                          costOptionMap,
                                                          param1Index,
                                                          param1Map);
            }
            return itemPropertyMap;
        }
    }
}
