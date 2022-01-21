using KPatcher.Patching.Models;
using KPatcher.Patching.Models.InstallCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcherBase.Source.Models.Mod {

    public class ModIfStatement: ModPackageComponent,
        ModFilePatchCommandExpression,
        ModFileInstallCommand
    {
        private Dictionary<ModIfStatementClause, ModPackageComponent[]> ifMap;

        public ModIfStatement(Dictionary<ModIfStatementClause, ModPackageComponent[]> ifMap) {
            this.ifMap = ifMap;
        }

        public string evaluatedValue() {
            throw new NotImplementedException();
        }

        public void store(string value) {
            throw new NotImplementedException();
        }
    }

    public class ModIfStatementClause {
        private ModBooleanExpression boolExpression;
        private ModIfStatementType ifType;

        public ModIfStatementClause(ModIfStatementType ifType,
            ModBooleanExpression boolExpression)
        {
            this.ifType = ifType;
            this.boolExpression = boolExpression;
        }
    }
}
