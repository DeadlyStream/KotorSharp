using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Install {
    public class ModInstallInfo {
        public Dictionary<String, ModInstallOptionMap> map;

        public ModInstallInfo(Dictionary<String, ModInstallOptionMap> map) {
            this.map = map;
        }
    }
}
