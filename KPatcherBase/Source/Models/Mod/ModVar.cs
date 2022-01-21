using KPatcher.Patching.Models;
using KPatcher.Patching.Models.InstallCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {
    public class ModVar: ModPackageComponent,
        ModFilePatchCommandExpression,
        ModFileInstallCommand
    {
        private ModValueExpression expression;
        private string varName;

        public ModVar(string varName, ModValueExpression expression) {
            this.varName = varName;
            this.expression = expression;
        }

        public string evaluatedValue() {
            throw new NotImplementedException();
        }

        public void store(string value) {
            throw new NotImplementedException();
        }
    }
}
