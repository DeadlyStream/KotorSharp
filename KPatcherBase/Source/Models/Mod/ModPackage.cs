using KPatcher.Patching.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {
    public class ModPackage {
        public String path;
        public String modName;
        public String modVersion;
        public ModPackageComponent[] instructions;

        public ModPackage(String path, String name, String version, ModPackageComponent[] instructions) {
            this.path = path;
            this.modName = name;
            this.modVersion = version;
            this.instructions = instructions;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0} {1}:\n", modName,
                                                  modVersion));
            foreach (ModPackageComponent instruction in instructions) {
                sb.Append(instruction.ToString());
            }
            sb.AppendFormat("{0}\n", ReservedWord.General.End);
            return sb.ToString();
        }
    }
}
