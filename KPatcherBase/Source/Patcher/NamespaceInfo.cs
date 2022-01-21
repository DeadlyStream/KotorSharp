using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Patcher {
    public struct NamespaceInfo {

        public String iniName;
        public String infoName;
        public String dataPath;
        public String name;
        public String description;

        internal NamespaceInfo(String iniName, String infoName, String dataPath, String name, String description) {
            this.iniName = iniName;
            this.infoName = infoName;
            this.dataPath = dataPath;
            this.name = name;
            this.description = description;
        }
    }
}
