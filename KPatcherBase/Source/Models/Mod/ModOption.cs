using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {
    public class ModOption: ModPackageComponent {
        private string optionName;
        private List<string> options;

        public ModOption(string optionName, List<string> options) {
            this.optionName = optionName;
            this.options = options;
        }
    }
}
