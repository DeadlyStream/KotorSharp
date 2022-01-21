using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPatcher.Patching.Models.PatchCommands {
    class _2daClearRowPatchCommand: ModFilePatchCommandExpression {
        private ModValueExpression expression;

        public _2daClearRowPatchCommand(ModValueExpression expression) {
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
