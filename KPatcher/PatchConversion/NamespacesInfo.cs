using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.PatchConversion {
    public class NamespacesInfo {
        public Dictionary<String, String> optionMap { get; }

        public NamespacesInfo(Dictionary<String, String> optionMap) {
            this.optionMap = optionMap;
        }
    }
}
