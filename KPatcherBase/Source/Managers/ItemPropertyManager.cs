using AppToolbox.Context;
using AppToolbox.Source.Managers;
using AuroraIO;
using KPatcher.Installation.Paths;
using KPatcherBase.Models;
using KPatcherBase.Models.Install;
using KPatcherBase.Source.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Managers {
    public sealed class ItemPropertyManager {

        private static class Constant {
            public static readonly String ChitinKeyPath = GamePath.Root + "\\chitin.key";
            public static readonly String ItemPropertiesJSONPath = PatcherPath.UserInfo + "\\itempropertymanager.json";
        }

        private static readonly ItemPropertyManager instance = new ItemPropertyManager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ItemPropertyManager() {
        }

        public static ItemPropertyManager Instance {
            get {
                return instance;
            }
        }

        private readonly Dictionary<string, int> propertyIndexMap = new Dictionary<string, int>();
        public readonly Dictionary<int, ItemPropertyInfoModel> itemPropertyMap = new Dictionary<int, ItemPropertyInfoModel>();


        private ItemPropertyManager() {
            //try to load from json
            
            try {
                itemPropertyMap = ApplicationContext.Shared.fileLoader.loadCachedProperties(Constant.ItemPropertiesJSONPath);
            } catch {
                itemPropertyMap = ApplicationContext.Shared.fileLoader.loadFromBif();
//                File.WriteAllText(Constant.ItemPropertiesJSONPath,
//                                  JsonConvert.SerializeObject(convertedDict));
            }

            propertyIndexMap = itemPropertyMap.ToDictionary(pair => pair.Value.propertyName, pair => pair.Key);
            //otherwise load from bif and save to json
        }

        public ItemBlueprintPropertyInfo generateItemBlueprintProperty(string propertyName, string subType, string cost, string param1) {
            int propertyValue = -1;
            int subtypeValue = -1;
            int costValue = -1;
            int paramValue = -1;

            if (propertyIndexMap.ContainsKey(propertyName)) {
                propertyValue = propertyIndexMap[propertyName];
            }

            ItemPropertyInfoModel propertyInfo = propertyInfoForPropertyName(propertyName);


            if (subType != null && propertyInfo.subTypeMap.ContainsKey(subType)) {
                subtypeValue = propertyInfo.subTypeMap[subType];
            }

            if (cost != null && propertyInfo.costValueMap.ContainsKey(cost)) {
                costValue = propertyInfo.costValueMap[cost];
            }

            if (param1 != null && propertyInfo.param1Map.ContainsKey(param1)) {
                paramValue = propertyInfo.param1Map[param1];
            }

            return new ItemBlueprintPropertyInfo(propertyValue,
                                          subtypeValue,
                                          propertyInfo.costTable,
                                          costValue,
                                          propertyInfo.param1,
                                          paramValue);
        }

        public ItemPropertyInfoModel propertyInfoForPropertyName(string propertyName) {
            int index = propertyIndexMap[propertyName];
            return itemPropertyMap[index];
        }
    }
}
