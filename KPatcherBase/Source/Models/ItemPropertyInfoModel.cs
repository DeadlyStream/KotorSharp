using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Models {
    public class ItemPropertyInfoModel {
        public string propertyName;
        public int costTable;
        public Dictionary<string, int> subTypeMap = new Dictionary<string, int>();
        public Dictionary<string, int> costValueMap = new Dictionary<string, int>();
        public int param1;
        public Dictionary<string, int> param1Map = new Dictionary<string, int>();

        public ItemPropertyInfoModel(string propertyName,
            Dictionary<string, int> subTypeMap,
            int costOptionRow,
            Dictionary<string, int> costOptionMap,
            int param1Index,
            Dictionary<string, int> param1Map) {
            this.propertyName = propertyName;
            this.subTypeMap = subTypeMap;
            this.costTable = costOptionRow;
            this.costValueMap = costOptionMap;
            this.param1 = param1Index;
            this.param1Map = param1Map;
        }
    }
}
