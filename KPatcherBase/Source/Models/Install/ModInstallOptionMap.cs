using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Install {
    public class ModInstallOptionMap {
        Dictionary<String, String> options;

        public ModInstallOptionMap(Dictionary<String, String> options) {
            this.options = options;
        }
    }
}
