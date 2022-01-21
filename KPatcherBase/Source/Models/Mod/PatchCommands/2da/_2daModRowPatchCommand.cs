using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.PatchCommands {
    class _2daModRowPatchCommand : ModFilePatchCommandExpression {
        private ModValueExpression expression;
        private Dictionary<string, string> patchValues;

        public _2daModRowPatchCommand(ModValueExpression expression, Dictionary<string, string> patchValues) {
            this.expression = expression;
            this.patchValues = patchValues;
        }

        public string evaluatedValue() {
            throw new NotImplementedException();
        }

        public void store(string value) {
            throw new NotImplementedException();
        }
    }
}
